using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using LibForge.Extensions;
using GameArchives.Common;
using MidiCS;


namespace LibForge.Midi
{
  public class RBMid
  {
    public MidiFile ToMidiFile() =>
      new MidiFile(MidiFormat.MultiTrack, new List<MidiTrack>(MidiTracks), 480);

    public struct TICKTEXT
    {
      public uint Tick;
      public string Text;
    }
    public struct LYRICS
    {
      public string TrackName;
      public TICKTEXT[] Lyrics;
      public int Unknown1;
      public int Unknown2;
      public byte Unknown3;
    }

    public struct DRUMFILLS
    {
      public struct FILLS_UNK
      {
        public uint Tick;
        public uint Unknown;
      }
      public struct DRUMFILL
      {
        public uint StartTick;
        public uint EndTick;
        public byte Unknown;
      }
      public FILLS_UNK[] Unknown;
      public DRUMFILL[] Fills;
    }
    public struct ANIM
    {
      public struct EVENT
      {
        public float Time;
        public uint Tick;
        public int Unknown1;
        public int KeyBitfield;
        public int Unknown2;
        public short Unknown3;
      }
      public string TrackName;
      public int Unknown1;
      public int Unknown2;
      public EVENT[] Events;
      public int Unknown3;
    }

    public struct MARKERS
    {
      public struct MARKER
      {
        public uint Tick;
        [Flags]
        public enum FLAGS : int {
          Unk = 1,
          Unk2 = 2,
          ProYellow = 4,
          ProBlue = 8,
          ProGreen = 16
        }
        public FLAGS Flags;
      }
      public MARKER[] Markers;
      public int Unknown1;
      public int Unknown2;
    }

    public struct UNKTRACK
    {
      // TODO
    }

    public struct UNKTRACK2
    {
      public struct DATA
      {
        public uint Tick1;
        public uint Tick2;
        public int Unknown1;
        public int Unknown2;
      }
      public DATA[][] Data;
    }

    public struct DRUMMIXES
    {
      // 1st dimension: difficulties
      public TICKTEXT[][] Mixes;
    }

    public struct GEMTRACK
    {
      public struct GEM
      {
        public float StartMillis;
        public uint StartTicks;
        public short Unknown;
        public short Unknown2;
        public byte Unknown3;
        public int Unknown4;
        public byte Unknown5;
        public int Unknown6;
      }
      public GEM[][] Gems;
      public int Unknown;
    }
    public struct UNKTRACK3
    { 
      public struct EVENT
      {
        public uint Tick1;
        public uint Tick2;
      }
      public EVENT[][][] Unknown;
    }
    public struct VOCALTRACK
    {
      public struct PHRASE_MARKER
      {
        public float StartMillis;
        public float Length;
        public int Unknown1;
        public int Unknown2;
        public int Unknown3;
        public int Unknown4;
        public int Unknown5;
        public byte[] Unknown6;
      }
      public struct VOCAL_NOTE
      {
        public int Type;
        public int MidiNote;
        public int MidiNote2;
        public float StartMillis;
        public uint StartTick;
        public float Length;
        public short Unknown1;
        public string Lyric;
        public byte[] Unknown2;
      }
      public struct UNKNOWN
      {
        public float Unknown1;
        public float Unknown2;
      }
      public PHRASE_MARKER[] PhraseMarkers;
      public PHRASE_MARKER[] PhraseMarkers2;
      public VOCAL_NOTE[] Notes;
      public int[] Unknown1;
      public UNKNOWN[] Unknown2;
    }
    public struct HANDMAP
    {
      public struct MAP
      {
        public float StartMillis;
        public int Map;
      }
      public MAP[] Maps;
    }
    public struct HANDPOS
    {
      public struct POS
      {
        public float StartMillis;
        public float EndMillis;
        public int Position;
        public byte Unknown;
      }
      public POS[] Events;
    }
    public struct UNKTRACK4
    {
      public float[] Unknown;
    }
    public struct UNKSTRUCT
    {
      public uint StartTick;
      public uint EndTick;
      public int Unknown;
    }
    public struct UNKMARKUP {
      public uint StartTick;
      public uint EndTick;
      public int[] Unknown;
    }
    public struct TEMPO
    {
      public float StartMillis;
      public uint StartTick;
      public int Tempo;
    }
    public struct TIMESIG
    {
      public int Unknown;
      public uint Tick;
      public short Numerator;
      public short Denominator;
    }
    public struct BEAT
    {
      public uint Tick;
      public bool Downbeat;
    }
    public int Format;
    public LYRICS[] Lyrics;
    public DRUMFILLS[] DrumFills;
    public ANIM[] Anims;
    public MARKERS[] ProMarkers;
    public UNKTRACK[] UnkTrack;
    public UNKTRACK2[] UnkTrack2;
    public DRUMMIXES[] DrumMixes;
    public GEMTRACK[] GemTracks;
    public UNKTRACK3[] UnkTrack3;
    public VOCALTRACK[] VocalTracks;
    public int Unknown1;
    public int Unknown2;
    public float Unknown3;
    public int Unknown4;
    public int Unknown5;
    public int Unknown6;
    public int Unknown7;
    public float Unknown8;
    public float Unknown9;
    public int Unknown10;
    public int Unknown11;
    public int Unknown12;
    public byte Unknown13;
    public float PreviewStartMillis;
    public float PreviewEndMillis;
    public HANDMAP[] GuitarHandmap;
    public HANDPOS[] GuitarLeftHandPos;
    public UNKTRACK4[] Unktrack4;
    public UNKSTRUCT[] Unkstruct;
    public UNKMARKUP[] UnkMarkup;
    public UNKSTRUCT[] Unkstruct2;
    public MidiCS.MidiTrack[] MidiTracks;
    public TEMPO[] Tempos;
    public TIMESIG[] TimeSigs;
    public BEAT[] Beats;
    public string[] MidiTrackNames;
  }

}
