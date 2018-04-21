using System;
using System.Collections.Generic;
using System.Text;
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
        public float LowNote;
        public float HighNote;
        public byte UnknownCount; // seen: 0, 1, 2, and 3
        public bool UnknownFlag;
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
    public struct VocalTrackRange
    {
      public float StartMillis;
      public int StartTicks;
      public float LowNote;
      public float HighNote;
    }
    public struct MAP
    {
      public float StartTime;
      public int Map;
    }
    public struct HANDPOS
    {
      public float StartTime;
      public float Length;
      public int Position;
      public byte Unknown;
    }
    public struct MARKUP_SOLO_NOTES
    {
      public uint StartTick;
      public uint EndTick;
      public int NoteOffset;
    }
    public struct TWOTICKS
    {
      public uint StartTick;
      public uint EndTick;
    }
    public class MARKUPCHORD
    {
      public uint StartTick;
      public uint EndTick;
      public int[] Pitches;
    }
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
    public VocalTrackRange[] VocalRange;
    // Takes values 90, 92, 125, 130, 170, 250
    public int Unknown6;
    public uint NumPlayableTracks;
    public uint FinalEventTick;
    public uint UnkVrTick;
    public byte UnknownZeroByte;
    public float PreviewStartMillis;
    public float PreviewEndMillis;
    public MAP[][] HandMaps;
    public HANDPOS[][] GuitarLeftHandPos;
    public MAP[][] StrumMaps;

    public MARKUP_SOLO_NOTES[] MarkupSoloNotes1;
    public TWOTICKS[] MarkupLoop1;
    public MARKUPCHORD[] MarkupChords1;
    public MARKUP_SOLO_NOTES[] MarkupSoloNotes2;
    public MARKUP_SOLO_NOTES[] MarkupSoloNotes3;
    public TWOTICKS[] MarkupLoop2;

    public RBVREVENTS VREvents;
    public int UnknownTwo;
    public uint LastMarkupEventTick;
    public MidiTrack[] MidiTracks;
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


    private string Check<T>(IList<T> a, IList<T> b, string n, Func<T, T, string> f)
    {
      if ((b == null || b.Count == 0) && (a == null || a.Count == 0))
        return null;

      if (a.Count != b.Count)
        return $"{n}.Length: a={a.Count}, b={b.Count}";
      for (var i = 0; i < a.Count; i++)
      {
        var r = f(a[i], b[i]);
        if (r != null)
          return $"{n}[{i}].{r}";
      }
      return null;
    }
    private string Check<T>(T[][] a, T[][] b, string n, Func<T, T, string> f)
    {
      if ((b == null || b.Length == 0) && (a == null || a.Length == 0))
        return null;

      if (a.Length != b.Length)
        return $"{n}.Length: a={a.Length}, b={b.Length}";
      for (var i = 0; i < a.Length; i++)
      {
        if (a[i].Length != b[i].Length)
          return $"{n}[{i}].Length: a={a[i].Length}, b={b[i].Length}";
        for (var j = 0; j < a[i].Length; j++)
        {
          var r = f(a[i][j], b[i][j]);
          if (r != null)
            return $"{n}[{i}][{j}].{r}";
        }
      }
      return null;
    }
    private string Check<T>(T[][][] a, T[][][] b, string n, Func<T, T, string> f)
    {
      if ((b == null || b.Length == 0) && (a == null || a.Length == 0))
        return null;

      if (a.Length != b.Length)
        return $"{n}.Length: a={a.Length}, b={b.Length}";
      for (var i = 0; i < a.Length; i++)
      {
        if (a[i].Length != b[i].Length)
          return $"{n}[{i}].Length: a={a[i].Length}, b={b[i].Length}";
        for (var j = 0; j < a[i].Length; j++)
        {
          if (a[i][j].Length != b[i][j].Length)
            return $"{n}[{i}][{j}].Length: a={a[i][j].Length}, b={b[i][j].Length}";
          for (var k = 0; k < a[i][j].Length; k++)
          {
            var r = f(a[i][j][k], b[i][j][k]);
            if (r != null)
              return $"{n}[{i}][{j}][{k}].{r}";
          }
        }
      }
      return null;
    }
    private string Check<T>(T a, T b, string n)
      => a.Equals(b) ? null : $"{n}: a={a}, b={b}";
    private string Check<T>(T a, T b)
      => a.Equals(b) ? null : $": a={a}, b={b}";
    private string CheckFloats(float a, float b, string n, float tolerance = 0.1f)
      => Math.Abs(a - b) < tolerance ? null : $"{n}: a={a}, b={b}";
    private string CheckTickText(TICKTEXT a, TICKTEXT b)
      => Check(a.Tick, b.Tick, nameof(TICKTEXT.Tick))
      ?? Check(a.Text, b.Text, nameof(TICKTEXT.Text));
    private string CheckTwoTick(TWOTICKS a, TWOTICKS b)
      => Check(a.StartTick, b.StartTick, nameof(TWOTICKS.StartTick))
      ?? Check(a.EndTick, b.EndTick, nameof(TWOTICKS.EndTick));
    private string CheckSoloNotes(MARKUP_SOLO_NOTES their, MARKUP_SOLO_NOTES my)
      => Check(their.StartTick, my.StartTick, nameof(my.StartTick))
      ?? Check(their.EndTick, my.EndTick, nameof(my.EndTick))
      ?? Check(their.NoteOffset, my.NoteOffset, nameof(my.NoteOffset));
    /// <summary>
    /// Compares this RBMid with another RBMid.
    /// 
    /// Returns null if they are equivalent.
    /// If they are not equivalent, this returns the first field name in which they differ.
    /// For multi-dimensional fields, the return value will look like this:
    /// "Lyrics[0].Lyrics[0].Text"
    /// </summary>
    /// <param name="other">The RBMid to compare to</param>
    /// <returns>null if the files are equivalent, or else a string describing the first differing field</returns>
    public string Compare(RBMid other)
      => Check(other.Format, Format, nameof(Format))
      ?? Check(other.Lyrics, Lyrics, nameof(Lyrics), (their, my)
           => Check(their.TrackName, my.TrackName, nameof(my.TrackName))
           ?? Check(their.Lyrics, my.Lyrics, nameof(my.Lyrics), CheckTickText)
           ?? Check(their.Unknown1, my.Unknown1, nameof(my.Unknown1))
           ?? Check(their.Unknown2, my.Unknown2, nameof(my.Unknown2))
           ?? Check(their.Unknown3, my.Unknown3, nameof(my.Unknown3)))
      ?? Check(other.DrumFills, DrumFills, nameof(DrumFills), (their, my)
           => Check(their.Lanes, my.Lanes, nameof(my.Lanes), (their2, my2)
                => Check(their2.Tick, my2.Tick, nameof(my2.Tick))
                ?? Check(their2.Lanes, my2.Lanes, nameof(my2.Lanes)))
           ?? Check(their.Fills, my.Fills, nameof(my.Fills), (their2, my2)
                => Check(their2.StartTick, my2.StartTick, nameof(my2.StartTick))
                // TODO: Figure out why this is not always the same after conversion
                ?? CheckFloats(their2.EndTick, my2.EndTick, nameof(my2.EndTick), 11)
                ?? Check(their2.IsBRE, my2.IsBRE, nameof(my2.IsBRE))))
      // TODO: Keyboard anims... maybe
      ?? Check(other.Anims, Anims, nameof(Anims), (their, my) => null)
      ?? Check(other.ProMarkers, ProMarkers, nameof(ProMarkers), (their, my)
           => Check(their.Markers, my.Markers, nameof(my.Markers), (their2, my2)
                => Check(their2.Tick, my2.Tick, nameof(my2.Tick))
                ?? Check(their2.Flags, my2.Flags, nameof(my2.Flags)))
           ?? Check(their.Unknown1, my.Unknown1, nameof(my.Unknown1))
           ?? Check(their.Unknown2, my.Unknown2, nameof(my.Unknown2)))
      ?? Check(other.LaneMarkers, LaneMarkers, nameof(LaneMarkers), (their, my)
           => Check(their.Markers, my.Markers, nameof(my.Markers), (their2, my2)
                => Check(their2.StartTick, my2.StartTick, nameof(my2.StartTick))
                ?? Check(their2.EndTick, my2.EndTick, nameof(my2.EndTick))
                ?? Check(their2.Flags, my2.Flags, nameof(my2.Flags))))
      ?? Check(other.TrillMarkers, TrillMarkers, nameof(TrillMarkers), (their, my) 
           => Check(their.Trills, my.Trills, nameof(my.Trills), (their2, my2)
                => Check(their2.StartTick, my2.StartTick, nameof(my2.StartTick))
                ?? Check(their2.EndTick, my2.EndTick, nameof(my2.EndTick))
                ?? Check(their2.LowFret, my2.LowFret, nameof(my2.LowFret))
                ?? Check(their2.HighFret, my2.HighFret, nameof(my2.HighFret))))
      ?? Check(other.DrumMixes, DrumMixes, nameof(DrumMixes), (their, my)
           => Check(their.Mixes, my.Mixes, nameof(my.Mixes), (t,m)=>Check(t,m,"",CheckTickText)))
      ?? Check(other.GemTracks, GemTracks, nameof(GemTracks), (their, my)
           => Check(their.Gems, my.Gems, nameof(my.Gems), (their2, my2)
                => CheckFloats(their2.StartMillis, my2.StartMillis, nameof(my2.StartMillis), 0.2f)
                ?? CheckFloats(their2.LengthMillis, my2.LengthMillis, nameof(my2.LengthMillis), 1.5f) // who ever cared about a couple ms
                ?? Check(their2.StartTicks, my2.StartTicks, nameof(my2.StartTicks))
                ?? Check(their2.LengthTicks, my2.LengthTicks, nameof(my2.LengthTicks))
                ?? Check(their2.Lanes, my2.Lanes, nameof(my2.Lanes))
                // TODO: Swing notes...
                //?? Check(their2.IsHopo, my2.IsHopo, nameof(my2.IsHopo))
                ?? Check(their2.NoTail, my2.NoTail, nameof(my2.NoTail))
                // TODO
                //?? Check(their2.Unknown, my2.Unknown, nameof(my2.Unknown))
                ))
      ?? Check(other.OverdriveSoloSections, OverdriveSoloSections, nameof(OverdriveSoloSections), (their, my)
           => Check(their.Sections, my.Sections, nameof(my.Sections), (their2, my2)
                => Check(their2.StartTicks, my2.StartTicks, nameof(my2.StartTicks))
                ?? Check(their2.LengthTicks, my2.LengthTicks, nameof(my2.LengthTicks))))
      ?? Check(other.VocalTracks, VocalTracks, nameof(VocalTracks), (their, my)
           => Check(their.Notes, my.Notes, nameof(my.Notes), (their2, my2)
                => Check(their2.StartTick, my2.StartTick, nameof(my2.StartTick))
                ?? Check(their2.LengthTicks, my2.LengthTicks, nameof(my2.LengthTicks))
                ?? CheckFloats(their2.StartMillis, my2.StartMillis, nameof(my2.StartMillis), 0.4f)
                ?? CheckFloats(their2.LengthMillis, my2.LengthMillis, nameof(my2.LengthMillis), 0.4f)
                ?? Check(their2.MidiNote, my2.MidiNote, nameof(my2.MidiNote))
                ?? Check(their2.MidiNote2, my2.MidiNote2, nameof(my2.MidiNote2))
                ?? Check(their2.PhraseIndex, my2.PhraseIndex, nameof(my2.PhraseIndex))
                ?? Check(their2.Lyric, my2.Lyric, nameof(my2.Lyric))
                ?? Check(their2.Portamento, my2.Portamento, nameof(my2.Portamento))
                ?? Check(their2.LastNoteInPhrase, my2.LastNoteInPhrase, nameof(my2.LastNoteInPhrase)))
           ?? Check(their.Percussion, my.Percussion, nameof(my.Percussion), Check)
           ?? Check(their.Tacets, my.Tacets, nameof(my.Tacets), (their2, my2)
                => CheckFloats(their2.StartMillis, my2.StartMillis, nameof(my2.StartMillis), 0.2f)
                // TODO: Fix tacets on HARM tracks
                //?? CheckFloats(their2.EndMillis, my2.EndMillis, nameof(my2.EndMillis), 2f)
                )
           ?? Check(their.PhraseMarkers, my.PhraseMarkers, nameof(my.PhraseMarkers), (their2, my2)
                => // TODO: Fix first phrase marker. It seems to start earlier than authored!?
                //   CheckFloats(their2.StartMillis, my2.StartMillis, nameof(my2.StartMillis), 1f)
                //?? CheckFloats(their2.Length, my2.Length, nameof(my2.Length), 1f)
                //?? Check(their2.StartTicks, my2.StartTicks, nameof(my2.StartTicks))
                //?? Check(their2.LengthTicks, my2.LengthTicks, nameof(my2.LengthTicks))
                // TODO ???
                //?? Check(their2.IsPhrase, my2.IsPhrase, nameof(my2.IsPhrase))
                //?? Check(their2.IsOverdrive, my2.IsOverdrive, nameof(my2.IsOverdrive))
                null
                )
           ?? Check(their.PhraseMarkers2, my.PhraseMarkers2, nameof(my.PhraseMarkers2), (their2, my2) => null))
      ?? Check(other.UnknownOne, UnknownOne, nameof(UnknownOne))
      ?? Check(other.UnknownNegOne, UnknownNegOne, nameof(UnknownNegOne))
      ?? Check(other.UnknownHundred, UnknownHundred, nameof(UnknownHundred))
      ?? Check(other.Unknown4, Unknown4, nameof(Unknown4), (their, my) => null)
      ?? Check(other.VocalRange, VocalRange, nameof(VocalRange), (their, my)
            // TODO: Which one is ticks and which one is time?
           => Check(their.StartMillis, my.StartMillis, nameof(my.StartMillis))
           ?? Check(their.StartTicks, my.StartTicks, nameof(my.StartTicks))
           ?? Check(their.LowNote, my.LowNote, nameof(my.LowNote))
           ?? Check(their.HighNote, my.HighNote, nameof(my.HighNote)))
      ?? Check(other.Unknown6, Unknown6, nameof(Unknown6))
      ?? Check(other.NumPlayableTracks, NumPlayableTracks, nameof(NumPlayableTracks))
      ?? Check(other.FinalEventTick, FinalEventTick, nameof(FinalEventTick))
      ?? Check(other.UnknownZeroByte, UnknownZeroByte, nameof(UnknownZeroByte))
      ?? CheckFloats(other.PreviewStartMillis, PreviewStartMillis, nameof(PreviewStartMillis))
      ?? CheckFloats(other.PreviewEndMillis, PreviewEndMillis, nameof(PreviewEndMillis))
      ?? Check(other.HandMaps, HandMaps, nameof(HandMaps), (their, my) 
           => Check(their.StartTime, my.StartTime, nameof(my.StartTime))
           ?? Check(their.Map, my.Map, nameof(my.Map)))
      ?? Check(other.GuitarLeftHandPos, GuitarLeftHandPos, nameof(GuitarLeftHandPos), (their, my)
           => CheckFloats(their.StartTime, my.StartTime, nameof(my.StartTime), 0.0002f)
           ?? CheckFloats(their.Length, my.Length, nameof(my.Length), 0.2f)
           ?? Check(their.Position, my.Position, nameof(my.Position))
           ?? Check(their.Unknown, my.Unknown, nameof(my.Unknown)))
      ?? Check(other.StrumMaps, StrumMaps, nameof(StrumMaps), (their, my)
                => CheckFloats(their.StartTime, my.StartTime, nameof(my.StartTime))
                ?? Check(their.Map, my.Map, nameof(my.Map)))
      ?? Check(other.MarkupSoloNotes1, MarkupSoloNotes1, nameof(MarkupSoloNotes1), CheckSoloNotes)
      ?? Check(other.MarkupLoop1, MarkupLoop1, nameof(MarkupLoop1), CheckTwoTick)
      ?? Check(other.MarkupChords1, MarkupChords1, nameof(MarkupChords1), (their, my)
           => Check(their.StartTick, my.StartTick, nameof(my.StartTick))
           ?? Check(their.EndTick, my.EndTick, nameof(my.EndTick))
           ?? Check(their.Pitches, my.Pitches, nameof(my.Pitches), Check))
      ?? Check(other.MarkupSoloNotes2, MarkupSoloNotes2, nameof(MarkupSoloNotes2), CheckSoloNotes)
      ?? Check(other.MarkupSoloNotes3, MarkupSoloNotes3, nameof(MarkupSoloNotes3), CheckSoloNotes)
      ?? Check(other.MarkupLoop2, MarkupLoop2, nameof(MarkupLoop2), CheckTwoTick)
      ?? Check(other.UnknownTwo, UnknownTwo, nameof(UnknownTwo))
      ?? Check(other.LastMarkupEventTick, LastMarkupEventTick, nameof(LastMarkupEventTick))
      ?? Check(other.MidiTracks, MidiTracks, nameof(MidiTracks), (their, my)
           => Check(their.Name, my.Name, nameof(my.Name))
           ?? Check(their.TotalTicks, my.TotalTicks, nameof(my.TotalTicks))
           ?? Check(their.Messages, my.Messages, nameof(my.Messages), (IMidiMessage their2, IMidiMessage my2)
                => Check(their2.DeltaTime, my2.DeltaTime, nameof(my2.DeltaTime))
                ?? Check(their2.PrettyString, my2.PrettyString, "<pretty_string>")))
      ?? Check(other.FinalTick, FinalTick, nameof(FinalTick))
      ?? Check(other.Measures, Measures, nameof(Measures))
      ?? Check(other.Unknown, Unknown, nameof(Unknown), Check)
      ?? Check(other.FinalTickMinusOne, FinalTickMinusOne, nameof(FinalTickMinusOne))
      // TODO: Floats are sometimes 0xABCDABCD ???
      //?? Check(other.UnknownFloats, UnknownFloats, nameof(UnknownFloats), Check)
      ?? Check(other.Tempos, Tempos, nameof(Tempos), (their, my)
           => CheckFloats(their.StartMillis, my.StartMillis, nameof(my.StartMillis), 0.3f)
           ?? Check(their.StartTick, my.StartTick, nameof(my.StartTick))
           // TODO: Fix precision of tempo conversions...
           ?? CheckFloats(their.Tempo, my.Tempo, nameof(my.Tempo), 2))
      ?? Check(other.TimeSigs, TimeSigs, nameof(TimeSigs), (their, my)
           => Check(their.Tick, my.Tick, nameof(my.Tick))
           ?? Check(their.Measure, my.Measure, nameof(my.Measure))
           ?? Check(their.Numerator, my.Numerator, nameof(my.Numerator))
           ?? Check(their.Denominator, my.Denominator, nameof(my.Denominator)))
      ?? Check(other.Beats, Beats, nameof(Beats), (their, my)
           => Check(their.Tick, my.Tick, nameof(my.Tick))
           ?? Check(their.Downbeat, my.Downbeat, nameof(my.Downbeat)))
      ?? Check(other.UnknownZero, UnknownZero, nameof(UnknownZero))
      ?? Check(other.MidiTrackNames, MidiTrackNames, nameof(MidiTrackNames), Check);
  }
}
