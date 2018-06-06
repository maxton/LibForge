using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LibForge.Lipsync
{
  public class Lipsync
  {
    public int Version;
    public int Subtype;
    public float FrameRate;
    // Names for the viseme key names
    public string[] Visemes;
    // Players with visemes
    public string[] Players;
    public uint[] FrameIndices;
    public byte[] FrameData;
    //public FrameGroup[] Groups;
  }
}
