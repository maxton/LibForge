using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LibForge.Midi
{
  public class MidiFileResource
  {
    public class TEMPO
    {
      public float StartMillis;
      public uint StartTick;
      public int Tempo;
    }
    public struct TIMESIG
    {
      public int Measure;
      public uint Tick;
      public short Numerator;
      public short Denominator;
    }
    public struct BEAT
    {
      public uint Tick;
      public bool Downbeat;
    }

    public int MidiSongResourceMagic;
    public uint LastTrackFinalTick;
    public MidiCS.MidiTrack[] MidiTracks;
    public uint FinalTick;
    public uint Measures;
    public uint[] Unknown;
    public uint FinalTickMinusOne;
    public float[] UnknownFloats;
    public TEMPO[] Tempos;
    public TIMESIG[] TimeSigs;
    public BEAT[] Beats;
    public int UnknownZero;
    public string[] MidiTrackNames;
  }
}
