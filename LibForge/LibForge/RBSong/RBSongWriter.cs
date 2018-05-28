using LibForge.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace LibForge.RBSong
{
  public class RBSongWriter : WriterBase<RBSong>
  {
    public RBSongWriter(Stream s) : base(s) { }
    public override void WriteStream(RBSong v)
    {
      Write(0xE);
      Write(0);
    }
  }
}
