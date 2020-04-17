using LibForge.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace LibForge.Util
{
  public class EncryptedWriteStream : Stream
  {
    private long position;
    private int key;
    private int curKey;
    private long keypos;
    private Stream file;
    public byte xor;


    internal EncryptedWriteStream(Stream file, int key, byte xor = 0)
    {
      file.Position = 0;
      file.WriteInt32LE(key);
      position = 0;
      keypos = 0;
      // The initial key is found in the first 4 bytes.
      this.key = cryptRound(key);
      this.curKey = this.key;
      this.file = file;
      this.xor = xor;
    }

    public override bool CanRead => false;
    public override bool CanSeek => true;
    public override bool CanWrite => file.CanWrite;
    public override long Length => file.Length - 4;

    public override long Position
    {
      get
      {
        return position;
      }

      set
      {
        Seek(value, SeekOrigin.Begin);
      }
    }

    private void updateKey()
    {
      if (keypos == position)
        return;
      if (keypos > position) // reset key
      {
        keypos = 0;
        curKey = key;
      }
      while (keypos < position) // don't think there's a faster way to do this
      {
        curKey = cryptRound(curKey);
        keypos++;
      }
    }

    private int cryptRound(int key)
    {
      int ret = (key - ((key / 0x1F31D) * 0x1F31D)) * 0x41A7 - (key / 0x1F31D) * 0xB14;
      if (ret <= 0)
        ret += 0x7FFFFFFF;
      return ret;
    }

    public override int Read(byte[] buffer, int offset, int count)
    {
      throw new NotImplementedException();
    }

    public override long Seek(long offset, SeekOrigin origin)
    {
      int adjust = origin == SeekOrigin.Current ? 0 : 4;
      this.position = file.Seek(offset + adjust, origin) - 4;
      updateKey();
      return position;
    }

    #region Not Used

    public override void Flush()
    {
      throw new NotSupportedException();
    }

    public override void SetLength(long value)
    {
      throw new NotSupportedException();
    }

    public override void Write(byte[] buffer, int offset, int count)
    {
      if (offset + count > buffer.Length)
      {
        throw new IndexOutOfRangeException("Attempt to read buffer past its end");
      }
      updateKey();
      var copy = new byte[count];
      Buffer.BlockCopy(buffer, offset, copy, 0, count);
      for (uint i = 0; i < count; i++)
      {
        copy[i] ^= (byte)(this.curKey ^ xor);
        position++;
        updateKey();
      }
      // ensure file is at correct offset
      file.Seek(this.position + 4 - count, SeekOrigin.Begin);
      file.Write(copy, 0, count);
    }

    #endregion
  }
}
