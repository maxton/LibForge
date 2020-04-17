using LibForge.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace LibForge.Ark
{
  public class FileEntry
  {
    public long Offset;
    public string Path;
    public int Flags;
    public uint Size;

    public void Load(Stream s)
    {
      var r = new BinReader(s);
      Offset = r.Long();
      Path = r.String();
      Flags = r.Int();
      Size = r.UInt();
    }

    public void Save(Stream s)
    {
      var w = new BinWriter(s);
      w.Write(Offset);
      w.Write(Path);
      w.Write(Flags);
      w.Write(Size);
    }
  }
}
