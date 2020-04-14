using LibForge.Engine;
using LibForge.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace LibForge.RBSong
{
  public class RBSongResourceWriter : WriterBase<RBSongResource>
  {
    public RBSongResourceWriter(Stream s) : base(s) { }
    public override void WriteStream(RBSongResource v)
    {
      WriteEntityResource(v);
    }
    public void WriteEntityResource(EntityResource v)
    {
      Write(v.Version);
      Write(v.InlineLayerNames, Write);
      WriteEntity(v);
    }
    public void WriteInlineResources(EntityResource er, int layerIndex)
    {
      Write(er.Version);
      Write(er.InlineResourceLayers[layerIndex].Length);
      for(int i = 0; i < er.InlineResourceLayers[layerIndex].Length; i++)
      {
        var resource = er.InlineResourceLayers[layerIndex][i];
        Write(resource.Type);
        Write(resource.Path);
        WriteEntityResource(er.InlineResourceLayers[layerIndex][i] as EntityResource);
      }
    }
    public void WriteEntity(EntityResource rsrc)
    {
      if(rsrc.Version > 0xE) { Write(0); }
      Write(rsrc.Entity.Version);
      Write(rsrc.Entity.Layers.Length);
      for(int i = 0; i < rsrc.Entity.Layers.Length; i++)
      {
        WriteEntityLayer(rsrc.Entity.Layers[i]);
        WriteInlineResources(rsrc, i);
      }
    }
    public void WriteEntityLayer(EntityLayer e)
    {
      Write(e.Version);
      Write(e.fileSlotIndex);
      Write(e.TotalObjectLayers);
      Write(e.Objects, WriteGameObject);
    }
    public void WriteGameObject(GameObject go)
    {
      if(go == null)
      {
        Write(-1);
        return;
      }
      var id = go.Id.Index | (go.Id.Layer << 16);
      Write(id);
      Write(go.Rev);
      Write(go.Name);
      Write(go.Components, WriteComponent);
    }
    public void WriteComponent(Component c)
    {
      Write(c.Name1);
      Write(c.Name2);
      Write(c.Rev);
      Write(c.Unknown2);
      Write(c.Props, WritePropDef);
      Array.ForEach(c.Props, p => WriteValue(p.Value));
    }
    public void WritePropDef(PropertyDef p)
    {
      Write(p.Name);
      WriteType(p.Type);
    }
    public void WriteType(Engine.Type t)
    {
      Write((int)t.InternalType);
      if(t is ArrayType)
      {
        var at = t as ArrayType;
        WriteType(at.ElementType);
      }
      else if(t is StructType)
      {
        var st = t as StructType;
        Write(st.Refcount);
        Write(st.Properties, WritePropDef);
      }
    }
    public void WriteValue(Value v)
    {
      switch (v)
      {
        case FloatValue x: Write(x.Data); break;
        case IntValue x: Write(x.Data); break;
        case ByteValue x: Write(x.Data); break;
        case UIntValue x: Write(x.Data); break;
        case LongValue x: Write(x.Data); break;
        case BoolValue x: Write(x.Data); break;
        case SymbolValue x: Write(x.Data); break;
        case ResourcePathValue x:
          Write(x.Prefix);
          Write(x.Data);
          break;
        case DrivenProp x:
          Write(x.Unknown1);
          Write(x.Unknown2);
          Write(x.ClassName);
          Write(x.Unknown3);
          Write(x.Unknown4);
          Write(x.PropertyName);
          break;
        case ArrayValue x: Write(x.Data, WriteValue); break;
        case StructValue x: Array.ForEach(x.Props, p => WriteValue(p.Value)); break;
        default:
          throw new Exception("This is an exhaustive switch");
      }
    }
  }
}
