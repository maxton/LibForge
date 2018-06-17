using LibForge.Util;
using System;
using System.Collections.Generic;
using System.IO;

namespace LibForge.RBSong
{
  public class RBSongReader : ReaderBase<RBSong>
  {
    public RBSongReader(Stream s) : base(s)
    {  }

    public override RBSong Read()
    {
      var ret = new RBSong();
      ret.Version = Int();
      Check(Int(), 1);
      Check(Int(), 0);
      ret.Object1 = ReadObjectContainer(ret.Version);
      Check(Int(), ret.Version);
      Check(Int(), 1);
      ret.KV = ReadKeyValue();
      Check(Int(), ret.Version);
      Check(Int(), 1);
      Check(Int(), 0);
      ret.Object2 = ReadObjectContainer(ret.Version);
      Check(Int(), ret.Version);
      Check(Int(), 0);
      return ret;
    }

    private KeyValue ReadKeyValue()
      => new KeyValue
      {
        Str1 = String(),
        Str2 = String()
      };
    private ObjectContainer ReadObjectContainer(int version)
    {
      var obj = new ObjectContainer();
      if (version > 0xE) Int();
      obj.Unknown1 = Int();
      obj.Unknown2 = Int();
      obj.Unknown3 = Int();
      if (version > 0xE) Int();
      obj.Unknown4 = Int();
      obj.Unknown5 = Short();
      obj.Entities = Arr(ReadEntity);
      return obj;
    }
    private Entity ReadEntity()
    {
      var ent = new Entity();
      ent.Index0 = UShort();
      ent.Index1 = UShort();
      if (ent.Index0 == 0xFFFF && ent.Index1 == 0xFFFF)
        return null;
      Check(UInt(), 2u);
      ent.Name = String();
      ent.Coms = Arr(ReadComponent);
      return ent;
    }
    private Component ReadComponent()
    {
      var entity = new Component
      {
        ClassName = String(),
        Name = String(),
        Unknown1 = Int(),
        Unknown2 = Long(),
        Props = Arr(ReadPropDef)
      };
      foreach(var prop in entity.Props)
      {
        prop.Value = ReadValue(prop.Type);
      }
      return entity;
    }
    private Type ReadType()
    {
      var type = (DataType)Int();
      if (type.HasFlag(DataType.Array))
      {
        return new ArrayType
        {
          InternalType = type,
          ElementType = ReadType()
        };
      }
      else if(type.HasFlag(DataType.Struct))
      {
        return new StructType
        {
          InternalType = type,
          Refcount = Long(),
          Properties = Arr(ReadPropDef)
        };
      }
      switch (type)
      {
        case DataType.Float:
          return PrimitiveType.Float;
        case DataType.Int32:
          return PrimitiveType.Int;
        case DataType.Uint8:
          return PrimitiveType.Byte;
        case DataType.Uint32:
          return PrimitiveType.Flag;
        case DataType.Uint64:
          return PrimitiveType.Long;
        case DataType.Bool:
          return PrimitiveType.Bool;
        case DataType.Symbol:
          return PrimitiveType.Symbol;
        case DataType.ResourcePath:
          return PrimitiveType.String;
        case DataType.PropRef:
          return PrimitiveType.PropRef;
        default:
          return new PrimitiveType(type);
      }
    }
    private Property ReadPropDef()
      => new Property
      {
        Name = String(),
        Type = ReadType()
      };
    private Value ReadValue(Type t)
    {
      switch (t)
      {
        case ArrayType at:
          return new ArrayValue(at, Arr(() => ReadValue(at.ElementType)));
        case StructType st:
          var props = new List<Property>();
          foreach(var p in st.Properties)
          {
            props.Add(new Property
            {
              Name = p.Name,
              Type = p.Type,
              Value = ReadValue(p.Type)
            });
          }
          return new StructValue(st, props.ToArray());
        case PrimitiveType pt:
          switch (pt.InternalType)
          {
            case DataType.Float:
              return new FloatValue(Float());
            case DataType.Int32:
              return new IntValue(Int());
            case DataType.Uint8:
              return new ByteValue(Byte());
            case DataType.Uint32:
              return new FlagValue(Int());
            case DataType.Uint64:
              return new LongValue(Long());
            case DataType.Bool:
              return new BoolValue(Byte() != 0);
            case DataType.Symbol:
              var sym = String();
              if (sym.Length == 0 && Byte() != 0) s.Position -= 1;
              return new SymbolValue(sym);
            case DataType.ResourcePath:
              var str = String();
              if (Byte() != 0) s.Position -= 1;
              return new StringValue(str);
            case DataType.PropRef:
              return new PropRef
              {
                Unknown1 = Long(),
                ClassName = String(),
                Unknown2 = Int(),
                Unknown3 = Long(),
                PropertyName = String()
              };
            default:
              throw new InvalidDataException("Unknown type");
          }
        default:
          throw new Exception("This switch is exhaustive but C# doesn't have sum types");
      }
    }
  }
}
