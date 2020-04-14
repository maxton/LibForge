using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LibForge.Extensions
{
  static class TupleExtensions
  {
    public static void Deconstruct<T1,T2>(this Tuple<T1, T2> t, out T1 t1, out T2 t2)
    {
      t1 = t.Item1;
      t2 = t.Item2;
    }
  }
}
