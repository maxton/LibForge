using LibForge.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace LibForge.Engine
{
  public class EntityLayer
  {
    public int Version;
    public int fileSlotIndex;
    public short TotalObjectLayers;
    public GameObject[] Objects;

    public void Load(Stream s, int index, int version)
    {
      var r = new BinReader(s);
      Version = r.Int();
      if (Version < 8)
        throw new InvalidDataException("Entity layer version should be > 8");
      if (Version >= 0x17)
      {
        r.Int(); // unknown
      }
      fileSlotIndex = r.Int();
      if (fileSlotIndex != index)
      {
        //throw new InvalidDataException("Entity layer failed to load (serialized to different slot)");
      }
      TotalObjectLayers = r.Short();
      Objects = new GameObject[r.Int()];
      for (int i = 0; i < Objects.Length; i++)
      {
        Objects[i] = new GameObject();
        Objects[i].Load(s, Version);
        if (Objects[i] != null && Objects[i].Rev >= 3 && Objects[i].Name.Length == 0)
        {
          // It seems like maybe these are "ghost objects" that should be overwritten??? Need more RE
          i--;
        }
      }
    }
  }

  public class Entity
  {
    public const int MaxVersion = 0x1E;
    public int Version;
    public EntityLayer[] Layers;

    public void Load(Stream s, EntityResource rsrc)
    {
      var r = new BinReader(s);
      Version = r.Int();
      if (Version > MaxVersion)
      {
        throw new Exception("Can't handle entity version " + Version);
      }
      if (Version <= 1)
      {
        throw new InvalidDataException("Entity version must be > 1");
      }
      if (Version <= 4)
      {
        r.String();
      }
      int numLayers = 1;
      if (Version >= 9)
      {
        numLayers = r.Int();
      }
      else
      {
        int rootId = r.Int();
        if (Version > 6)
        {
          numLayers = r.Int();
        }
        else
        {
          short arraySize = r.Short();
        }
      }
      for (int li = 0; li < numLayers; li++)
      {
        if (Version < 8)
        {
          if (Version >= 7)
            r.Short(); // layer_field_28
          if (numLayers != 1)
            throw new Exception("Num layers should be 1 for version <8");
          var propArrayBaseSize = r.Int();
          if (propArrayBaseSize > 0)
          {
            throw new NotImplementedException("Version < 8 should load objs here");
          }
        }
        else if (Version <= 11)
        {
          r.String(); // layer_name
        }
      }
      if (Version < 8)
      {
        throw new NotImplementedException("Version < 8 not implemented");
      }
      Layers = new EntityLayer[numLayers];
      LoadLayer(s, 0, rsrc);
      for (int i = 1; i < numLayers; i++)
      {
        LoadLayer(s, i, rsrc);
      }
    }
    private void LoadLayer(Stream s, int layerIndex, EntityResource rsrc)
    {
      Layers[layerIndex] = new EntityLayer();
      Layers[layerIndex].Load(s, layerIndex, Version);
      if (Version >= 0xC)
      {
        rsrc.LoadInlineResources(s, layerIndex);
      }
    }
  }
}
