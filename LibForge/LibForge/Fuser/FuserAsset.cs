using LibForge.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace LibForge.Fuser
{
  public class FuserAsset
  {
    public long Version;
    public long Version2;
    public int Unk3;
    public long FilenameHash;
    public string Filename;
    public int Unk4;
    public int Unk5;

    public ResourceFile[] ResourceFiles;
    public static FuserAsset Read(Stream s)
    {
      var ret = new FuserAsset();
      ret.Load(s);
      return ret;
    }
    private FuserAsset() { }
    private void Load(Stream s)
    {
      var b = new BinReader(s);
      Version = b.Check(b.Long(), 7);
      Version2 = b.Long();
      if (Version2 == 14 || Version2 == 12)
      {
        // Bunch of unknown values here.
        s.Position += 45;
      }
      else if (Version2 != 7)
      {
        throw new InvalidDataException("Unknown value at 0x8: " + Version2);
      }
      Unk3 = b.Int();
      FilenameHash = b.Long();
      Filename = b.UE4String();
      Unk4 = b.Int();
      Unk5 = b.Int();
      if (Unk4 == 1 && Unk5 == 0)
      {
        ResourceFiles = new ResourceFile[1];
      } 
      else
      {
        ResourceFiles = new ResourceFile[b.Long()];
      }
      for (int i = 0; i < ResourceFiles.Length; i++)
      {
        ResourceFiles[i] = ResourceFile.Read(s);
      }
    }
  }
}
