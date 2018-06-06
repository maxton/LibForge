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
      /*
      l.Groups = new FrameGroup[l.Players.Length];

      uint[] offsets = Arr(UInt); // Key frame offsets

      // Inits groups
      for (int i = 0; i < l.Groups.Length; i++)
      {
        l.Groups[i] = new FrameGroup();
        l.Groups[i].Frames = new KeyFrame[offsets.Length - 1];
        
      }

      
      for (int i = 0; i < offsets.Length - 1; i++)
      {
        int groupSize = (int)(offsets[i + 1] - offsets[i]);
        
        int j = 0;
        while (j < l.Groups.Length)
        {
          int keySize = GetKeySize(s, groupSize);
          if (keySize == 0) break;

          var events = l.Groups[i].Frames[j];

          groupSize -= keySize;
          j++;
        }
      }
      */
    }

    private static int GetKeySize(System.IO.Stream s, int max)
    {
      if (max <= 0) return 0;

      long start = s.Position;
      int i = 0;

      // Reads in size until terminating character
      while (i < max)
      {
        i++;

        if (s.ReadByte() == 0xFF)
          break;

        i++;
      }

      s.Seek(start, System.IO.SeekOrigin.Begin);
      return max;
    }
  }
}
