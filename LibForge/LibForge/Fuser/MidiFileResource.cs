using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace LibForge.Fuser
{
  public class MidiFileResource : ResourceFile
  {
    public LibForge.Midi.MidiFileResource MidiFile;
    public override void Load(Stream s)
    {
      MidiFile = new LibForge.Midi.MidiFileResourceReader(s).Read();
    }
  }
}
