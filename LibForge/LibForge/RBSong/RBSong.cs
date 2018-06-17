using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DtxCS.DataTypes;

namespace LibForge.RBSong
{
  public class RBSong
  {
    public int Version;
    public ObjectContainer Object1;
    public KeyValue KV;
    public ObjectContainer Object2;
  }

  // A TYPE is either:
  // A primitive data type
  // A struct containing fields, each with an associated TYPE
  // An array of a TYPE
  // TODO: There must be a better way to do this.
  public abstract class Type
  {
    public DataType InternalType;
    internal static Dictionary<string, Type> TypeDefinitions = new Dictionary<string, Type>();
    public static Type FromData(DataArray n)
    {
      var type = n.Any(1);
      switch (type)
      {
        case "struct":
          return StructType.FromData(n);
        case "float":
          return PrimitiveType.Float;
        case "int":
          return PrimitiveType.Int;
        case "byte":
          return PrimitiveType.Byte;
        case "flag":
          return PrimitiveType.Flag;
        case "long":
          return PrimitiveType.Long;
        case "bool":
          return PrimitiveType.Bool;
        case "symbol":
          return PrimitiveType.Symbol;
        case "string":
          return PrimitiveType.String;
        case "array":
          return new ArrayType { ElementType = FromData(n.Array("item")) };
        default:
          if (TypeDefinitions.ContainsKey(type))
            return TypeDefinitions[type];
          throw new Exception("Unknown type " + n.Any(1));
      }
    }
  }
  public class PrimitiveType : Type
  {
    public static Type Float = new PrimitiveType(DataType.Float);
    public static Type Int = new PrimitiveType(DataType.Int32);
    public static Type Byte = new PrimitiveType(DataType.Uint8);
    public static Type Flag = new PrimitiveType(DataType.Uint32);
    public static Type Long = new PrimitiveType(DataType.Uint64);
    public static Type Bool = new PrimitiveType(DataType.Bool);
    public static Type Symbol = new PrimitiveType(DataType.Symbol);
    public static Type String = new PrimitiveType(DataType.ResourcePath);
    public static Type PropRef = new PrimitiveType(DataType.PropRef);
    internal PrimitiveType(DataType internalType) { InternalType = internalType; }
  }
  public class StructType : Type
  {
    public long Refcount;
    public PropertyDef[] Properties;
    public static new StructType FromData(DataArray array)
    {
      var props = array.Array("props");
      var propList = new List<PropertyDef>();
      foreach(var p in props.Children)
      {
        var arr = p as DataArray;
        if (arr == null) continue;
        propList.Add(new PropertyDef { Name = arr.Any(0), Type = Type.FromData(arr) });
      }
      var ret = new StructType { InternalType = DataType.Struct, Properties = propList.ToArray() };
      if(array.Any(0) == "define")
      {
        TypeDefinitions.Add(array.Any(1), ret);
      }
      return ret;
    }
  }
  public class ArrayType : Type
  {
    public Type ElementType;
  }
  [Flags]
  public enum DataType : int
  {
    Float = 0,
    Int8 = 1,
    Int16 = 2,
    Int32 = 3,
    Int64 = 4,
    Uint8 = 5,
    Uint16 = 6,
    Uint32 = 7,
    Uint64 = 8,
    Bool = 9,
    GameObjectId = 0xA,
    Symbol = 0xB,
    ResourcePath = 0xC,
    Color = 0xD,
    // Array = 0xE,
    Struct = 0xF,
    PropRef = 0x10,
    Action = 0x11,
    WaveformFloat = 0x12,
    WaveformColor = 0x13,
    Array = 0x100
  }
  public class PropertyDef
  {
    public string Name;
    public Type Type;
  }
  public class Property : PropertyDef
  {
    public Property() { }
    public Property(string name, Value val)
    {
      Name = name;
      Value = val;
      Type = val.Type;
    }
    public Value Value;
  }
  public abstract class Value
  {
    public abstract Type Type { get; }
    public static Value FromData(Type t, DataNode d)
    {
      var atom = d as DataAtom;
      var arr = d as DataArray;
      switch (t.InternalType)
      {
        case DataType.Float:
          return new FloatValue(atom.Float);
        case DataType.Int32:
          return new IntValue(atom.Int);
        case DataType.Uint8:
          return new ByteValue((byte)atom.Int);
        case DataType.Uint32:
          return new FlagValue(atom.Int);
        case DataType.Uint64:
          return new LongValue(atom.Int);
        case DataType.Bool:
          return new BoolValue(atom.Int != 0);
        case DataType.Symbol:
          return new SymbolValue(d.ToString());
        case DataType.ResourcePath:
          return new StringValue(d.ToString());
        case DataType.Struct:
          return StructValue.FromData(t as StructType, arr);
        case DataType.PropRef:
        case DataType.Array:
        default:
          throw new Exception("Unhandled case (TODO)");
      }
    }
  }
  public class FloatValue : Value
  {
    public FloatValue(float data) { Data = data; }
    public override Type Type { get; } = PrimitiveType.Float;
    public float Data;
  }
  public class IntValue : Value
  {
    public IntValue(int data) { Data = data; }
    public override Type Type { get; } = PrimitiveType.Int;
    public int Data;
  }
  public class ByteValue : Value
  {
    public ByteValue(byte data) { Data = data; }
    public override Type Type { get; } = PrimitiveType.Byte;
    public byte Data;
  }
  public class FlagValue : Value
  {
    public FlagValue(int data) { Data = data; }
    public override Type Type { get; } = PrimitiveType.Flag;
    public int Data;
  }
  public class LongValue : Value
  {
    public LongValue(long data) { Data = data; }
    public override Type Type { get; } = PrimitiveType.Long;
    public long Data;
  }
  public class BoolValue : Value
  {
    public BoolValue(bool data) { Data = data; }
    public override Type Type { get; } = PrimitiveType.Bool;
    public bool Data;
  }
  public class SymbolValue : Value
  {
    public SymbolValue(string data) { Data = data; }
    public override Type Type { get; } = PrimitiveType.Symbol;
    public string Data;
  }
  public class StringValue : Value
  {
    public StringValue(string data) { Data = data; }
    public override Type Type { get; } = PrimitiveType.String;
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
    public static StructValue FromData(StructType t, DataArray arr)
    {
      var propVals = new List<Property>();
      foreach(var prop in t.Properties)
      {
        propVals.Add(new Property(prop.Name, FromData(prop.Type, arr.Array(prop.Name).Node(1))));
      }
      return new StructValue(t, propVals.ToArray());
    }
  }
  public class PropRef : Value
  {
    public override Type Type { get; } = PrimitiveType.PropRef;
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
    public int Unknown1;
    public long Unknown2;
    public Property[] Props;
  }
  public class Entity
  {
    public ushort Index0;
    public ushort Index1;
    // public uint Unknown2; // always 2
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
}
