using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using LibForge.Util;

namespace LibForge.Fuser
{
  public class FusionPatchResource : ResourceFile
  {
    public string data;
    public override void Load(Stream s)
    {
      var b = new BinReader(s);
      data = b.String((int)Size);
    }
  }
}
