using System;
using System.Text;
using LibForge.Extensions;

namespace LibForge.Util
{
  public abstract class WriterBase<D> : BinWriter
  {
    public WriterBase(System.IO.Stream s) : base(s) { }
    public abstract void WriteStream(D v);
  }
}
