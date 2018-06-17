using LibForge.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace LibForge.RBSong
{
  public class RBSongWriter : WriterBase<RBSong>
  {
    public RBSongWriter(Stream s) : base(s) { }
    public override void WriteStream(RBSong v)
    {
      Write(v.Version);
      Write(1);
      Write(0);
      WriteObjectContainer(v.Version, v.Object1);
      Write(v.Version);
      Write(1);
      WriteKeyValue(v.KV);
      Write(v.Version);
      Write(1);
      Write(0);
      WriteObjectContainer(v.Version, v.Object2);
      Write(v.Version);
      Write(0);
    }
    public void WriteObjectContainer(int version, ObjectContainer oc)
    {
      if(version > 0xE) { Write(0); }
      Write(oc.Unknown1);
      Write(oc.Unknown2);
      Write(oc.Unknown3);
      Write(oc.Unknown4);
      Write(oc.Unknown5);
      Write(oc.Entities, WriteEntity);
    }
    public void WriteEntity(Entity e)
    {
      Write(e.Index0);
      Write(e.Index1);
      Write(2);
      Write(e.Name);
      Write(e.Coms, WriteComponent);
    }
    public void WriteComponent(Component c)
    {
      Write(c.ClassName);
      Write(c.Name);
      Write(c.Unknown1);
      Write(c.Unknown2);
      Write(c.Props, WritePropDef);
      Array.ForEach(c.Props, p => WriteValue(p.Value));
    }
    public void WritePropDef(PropertyDef p)
    {
      Write(p.Name);
      WriteType(p.Type);
    }
    public void WriteType(Type t)
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
        case FlagValue x: Write(x.Data); break;
        case LongValue x: Write(x.Data); break;
        case BoolValue x: Write(x.Data); break;
        // TODO: Look into extra null-terminator?
        case SymbolValue x:
          Write(x.Data);
          if (x.Data.Length == 0) Write((byte)0);
          break;
        case StringValue x: Write(x.Data); break;
        case PropRef x:
          Write(x.Unknown1);
          Write(x.ClassName);
          Write(x.Unknown2);
          Write(x.Unknown3);
          Write(x.PropertyName);
          break;
        case ArrayValue x: Write(x.Data, WriteValue); break;
        case StructValue x: Array.ForEach(x.Props, p => WriteValue(p.Value)); break;
        default:
          throw new Exception("This is an exhaustive switch");
      }
    }
    public void WriteKeyValue(KeyValue kv)
    {
      Write(kv.Str1);
      Write(kv.Str2);
    }

  }
}
