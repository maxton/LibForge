using System;
using System.Collections.Generic;
using System.Text;
using LibForge.Extensions;

namespace LibForge.Util
{
  static class ReaderExtensions
  {
    // For doing side effects else fluently in an expression
    public static T Then<T>(this T v, Action a)
    {
      a();
      return v;
    }
    public static T Then<T>(this T v, Action<T> a)
    {
      a(v);
      return v;
    }
  }
  public abstract class ReaderBase<D>
  {
    protected System.IO.Stream s;
    public ReaderBase(System.IO.Stream s)
    {
      this.s = s;
    }
    public virtual D Read() => (D)Read(typeof(D));
    
    protected T Object<T>()
    {
      T ret = default;
      foreach(var field in typeof(T).GetFields())
      {
        var type = field.FieldType;
        field.SetValue(ret, Read(field.FieldType));
      }
      return default;
    }
    protected object Read(Type t)
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
    protected static Func<T> Seq<T>(Action a, Func<T> v)
    {
      return () =>
      {
        a();
        return v();
      };
    }
    protected T Check<T>(T v, T expected)
    {
      if (v.Equals(expected))
        return v;
      throw new Exception($"Invalid data encountered at {s.Position:X}: expected {expected}, got {v}");
    }

    protected byte CheckRange(byte v, byte minimum, byte maximum)
    {
      if (minimum <= v && maximum >= v)
        return v;
      throw new Exception($"Range of {minimum} -> {maximum} exceeded at {s.Position:X}: got {v} ");
    }
    // For reading a fixed size array of something
    protected T[] FixedArr<T>(Func<T> constructor, uint size)
    {
      if(size > s.Length - s.Position)
      {
        throw new Exception($"Invalid array size {size:X} encountered at {s.Position:X}. File is corrupt or not understood.");
      }
      var arr = new T[size];
      for (var i = 0; i < size; i++)
        arr[i] = constructor();

      return arr;
    }
    // For reading a length-prefixed array of something
    protected T[] Arr<T>(Func<T> constructor, uint maxSize = 0)
    {
      var size = UInt();
      if (maxSize != 0 && size > maxSize)
        throw new Exception($"Array was too big ({size} > {maxSize}) at {s.Position:X}");
      return FixedArr(constructor, size);
    }
    protected T[] CheckedArr<T>(Func<T> constructor, uint size)
    {
      var fileSize = UInt();
      if(fileSize != size)
        throw new Exception($"Invalid array size ({fileSize} != {size}) at {s.Position:X}");
      return FixedArr(constructor, size);
    }
    // For skipping unknown data
    protected Action Skip(int count) => () => s.Position += count;
    // For reading simple types
    protected int Int() => s.ReadInt32LE();
    protected uint UInt() => s.ReadUInt32LE();
    protected long Long() => s.ReadInt64LE();
    protected ulong ULong() => s.ReadUInt64LE();
    protected float Float() => s.ReadFloat();
    protected short Short() => s.ReadInt16LE();
    protected ushort UShort() => s.ReadUInt16LE();
    protected byte Byte() => (byte)s.ReadByte();
    protected string String() => s.ReadLengthPrefixedString(Encoding.ASCII);
    protected string String(int length) => s.ReadFixedLengthNullTerminatedString(length);
    protected uint UInt24() => s.ReadUInt24LE();
    protected bool Bool() => CheckRange(Byte(), 0, 1) != 0;
  }
}
