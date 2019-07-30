using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using LibForge.Extensions;
using LibForge.Milo;

namespace LibForge.Lipsync
{
  public class LipsyncConverter
  {
    private static string[] partMapping = new string[] { "mic", "guitar", "bass", "drums" };

    public static Lipsync FromMilo(MiloFile milo)
    {
      var miloLips = milo.Entries
        .Where(x => x is CharLipSync)
        .Select(y => y as CharLipSync)
        .ToList();

      miloLips.Sort((x, y) =>
      {
        if (x.Name.Equals("song.lipsync", StringComparison.CurrentCultureIgnoreCase))
          return -1;
        else if (y.Name.Equals("song.lipsync", StringComparison.CurrentCultureIgnoreCase))
          return 1;

        return string.Compare(x.Name, y.Name);
      });
      
      int totalFrames = miloLips.Select(x => x.Frames.Length).DefaultIfEmpty(0).Max();
      uint[] offsets = new uint[totalFrames + 1];

      long currentOffset = 0;
      var visemes = miloLips.SelectMany(x => x.Frames.SelectMany(y => y.Events))
        .Select(z => z.VisemeName)
        .Distinct()
        .ToDictionary(a => a, b => (int)(currentOffset++));
      
      using (MemoryStream ms = new MemoryStream())
      {
        for (int i = 0; i < totalFrames; i++)
        {
          currentOffset = ms.Position;
          var data = GetBytes(miloLips, visemes, i);
          ms.Write(data, 0, data.Length);

          offsets[i + 1] = (uint)ms.Position;
        }

        return new Lipsync
        {
          Version = 0,
          Subtype = 0,
          FrameRate = 30,
          Visemes = visemes.Select((x, y) => x.Key).ToArray(),
          Players = partMapping.Take(miloLips.Count).ToArray(),
          FrameIndices = offsets,
          FrameData = ms.ToArray()
        };

      }
    }

    private static byte[] GetBytes(List<CharLipSync> lips, Dictionary<string, int> visemes, int fo)
    {
      byte[] GetBytes(CharLipSync lip)
      {
        byte[] data = new byte[(lip.Frames.Length <= fo) ? 0 : lip.Frames[fo].Events.Count * 2];
        if (data.Length <= 0) return data;

        var events = lip.Frames[fo].Events;

        for (int i = 0; i < events.Count; i++)
        {
          data[i * 2] = (byte)visemes[events[i].VisemeName];
          data[(i * 2) + 1] = (byte)events[i].Weight;
        }

        return data;
      }

      // Calculates max interlaced frames
      int max = lips.Count;
      while (max > 0)
      {
        if (fo < lips[max - 1].Frames.Length
          && lips[max - 1]
            .Frames[fo]
            .Events.Count > 0)
          break;

        max--;
      }

      if (max == 0)
        return new byte[0];
      
      using (MemoryStream ms = new MemoryStream())
      {
        var buffer = GetBytes(lips.First());
        ms.Write(buffer, 0, buffer.Length);
        
        foreach(var lip in lips.Take(max).Skip(1))
        {
          ms.WriteByte(0xFF);
          
          buffer = GetBytes(lip);
          ms.Write(buffer, 0, buffer.Length);
        }

        return ms.ToArray();
      }
    }
  }
}
