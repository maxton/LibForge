using LibForge.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace LibForge.Fuser
{
  public abstract class ResourceFile
  {
    public int Unk1;
    public string Filename;
    public int Unk2;
    public string Type;
    public long Size;

    public static ResourceFile Read(Stream s)
    {
      var b = new BinReader(s);
      var unk1 = b.Int();
      var filename = b.UE4String();
      var unk2 = b.Int();
      var type = b.UE4String();
      var size = b.Long();
      ResourceFile ret;
      switch(type)
      {
        case "FusionPatchResource":
          ret = new FusionPatchResource();
          break;
        case "MidiFileResource":
          ret = new MidiFileResource();
          break;
        //case "MidiMusicResource":
        //  ret = new MidiMusicResource();
        //  break;
        case "MoggSampleResource":
          ret = new MoggSampleResource();
          break;
        default:
          ret = new UnknownResource();
          break;
      }
      ret.Unk1 = unk1;
      ret.Filename = filename;
      ret.Unk2 = unk2;
      ret.Type = type;
      ret.Size = size;
      ret.Load(s);
      return ret;
    }

    public abstract void Load(Stream s);
  }
}
