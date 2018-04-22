using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LibForge.Util;

namespace LibForge.Lipsync
{
  public class LipsyncReader : ReaderBase<Lipsync>
  {
    public LipsyncReader(System.IO.Stream s) : base(s) { }
    public override Lipsync Read()
    {
      var l = new Lipsync();
      l.Version = Check(Int(), 0);
      l.Subtype = Check(Int(), 0);
      l.FrameRate = Check(Float(), 30f);
      l.Visemes = Arr(String);
      l.Players = Arr(String);
      l.FrameIndices = Arr(UInt);
      l.FrameData = FixedArr(Byte, l.FrameIndices.LastOrDefault());
      return l;
    }
  }
}
