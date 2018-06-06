using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using LibForge.Extensions;
using LibForge.Lipsync;

namespace LibForge.Milo
{
  public class CharLipSync : IMiloEntry
  {
    private string _name;

    public static CharLipSync FromStream(Stream stream, string name)
    {
      // Does not include DTA parsing so DC lipsync files are not supported at this time
      CharLipSync lipsync = new CharLipSync();
      lipsync._name = name;

      lipsync.Version = stream.ReadInt32BE();
      lipsync.SubVersion = stream.ReadInt32BE();
      lipsync.DTAImport = stream.ReadLengthUTF8(true);

      int dtb = stream.ReadByte();
      if (dtb != 0)
        throw new Exception("Parsing of LipSync files with embedded DTB is not currently supported");

      stream.Position += 4; // Skips zeros
      int visemeCount = stream.ReadInt32BE();
      lipsync.Visemes = Enumerable.Range(0, visemeCount).Select(x => stream.ReadLengthUTF8(true)).ToArray();

      int keyFrameCount = stream.ReadInt32BE();
      stream.Position += 4; // Skips total size of following data

      lipsync.Frames = new KeyFrame[keyFrameCount];
      for (int i = 0; i < keyFrameCount; i++)
      {
        int eventCount = stream.ReadByte();

        lipsync.Frames[i] = new KeyFrame();
        lipsync.Frames[i].Events = new List<VisemeEvent>();

        for (int j = 0; j < eventCount; j++)
        {
          lipsync.Frames[i].Events.Add(new VisemeEvent(lipsync.Visemes[stream.ReadByte()], (byte)stream.ReadByte()));
        }
      }

      // There's some other data ofter this sometimes but it's not needed

      return lipsync;
    }

    public int Version { get; set; }
    public int SubVersion { get; set; }
    public string DTAImport { get; set; }

    public string[] Visemes { get; set; }
    public KeyFrame[] Frames { get; set; } // Sampled at 30 Hz

    public string Name => _name;
    public string Type => "CharLipSync";

    public override string ToString() => $"{Name} ({Frames.Length} key frames)";
  }
}
