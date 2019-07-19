using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using DtxCS.DataTypes;

namespace LibForge.RBSong
{
  public class Component
  {
    public string Name1;
    public string Name2;
    public int Rev;
    public long Unknown2;
    public Property[] Props;
  }
  public struct GameObjectId
  {
    public short Layer;
    public int Index;
  }
  public class GameObject
  {
    public const int MaxVersion = 0x4;
    public GameObjectId Id;
    public int Rev;
    public string Name;
    public Component[] Components;
  }
  public class EntityLayer
  {
    public int Version;
    public int fileSlotIndex;
    public short TotalObjectLayers;
    public GameObject[] Objects;
  }
  public class Entity
  {
    public const int MaxVersion = 0x1E;
    public int Version;
    public EntityLayer[] Layers;
  }
  public abstract class Resource
  {
    public string Type;
    public string Path;
    public static Resource Create(string type)
    {
      switch (type)
      {
        case "PropAnimResource":
          return new PropAnimResource();
        case "RBSongResource":
          return new RBSongResource();
        default:
          throw new NotImplementedException("Unimplemented resource type: "+type);
      }
    }
  }
  public class EntityResource : Resource
  {
    public EntityResource() { Type = "EntityResource"; }
    public const int MaxVersion = 0x11;
    public int Version;
    public string[] InlineLayerNames;
    public Entity Entity;
    public Resource[][] InlineResourceLayers;
  }
  public class RBSongResource : EntityResource
  {
    public RBSongResource() { Type = "RBSongResource"; }
  }
  public class PropAnimResource : EntityResource
  {
    public PropAnimResource() { Type = "PropAnimResource"; }
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
        case "uint":
          return PrimitiveType.UInt;
        case "long":
          return PrimitiveType.Long;
        case "bool":
          return PrimitiveType.Bool;
        case "symbol":
          return PrimitiveType.Symbol;
        case "string":
          return PrimitiveType.ResourcePath;
        case "array":
          var eType = FromData(n.Array("item"));
          return new ArrayType {
            ElementType = eType,
            InternalType = DataType.Array | eType.InternalType
          };
        case "driven_prop":
          return PrimitiveType.DrivenProp;
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
    public static Type UInt = new PrimitiveType(DataType.Uint32);
    public static Type Long = new PrimitiveType(DataType.Uint64);
    public static Type Bool = new PrimitiveType(DataType.Bool);
    public static Type Symbol = new PrimitiveType(DataType.Symbol);
    public static Type ResourcePath = new PrimitiveType(DataType.ResourcePath);
    public static Type DrivenProp = new PrimitiveType(DataType.DrivenProp);
    public static Type GameObjectId = new PrimitiveType(DataType.GameObjectId);
    public static Type Color = new PrimitiveType(DataType.Color);
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
        TypeDefinitions[array.Any(1)] = ret;
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
    DrivenProp = 0x10,
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
          return new UIntValue((uint)atom.Int);
        case DataType.Uint64:
          return new LongValue(atom.Int);
        case DataType.Bool:
          return new BoolValue(atom.Int != 0);
        case DataType.Symbol:
          return new SymbolValue(d.ToString());
        case DataType.ResourcePath:
          return new ResourcePathValue(d.ToString());
        case DataType.Struct:
          return StructValue.FromData(t as StructType, arr);
        case DataType.DrivenProp:
          return DrivenProp.FromData(arr);
        default:
          if(t is ArrayType at)
            return ArrayValue.FromData(at, arr);
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
  public class UIntValue : Value
  {
    public UIntValue(uint data) { Data = data; }
    public override Type Type { get; } = PrimitiveType.UInt;
    public uint Data;
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
  public class ResourcePathValue : Value
  {
    public ResourcePathValue(string data, byte prefix = 0) { Data = data; Prefix = prefix; }
    public override Type Type { get; } = PrimitiveType.ResourcePath;
    public string Data;
    public byte Prefix;
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
      if (t.Properties.Length > 0 && arr.Array(t.Properties[0].Name) != null)
      {
        // Props should be defined by name
        foreach (var prop in t.Properties)
          propVals.Add(new Property(prop.Name, FromData(prop.Type, arr.Array(prop.Name).Node(1))));
      }
      else
      {
        // Anonymous struct (e.g. in an array)
        for (int i = 0; i < t.Properties.Length; i++)
          propVals.Add(new Property(
            t.Properties[i].Name,
            FromData(t.Properties[i].Type, arr.Node(i))));
      }
      return new StructValue(t, propVals.ToArray());
    }
  }
  public class GameObjectIdValue : Value
  {
    public override Type Type => PrimitiveType.GameObjectId;
    public int Unknown1;
    public int Unknown2;
    public int Unknown3;
    public int Unknown4;
    public int Unknown5;
    public int Unknown6;
  }
  public class ColorValue : Value
  {
    public override Type Type => PrimitiveType.Color;
    public float R;
    public float G;
    public float B;
    public float A;
    public int Unk1;
    public int Unk2;
    public int Unk3;
    public int Unk4;
  }
  public class DrivenProp : Value
  {
    public override Type Type { get; } = PrimitiveType.DrivenProp;
    public int Unknown1;
    public int Unknown2;
    public string ClassName;
    public int Unknown3;
    public long Unknown4;
    public string PropertyName;
    public static DrivenProp FromData(DataArray arr)
    {
      return new DrivenProp
      {
        Unknown1 = arr.Int(0),
        Unknown2 = arr.Int(1),
        ClassName = arr.Any(2),
        Unknown3 = arr.Int(3),
        Unknown4 = arr.Int(4),
        PropertyName = arr.Any(5)
      };
    }
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
    public static ArrayValue FromData(ArrayType t, DataArray arr)
    {
      var data = new List<Value>();
      foreach(var node in arr.Children)
      {
        data.Add(FromData(t.ElementType, node));
      }
      return new ArrayValue(t, data.ToArray());
    }
  }
}
