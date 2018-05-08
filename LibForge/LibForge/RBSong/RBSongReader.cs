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
      var structs = new List<RBSong.IUnknown>();
      var version = Int();
      var more = Int() == 1;
      while(more)
      {
        structs.Add(ReadTree());
        version = Int();
        if (version != 0xE)
        {
          throw new InvalidDataException($"Unknown RBsong version {version:X}");
        }
        more = Int() == 1;
      }
      
      return new RBSong { Structs = structs.ToArray() };
    }

    private RBSong.IUnknown ReadTree()
    {
      if(Int() != 0)
      {
        s.Position -= 4;
        return new RBSong.KeyValue
        {
          Str1 = String(),
          Str2 = String()
        };
      }
      else
      {
        return new RBSong.ObjectContainer
        {
          Unknown1 = Int(),
          Unknown2 = Int(),
          Unknown3 = Int(),
          Unknown4 = Int(),
          Unknown5 = Short(),
          Objects = Arr(ReadObject)
        };
      }
    }

    private RBSong.ForgeObject ReadObject()
      => new RBSong.ForgeObject
      {
        Index0 = UShort(),
        Index1 = UShort(),
        Unknown2 = UInt(),
        Name = String(),
        Coms = Arr(ReadEntity)
      };
    
    private RBSong.Component ReadEntity()
    {
      var entity = new RBSong.Component
      {
        ClassName = String(),
        InstanceName = String(),
        Type = Int(),
        Count = Long(),
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
      return new RBSong.PrimitiveType
      {
        InternalType = type
      };
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
              return new RBSong.FloatValue { Data = Float() };
            case RBSong.DataType.Int:
              return new RBSong.IntValue { Data = Int() };
            case RBSong.DataType.Byte:
              return new RBSong.ByteValue { Data = Byte() };
            case RBSong.DataType.Flag:
              return new RBSong.FlagValue { Data = Int() };
            case RBSong.DataType.Enum:
              return new RBSong.EnumValue { Data = Long() };
            case RBSong.DataType.Bool:
              return new RBSong.BoolValue { Data = Byte() != 0 };
            case RBSong.DataType.Symbol:
              var sym = String();
              if (sym.Length == 0 && Byte() != 0) s.Position -= 1;
              return new RBSong.SymbolValue { Data = sym };
            case RBSong.DataType.String:
              var str = String();
              if (Byte() != 0) s.Position -= 1;
              return new RBSong.StringValue { Data = str };
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
