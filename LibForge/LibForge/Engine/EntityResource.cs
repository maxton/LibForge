using LibForge.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace LibForge.Engine
{
  public class EntityResource : Resource
  {
    public EntityResource() { Type = "EntityResource"; }
    public const int MaxVersion = 0x11;
    public int Version;
    public string[] InlineLayerNames;
    public Entity Entity;
    public Resource[][] InlineResourceLayers;

    public override void Load(Stream s)
    {
      var r = new BinReader(s);
      Version = r.Int();
      if (Version < 0xC || Version > MaxVersion)
      {
        throw new InvalidDataException("Can't handle EntityResource version " + Version);
      }
      InlineLayerNames = r.Arr(r.String);
      InlineResourceLayers = new Resource[InlineLayerNames.Length][];
      if (Version >= 0xF)
      {
        var unk0 = r.Int();
        if (Version >= 0x10)
        {
          var unk1 = r.Int();
        }

      }
      Entity = new Entity();
      Entity.Load(s, this);
    }
    public void LoadInlineResources(Stream s, int layerIndex)
    {
      var r = new BinReader(s);
      if (layerIndex > InlineResourceLayers.Length)
        throw new InvalidDataException("More inline resource layers than entity resource layers");
      int version = r.Int();
      // HACK: Allows reading codemonkey_rbn.rbsong
      if (version == 1)
      {
        r.Int();
        r.Int();
        r.Int();
        version = r.Int();
      }
      int count = r.Int();
      var layer = InlineResourceLayers[layerIndex] = new Resource[count];
      for (int i = 0; i < count; i++)
      {
        var type = r.String();
        var inline = layer[i] = Resource.Create(type);
        inline.Path = r.String();
        inline.Load(s);
      }
    }
  }
}
