using LibForge.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace LibForge.Fuser
{
  public class MoggSampleResource : ResourceFile
  {
    public int SampleRate;
    public int NumberOfSamples;
    public byte[] MoggFileData;
    public override void Load(Stream s)
    {
      var b = new BinReader(s);
      b.Check(b.FixedString(4), "mogs");
      b.Check(b.Int(), 1);
      SampleRate = b.Int();
      b.Check(b.Int(), 2);
      NumberOfSamples = b.Int();
      b.Check(b.Int(), 3);
      b.Check(b.Int(), 2);
      MoggFileData = new byte[b.Int()];
      s.Read(MoggFileData, 0, MoggFileData.Length);
    }
  }
}
