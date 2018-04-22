using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LibForge.Lipsync
{
  public class LipsyncWriter : Util.WriterBase<Lipsync>
  {
    public LipsyncWriter(System.IO.Stream s) : base(s) { }
    public override void WriteStream(Lipsync v)
    {
      Write(v.Version);
      Write(v.Subtype);
      Write(v.FrameRate);
      Write(v.Visemes, Write);
      Write(v.Players, Write);
      Write(v.FrameIndices, Write);
      s.Write(v.FrameData, 0, v.FrameData.Length);
    }
  }
}
