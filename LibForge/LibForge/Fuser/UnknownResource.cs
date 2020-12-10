using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace LibForge.Fuser
{
  public class UnknownResource : ResourceFile
  {
    public byte[] data;
    public override void Load(Stream s)
    {
      data = new byte[Size];
      s.Read(data, 0, data.Length);
    }
  }
}
