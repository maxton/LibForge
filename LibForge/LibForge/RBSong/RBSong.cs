using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DtxCS;

namespace LibForge.RBSong
{
  public class RBSong
  {
    // A TYPE is either:
    // A primitive data type
    // A struct containing fields, each with an associated TYPE
    // An array of a TYPE
    public abstract class Type
    {
      public DataType InternalType;
    }
    public class PrimitiveType : Type
    {
    }
    public class StructType : Type
    {
      public long Refcount;
      public PropertyDef[] Properties;
    }
    public class ArrayType : Type
    {
      public Type ElementType;
    }
    [Flags]
    public enum DataType : int
    {
      Float = 0,
      Int = 3,
      Byte = 5,
      Flag = 7,
      Enum = 8,
      Bool = 9,
      Symbol = 0xB,
      String = 0xC,
      Struct = 0xF,
      DrivenValue = 0x10,
      Array = 0x100
    }
    public class PropertyDef
    {
      public string Name;
      public Type Type;
    }
    public class Property : PropertyDef
    {
      public Value Value;
    }
    public abstract class Value
    {
      public abstract Type Type { get; }
    }
    public class FloatValue : Value
    {
      public override Type Type { get; }
        = new PrimitiveType { InternalType = DataType.Float };
      public float Data;
    }
    public class IntValue : Value
    {
      public override Type Type { get; } = new PrimitiveType { InternalType = DataType.Int };
      public int Data;
    }
    public class ByteValue : Value
    {
      public override Type Type { get; } = new PrimitiveType { InternalType = DataType.Byte };
      public byte Data;
    }
    public class FlagValue : Value
    {
      public override Type Type { get; } = new PrimitiveType { InternalType = DataType.Flag };
      public int Data;
    }
    public class EnumValue : Value
    {
      public override Type Type { get; } = new PrimitiveType { InternalType = DataType.Enum };
      public long Data;
    }
    public class BoolValue : Value
    {
      public override Type Type { get; } = new PrimitiveType { InternalType = DataType.Bool };
      public bool Data;
    }
    public class SymbolValue : Value
    {
      public override Type Type { get; } = new PrimitiveType { InternalType = DataType.Symbol };
      public string Data;
    }
    public class StringValue : Value
    {
      public override Type Type { get; } = new PrimitiveType { InternalType = DataType.String };
      public string Data;
    }
    public class StructValue : Value
    {
      public StructValue(Type t, Property[] props)
      {
        Type = t;
        Props = props;
      }
      public override Type Type { get; }
      public Property[] Props;
    }
    public class DrivenProp : Value
    {
      public override Type Type { get; } = new PrimitiveType { InternalType = DataType.DrivenValue };
      public long Unknown1;
      public string ClassName;
      public int Unknown2;
      public long Unknown3;
      public string PropertyName;
    }
    public class ArrayValue : Value
    {
      public ArrayValue(ArrayType t, Value[] values)
      {
        Type = t;
        Data = values;
      }
      public override Type Type { get; }
      public Value[] Data;
    }
    public class Component
    {
      public string ClassName;
      public string Name;
      public int Type;
      public long Count;
      public Property[] Props;
    }
    public class Entity
    {
      public ushort Index0;
      public ushort Index1;
      public uint Unknown2;
      public string Name;
      public Component[] Coms;
    }
    public class ObjectContainer
    {
      public int Unknown1;
      public int Unknown2;
      public int Unknown3;
      public int Unknown4;
      public short Unknown5;
      public Entity[] Entities;
    }
    public class KeyValue
    {
      public string Str1;
      public string Str2;
    }
    public ObjectContainer Object1;
    public KeyValue KV;
    public ObjectContainer Object2;
  }
}
