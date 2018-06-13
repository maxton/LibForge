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
      var version = Check(Int(), 0xE);
      Check(Int(), 1);
      Check(Int(), 0);
      ret.Object1 = ReadObjectContainer();
      Check(Int(), 0xE);
      Check(Int(), 1);
      ret.KV = ReadKeyValue();
      Check(Int(), 0xE);
      Check(Int(), 1);
      Check(Int(), 0);
      ret.Object2 = ReadObjectContainer();
      Check(Int(), 0xE);
      Check(Int(), 0);
      return ret;
    }

    private RBSong.KeyValue ReadKeyValue()
      => new RBSong.KeyValue
      {
        Str1 = String(),
        Str2 = String()
      };
    private RBSong.ObjectContainer ReadObjectContainer()
      => new RBSong.ObjectContainer
      {
        Unknown1 = Int(),
        Unknown2 = Int(),
        Unknown3 = Int(),
        Unknown4 = Int(),
        Unknown5 = Short(),
        Entities = Arr(ReadEntity)
      };
    private RBSong.Entity ReadEntity()
      => new RBSong.Entity
      {
        Index0 = UShort(),
        Index1 = UShort().Then(() => Check(UInt(), 2u)),
        Name = String(),
        Coms = Arr(ReadComponent)
      };
    private RBSong.Component ReadComponent()
    {
      var entity = new RBSong.Component
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
    private RBSong.Type ReadType()
    {
      var type = (RBSong.DataType)Int();
      if (type.HasFlag(RBSong.DataType.Array))
      {
        return new RBSong.ArrayType
        {
          InternalType = type,
          ElementType = ReadType()
        };
      }
      else if(type.HasFlag(RBSong.DataType.Struct))
      {
        return new RBSong.StructType
        {
          InternalType = type,
          Refcount = Long(),
          Properties = Arr(ReadPropDef)
        };
      }
      switch (type)
      {
        case RBSong.DataType.Float:
          return RBSong.PrimitiveType.Float;
        case RBSong.DataType.Int:
          return RBSong.PrimitiveType.Int;
        case RBSong.DataType.Byte:
          return RBSong.PrimitiveType.Byte;
        case RBSong.DataType.Flag:
          return RBSong.PrimitiveType.Flag;
        case RBSong.DataType.Long:
          return RBSong.PrimitiveType.Long;
        case RBSong.DataType.Bool:
          return RBSong.PrimitiveType.Bool;
        case RBSong.DataType.Symbol:
          return RBSong.PrimitiveType.Symbol;
        case RBSong.DataType.String:
          return RBSong.PrimitiveType.String;
        case RBSong.DataType.DrivenValue:
          return RBSong.PrimitiveType.DrivenValue;
        default:
          return new RBSong.PrimitiveType(type);
      }
    }
    private RBSong.Property ReadPropDef()
      => new RBSong.Property
      {
        Name = String(),
        Type = ReadType()
      };
    private RBSong.Value ReadValue(RBSong.Type t)
    {
      switch (t)
      {
        case RBSong.ArrayType at:
          return new RBSong.ArrayValue(at, Arr(() => ReadValue(at.ElementType)));
        case RBSong.StructType st:
          var props = new List<RBSong.Property>();
          foreach(var p in st.Properties)
          {
            props.Add(new RBSong.Property
            {
              Name = p.Name,
              Type = p.Type,
              Value = ReadValue(p.Type)
            });
          }
          return new RBSong.StructValue(st, props.ToArray());
        case RBSong.PrimitiveType pt:
          switch (pt.InternalType)
          {
            case RBSong.DataType.Float:
              return new RBSong.FloatValue(Float());
            case RBSong.DataType.Int:
              return new RBSong.IntValue(Int());
            case RBSong.DataType.Byte:
              return new RBSong.ByteValue(Byte());
            case RBSong.DataType.Flag:
              return new RBSong.FlagValue(Int());
            case RBSong.DataType.Long:
              return new RBSong.LongValue(Long());
            case RBSong.DataType.Bool:
              return new RBSong.BoolValue(Byte() != 0);
            case RBSong.DataType.Symbol:
              var sym = String();
              if (sym.Length == 0 && Byte() != 0) s.Position -= 1;
              return new RBSong.SymbolValue(sym);
            case RBSong.DataType.String:
              var str = String();
              if (Byte() != 0) s.Position -= 1;
              return new RBSong.StringValue(str);
            case RBSong.DataType.DrivenValue:
              return new RBSong.DrivenProp
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
