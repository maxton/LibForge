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
        public byte IsBRE;
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

    public struct CYMBALMARKER
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

    public struct LANEMARKER
    {
      public struct MARKER {
        public uint StartTick;
        public uint EndTick;
        [Flags]
        public enum Flag : uint
        {
          Glissando = 1,
          Unknown = 2,
          Roll_1Lane = 4,
          Roll_2Lane = 8
        }
        public Flag Flags;
      }
      // First dimension: difficulty
      public MARKER[][] Markers;
    }

    public struct GTRTRILLS
    {
      public struct TRILL
      {
        public uint StartTick;
        public uint EndTick;
        public int LowFret;
        public int HighFret;
      }
      // First dimension: difficulty
      public TRILL[][] Trills;
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
    public struct SECTIONS
    { 
      public struct SECTION
      {
        public uint StartTicks;
        public uint LengthTicks;
      }
      // Only seen 0 and 1 used although there are always 6 entries...
      public enum SectionType : int
      {
        Overdrive = 0,
        Solo = 1
      }
      // 1st dimension: difficulty, 2nd: section type, 3rd: list of sections
      public SECTION[][][] Sections;
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
    public struct MARKUP_SOLO_NOTES
    {
      public uint StartTick;
      public uint EndTick;
      public int NoteOffset;
    }
    public struct UNKSTRUCT2
    {
      public uint Tick1;
      public uint Tick2;
    }
    public struct MARKUPCHORD
    {
      public uint StartTick;
      public uint EndTick;
      public int[] Pitches;
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
    public CYMBALMARKER[] ProMarkers;
    public LANEMARKER[] LaneMarkers;
    public GTRTRILLS[] TrillMarkers;
    public DRUMMIXES[] DrumMixes;
    public GEMTRACK[] GemTracks;
    public SECTIONS[] OverdriveSoloSections;
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
    public int NumPlayableTracks;
    public int Unknown12;
    public byte Unknown13;
    public float PreviewStartMillis;
    public float PreviewEndMillis;
    public HANDMAP[] GuitarHandmap;
    public HANDPOS[] GuitarLeftHandPos;
    public UNKTRACK4[] Unktrack4;
    public MARKUP_SOLO_NOTES[] Unkstruct;
    public UNKSTRUCT2[] Unkstruct2;
    public UNKSTRUCT2[] Unkstruct4;
    public MARKUPCHORD[] MarkupChords1;
    public MARKUPCHORD[] MarkupChords2;
    public MARKUP_SOLO_NOTES[] Unkstruct3;
    public MARKUP_SOLO_NOTES[] Unkstruct5;
    public int UnknownTwo;
    public int Unknown16;
    public MidiTrack[] MidiTracks;
    public int[] UnknownInts;
    public float[] UnknownFloats;
    public TEMPO[] Tempos;
    public TIMESIG[] TimeSigs;
    public BEAT[] Beats;
    public int Unknown19;
    public string[] MidiTrackNames;
  }

}
