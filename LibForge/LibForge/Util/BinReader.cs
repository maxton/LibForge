using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using LibForge.Extensions;

namespace LibForge.Util
{
  public class BinReader
  {
    protected Stream s;
    public BinReader(Stream stream)
    {
      s = stream;
    }
    public T Object<T>()
    {
      T ret = default;
      foreach (var field in typeof(T).GetFields())
      {
        var type = field.FieldType;
        field.SetValue(ret, Read(field.FieldType));
      }
      return default;
    }
    public object Read(Type t)
    {
      var readers =
      new Dictionary<Type, Func<object>> {
        { typeof(int), () => Int() },
        { typeof(uint), () => UInt() },
        { typeof(long), () => Long() },
        { typeof(ulong), () => ULong() },
        { typeof(float), () => Float() },
        { typeof(short), () => Short() },
        { typeof(ushort), () => UShort() },
        { typeof(byte), () => Byte() },
        { typeof(string), () => String() },
        { typeof(bool), () => Bool() },
      };
      if (readers.ContainsKey(t))
        return readers[t]();
      var method = GetType().GetMethod(nameof(Object)).MakeGenericMethod(t);
      return method.Invoke(this, new object[] { });
    }
    public static Func<T> Seq<T>(Action a, Func<T> v)
    {
      return () =>
      {
        a();
        return v();
      };
    }
    public T Check<T>(T v, T expected, string where = null)
    {
      if (v.Equals(expected))
        return v;
      throw new Exception($"Invalid data encountered at {s.Position:X} {where}: expected {expected}, got {v}");
    }

    public byte CheckRange(byte v, byte minimum, byte maximum)
    {
      if (minimum <= v && maximum >= v)
        return v;
      throw new Exception($"Range of {minimum} -> {maximum} exceeded at {s.Position:X}: got {v} ");
    }
    public int CheckRange(int v, int minimum, int maximum)
    {
      if (minimum <= v && maximum >= v)
        return v;
      throw new Exception($"Range of {minimum} -> {maximum} exceeded at {s.Position:X}: got {v} ");
    }
    // For reading a fixed size array of something
    public T[] FixedArr<T>(Func<T> constructor, uint size)
    {
      if (size > s.Length - s.Position)
      {
        throw new Exception($"Invalid array size {size:X} encountered at {s.Position:X}. File is corrupt or not understood.");
      }
      var arr = new T[size];
      for (var i = 0; i < size; i++)
        arr[i] = constructor();

      return arr;
    }
    // For reading a length-prefixed array of something
    public T[] Arr<T>(Func<T> constructor, uint maxSize = 0)
    {
      var size = UInt();
      if (maxSize != 0 && size > maxSize)
        throw new Exception($"Array was too big ({size} > {maxSize}) at {s.Position:X}");
      return FixedArr(constructor, size);
    }
    public T[] CheckedArr<T>(Func<T> constructor, uint size)
    {
      var fileSize = UInt();
      if (fileSize != size)
        throw new Exception($"Invalid array size ({fileSize} != {size}) at {s.Position:X}");
      return FixedArr(constructor, size);
    }
    // For skipping unknown data
    public Action Skip(int count) => () => s.Position += count;
    // For reading simple types
    public int Int() => s.ReadInt32LE();
    public uint UInt() => s.ReadUInt32LE();
    public long Long() => s.ReadInt64LE();
    public ulong ULong() => s.ReadUInt64LE();
    public float Half() => s.ReadHalfFloat();
    public float Float() => s.ReadFloat();
    public short Short() => s.ReadInt16LE();
    public ushort UShort() => s.ReadUInt16LE();
    public byte Byte() => (byte)s.ReadByte();
    public string String() => s.ReadLengthPrefixedString(Encoding.UTF8);
    public string String(int length) => s.ReadFixedLengthNullTerminatedString(length);
    public string FixedString(int length) => s.ReadFixedLengthString(length);
    public string UE4String() => String(Int());
    public uint UInt24() => s.ReadUInt24LE();
    /// <summary>
    /// Reads a byte as a boolean, throwing if it's not 1 or 0
    /// </summary>
    public bool Bool() => CheckRange(Byte(), (byte)0, (byte)1) != 0;
  }
}
