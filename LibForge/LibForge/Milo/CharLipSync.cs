using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using LibForge.Extensions;

namespace LibForge.Milo
{
  public struct VisemeEvent
  {
    public VisemeEvent(string name, byte weight)
    {
      VisemeName = name;
      Weight = weight;
    }

    public string VisemeName;
    public byte Weight;

    public override string ToString() => $"{VisemeName} ({Weight})";
  }

  public struct KeyFrame
  {
    public List<VisemeEvent> Events;
    public override string ToString() => string.Join(", ", Events.Select(x => x.ToString()));
  }

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
      lipsync.Visemes = Enumerable.Range(0, visemeCount).Select(x => stream.ReadLengthUTF8(true)).ToList();

      int keyFrameCount = stream.ReadInt32BE();
      stream.Position += 4; // Skips total size of following data

      lipsync.Frames = new List<KeyFrame>();
      for (int i = 0; i < keyFrameCount; i++)
      {
        int eventCount = stream.ReadByte();

        KeyFrame frame = new KeyFrame();
        frame.Events = new List<VisemeEvent>();

        for (int j = 0; j < eventCount; j++)
        {
          frame.Events.Add(new VisemeEvent(lipsync.Visemes[stream.ReadByte()], (byte)stream.ReadByte()));
        }

        lipsync.Frames.Add(frame);
      }

      // There's some other data ofter this sometimes but it's not needed

      return lipsync;
    }

    public int Version { get; set; }
    public int SubVersion { get; set; }
    public string DTAImport { get; set; }

    public List<string> Visemes { get; set; }
    public List<KeyFrame> Frames { get; set; } // Sampled at 30 Hz

    public string Name => _name;
    public string Type => "CharLipSync";

    public override string ToString() => $"{Name} ({Frames.Count} key frames)";
  }
}
