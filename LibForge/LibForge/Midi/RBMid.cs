using System;
using MidiCS;


namespace LibForge.Midi
{
  public class RBMid
  {
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
      public struct FILL_LANES
      {
        public uint Tick;
        public uint Lanes;
      }
      public struct FILL
      {
        public uint StartTick;
        public uint EndTick;
        public byte IsBRE;
      }
      public FILL_LANES[] Lanes;
      public FILL[] Fills;
    }
    public struct ANIM
    {
      public struct EVENT
      {
        public float StartMillis;
        public uint StartTick;
        public ushort LengthMillis;
        public ushort LengthTicks;
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
      public class MARKER
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
      public class TRILL
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
      public class GEM
      {
        public float StartMillis;
        public uint StartTicks;
        public ushort LengthMillis;
        public ushort LengthTicks;
        public int Lanes;
        public bool IsHopo;
        public bool NoTail;
        public int Unknown;
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
      public class PHRASE_MARKER
      {
        public float StartMillis;
        public float Length;
        public uint StartTicks;
        public uint LengthTicks;
        public int StartNoteIdx;
        public int EndNoteIdx;
        public byte IsPhrase;
        public byte IsOverdrive;
        public byte UnkFlag1;
        public byte UnkFlag2;
        public byte[] Unknown6;
      }
      public struct VOCAL_NOTE
      {
        public int PhraseIndex;
        public int MidiNote;
        public int MidiNote2;
        public float StartMillis;
        public uint StartTick;
        public float LengthMillis;
        public ushort LengthTicks;
        public string Lyric;
        // 9 Bytes are flags
        // 0 0 0 0 0 1 0 0 1 for normal notes
        // 0 0 0 0 0 1 1 0 1 for portamento
        // 0 0 1 0 0 1 0 0 1 for unpitched
        public bool LastNoteInPhrase;
        public bool UnknownFalse;
        public bool Unpitched;
        public bool UnknownFalse2;
        public bool UnkFlag1;
        public byte Unknown;
        public bool Portamento;
        public bool Flag8;
        public bool Flag9;
      }
      public struct VOCAL_TACET
      {
        public float StartMillis;
        public float EndMillis;
      }
      public PHRASE_MARKER[] PhraseMarkers;
      public PHRASE_MARKER[] PhraseMarkers2;
      public VOCAL_NOTE[] Notes;
      public uint[] Percussion;
      public VOCAL_TACET[] Tacets;
    }
    public struct UNKSTRUCT1
    {
      public uint Tick;
      public float FloatData;
    }
    public struct UNKSTRUCT2
    {
      public int Unknown1;
      public int Unknown2;
      public float Unknown3;
      public float Unknown4;
    }
    public struct HANDMAP
    {
      public struct MAP
      {
        public float StartTime;
        public int Map;
      }
      public MAP[] Maps;
    }
    public struct HANDPOS
    {
      public struct POS
      {
        public float StartTime;
        public float Length;
        public int Position;
        public byte Unknown;
      }
      public POS[] Events;
    }
    public struct UNKTRACK
    {
      public struct DATA
      {
        public float FloatData;
        public int IntData;
      }
      public DATA[] Data;
    }
    public struct MARKUP_SOLO_NOTES
    {
      public uint StartTick;
      public uint EndTick;
      public int NoteOffset;
    }
    public struct TWOTICKS
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
    public class RBVREVENTS
    {
      public struct BEATMATCH_SECTION
      { 
        public int unk_zero;
        public string beatmatch_section;
        public uint StartTick;
        public uint EndTick;
      }
      public struct UNKSTRUCT1
      {
        public int Unk1;
        public float StartPercentage;
        public float EndPercentage;
        public uint StartTick;
        public uint EndTick;
        public int Unk2;
      }
      public struct UNKSTRUCT2
      {
        public int Unk;
        public string Name;
        public uint Tick;
      }
      public struct UNKSTRUCT3
      {
        public int Unk1;
        public string exsandohs;
        public uint StartTick;
        public uint EndTick;
        public byte[] Flags;
        public int Unk2;
      }
      public struct UNKSTRUCT4
      {
        public int Unk;
        public string Name;
        public uint StartTick;
        public uint EndTick;
      }
      public struct UNKSTRUCT5
      {
        public int Unk1;
        public string Name;
        public string[] ExsOhs;
        public uint StartTick;
        public uint EndTick;
        public byte Unk2;
      }
      public struct UNKSTRUCT6
      {
        public uint Tick;
        public int Unk;
      }
      public BEATMATCH_SECTION[] BeatmatchSections;
      public UNKSTRUCT1[] UnkStruct1;
      public UNKSTRUCT2[] UnkStruct2;
      public UNKSTRUCT3[] UnkStruct3;
      public UNKSTRUCT4[] UnkStruct4;
      public UNKSTRUCT5[] UnkStruct5;
      public uint[] UnknownTicks;
      public int UnkZero2;
      public UNKSTRUCT6[] UnkStruct6;
    }

    public const int FORMAT_RB4 = 0x10;
    public const int FORMAT_RBVR = 0x2F;
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
    public int UnknownOne;
    public int UnknownNegOne;
    public float UnknownHundred;
    public UNKSTRUCT1[] Unknown4;
    public UNKSTRUCT2[] Unknown5;
    // Takes values 90, 92, 125, 130, 170, 250
    public int Unknown6;
    public uint NumPlayableTracks;
    public uint FinalTick;
    public uint UnkVrTick;
    public byte UnknownZeroByte;
    public float PreviewStartMillis;
    public float PreviewEndMillis;
    public HANDMAP[] GuitarHandmap;
    public HANDPOS[] GuitarLeftHandPos;
    public UNKTRACK[] Unktrack;

    public MARKUP_SOLO_NOTES[] MarkupSoloNotes1;
    public TWOTICKS[] TwoTicks1;
    public MARKUPCHORD[] MarkupChords1;
    public MARKUP_SOLO_NOTES[] MarkupSoloNotes2;
    public MARKUP_SOLO_NOTES[] MarkupSoloNotes3;
    public TWOTICKS[] TwoTicks2;

    public RBVREVENTS VREvents;
    public int UnknownTwo;
    public uint LastMarkupEventTick;
    public MidiTrack[] MidiTracks;
    public int[] UnknownInts;
    public float[] UnknownFloats;
    public TEMPO[] Tempos;
    public TIMESIG[] TimeSigs;
    public BEAT[] Beats;
    public int UnknownZero;
    public string[] MidiTrackNames;
  }

}
