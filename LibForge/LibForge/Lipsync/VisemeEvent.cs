using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LibForge.Lipsync
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
}
