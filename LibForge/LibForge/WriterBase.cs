using System;
using System.Text;
using LibForge.Extensions;

namespace LibForge.Util
{
  public abstract class WriterBase<D>
  {
    protected System.IO.Stream s;
    public WriterBase(System.IO.Stream s)
    {
      this.s = s;
    }
    public abstract void WriteStream(D v);
    protected void Write(byte v) => s.WriteByte(v);
    protected void Write(short v) => s.WriteInt16LE(v);
    protected void Write(ushort v) => s.WriteUInt16LE(v);
    protected void Write(int v) => s.WriteInt32LE(v);
    protected void Write(uint v) => s.WriteUInt32LE(v);
    protected void Write(long v) => s.WriteInt64LE(v);
    protected void Write(ulong v) => s.WriteUInt64LE(v);
    protected void Write(float v) => s.Write(BitConverter.GetBytes(v), 0, 4);
    protected void Write(bool v) => s.WriteByte((byte)(v ? 1 : 0));
    protected void Write(string v)
    {
      var bytes = Encoding.UTF8.GetBytes(v);
      s.WriteInt32LE(bytes.Length);
      s.Write(bytes, 0, bytes.Length);
    }
    protected void Write(string v, int length)
    {
      var bytes = Encoding.UTF8.GetBytes(v);
      s.Write(bytes, 0, bytes.Length);
      s.WriteByte(0);
      s.Position += length - bytes.Length - 1;
    }
    protected void Write<T>(T[] arr, Action<T> writer)
    {
      // Treat uninitialized arrays as empty ones
      if(arr == null)
      {
        Write(0);
        return;
      }
      Write(arr.Length);
      foreach (var x in arr)
      {
        writer(x);
      }
    }
  }
}
