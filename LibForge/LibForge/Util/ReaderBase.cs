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
  public abstract class ReaderBase<D> : BinReader
  {
    public ReaderBase(System.IO.Stream s) : base(s) { }
    public virtual D Read() => (D)Read(typeof(D));
  }
}
