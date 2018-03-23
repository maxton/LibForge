using System;
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
  }
  public abstract class ReaderBase
  {
    protected System.IO.Stream s;
    protected ReaderBase(System.IO.Stream s)
    {
      this.s = s;
    }
    protected static Func<T> Seq<T>(Action a, Func<T> v)
    {
      return () =>
      {
        a();
        return v();
      };
    }

    // For reading a fixed size array of something
    protected T[] Arr<T>(Func<T> constructor, int size)
    {
      var arr = new T[size];
      for (var i = 0; i < size; i++)
        arr[i] = constructor();

      return arr;
    }
    // For reading a length-prefixed array of something
    protected T[] Arr<T>(Func<T> constructor) => Arr(constructor, Int());
    // For skipping unknown data
    protected Action Skip(int count) => () => s.Position += count;
    // For reading simple types
    protected int Int() => s.ReadInt32LE();
    protected uint UInt() => s.ReadUInt32LE();
    protected float Float() => s.ReadFloat();
    protected short Short() => s.ReadInt16LE();
    protected ushort UShort() => s.ReadUInt16LE();
    protected byte Byte() => (byte)s.ReadByte();
    protected string String() => s.ReadLengthPrefixedString(Encoding.ASCII);
    protected uint UInt24() => s.ReadUInt24LE();
  }
}
