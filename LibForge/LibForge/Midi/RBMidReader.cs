using System;
using System.Collections.Generic;
using System.IO;
using LibForge.Util;
using MidiCS;
using MidiCS.Events;

namespace LibForge.Midi
{
  public class RBMidReader : ReaderBase<RBMid>
  {
    public static RBMid ReadStream(Stream s)
    {
      return new RBMidReader(s).Read();
    }
    public RBMidReader(Stream s) : base(s) { }

    const uint MaxInstTracks = 10;
    const uint MaxKeysAnimTracks = 2;
    const uint MaxVocalTracks = 4;
    public override RBMid Read()
    {
      var r = new RBMid();
      r.Format = Int();
      if(r.Format != RBMid.FORMAT_RB4 && r.Format != RBMid.FORMAT_RBVR)
      {
        throw new Exception($"Invalid magic number (expected 10 or 2f, got {r.Format:X}");
      }
      r.Lyrics = Arr(ReadLyrics, MaxInstTracks);
      var numTracks = (uint)r.Lyrics.Length;
      r.DrumFills = Arr(ReadDrumFills, numTracks);
      r.Anims = Arr(ReadAnims, numTracks);
      r.ProMarkers = Arr(ReadMarkers, numTracks);
      r.LaneMarkers = Arr(ReadUnktrack, numTracks);
      r.TrillMarkers = Arr(ReadUnktrack2, numTracks);
      r.DrumMixes = Arr(ReadDrumMixes, numTracks);
      r.GemTracks = Arr(ReadGemTracks, numTracks);
      r.OverdriveSoloSections = Arr(ReadOverdrives, numTracks);
      r.VocalTracks = Arr(ReadVocalTrack, MaxVocalTracks);
      r.UnknownOne = Check(Int(), 1);
      r.UnknownNegOne = Check(Int(), -1);
      r.UnknownHundred = Check(Float(), 100f);
      r.Unknown4 = Arr(ReadUnkstruct1);
      r.VocalRange = Arr(ReadVocalTrackRange);
      r.HopoThreshold = Int();
      r.NumPlayableTracks = Check(UInt(), numTracks);
      r.FinalEventTick = UInt();
      if(r.Format == RBMid.FORMAT_RBVR)
      {
        r.UnkVrTick = UInt();
      }
      r.UnknownZeroByte = Check(Byte(), (byte)0);
      r.PreviewStartMillis = Float();
      r.PreviewEndMillis = Float();
      r.HandMaps = Arr(() => Arr(ReadMap), numTracks);
      r.GuitarLeftHandPos = Arr(() => Arr(ReadHandPos), numTracks);
      r.StrumMaps = Arr(() => Arr(ReadMap), numTracks);

      r.MarkupSoloNotes1 = Arr(ReadMarkupSoloNotes);
      r.MarkupLoop1 = Arr(ReadTwoTicks);
      r.MarkupChords1 = Arr(ReadMarkupChord);
      r.MarkupSoloNotes2 = Arr(ReadMarkupSoloNotes);
      r.MarkupSoloNotes3 = Arr(ReadMarkupSoloNotes);
      r.MarkupLoop2 = Arr(ReadTwoTicks);

      if(r.Format == RBMid.FORMAT_RBVR)
      {
        r.VREvents = ReadVREvents();
      }
      new MidiFileResourceReader(s).Read(r);
      return r;
    }
    private RBMid.TICKTEXT ReadTickText() => new RBMid.TICKTEXT
    {
      Tick = UInt(),
      Text = String()
    };
    private RBMid.LYRICS ReadLyrics() => new RBMid.LYRICS
    {
      TrackName = String(),
      Lyrics = Arr(ReadTickText),
      Unknown1 = Int(),
      Unknown2 = Int(),
      Unknown3 = Byte()
    };
    private RBMid.DRUMFILLS ReadDrumFills() => new RBMid.DRUMFILLS
    {
      Lanes = Arr(() => new RBMid.DRUMFILLS.FILL_LANES
      {
        Tick = UInt(),
        Lanes = UInt()
      }),
      Fills = Arr(() => new RBMid.DRUMFILLS.FILL
      {
        StartTick = UInt(),
        EndTick = UInt(),
        IsBRE = Byte()
      })
    };
    private RBMid.ANIM ReadAnims() => new RBMid.ANIM
    {
      TrackName = String(),
      Unknown1 = Int(),
      Unknown2 = Int(),
      Events = Arr(() => new RBMid.ANIM.EVENT
      {
        StartMillis = Float(),
        StartTick = UInt(),
        LengthMillis = UShort(),
        LengthTicks = UShort(),
        KeyBitfield = Int(),
        Unknown2 = Int(),
        Unknown3 = Short()
      }),
      Unknown3 = Int()
    };
    private RBMid.TOMMARKER ReadMarkers() => new RBMid.TOMMARKER
    {
      Markers = Arr(() => new RBMid.TOMMARKER.MARKER
      {
        Tick = UInt(),
        Flags = (RBMid.TOMMARKER.MARKER.FLAGS)Int()
      }),
      Unknown1 = Int(),
      Unknown2 = Int()
    };
    private RBMid.LANEMARKER ReadUnktrack() => new RBMid.LANEMARKER
    {
      Markers = Arr(() => Arr(() => new RBMid.LANEMARKER.MARKER
      {
        StartTick = UInt(),
        EndTick = UInt(),
        Lanes = Int()
      }))
    };
    private RBMid.GTRTRILLS ReadUnktrack2() => new RBMid.GTRTRILLS
    {
      Trills = Arr(() => Arr(() => new RBMid.GTRTRILLS.TRILL
      {
        StartTick = UInt(),
        EndTick = UInt(),
        FirstFret = Int(),
        SecondFret = Int()
      }))
    };
    private RBMid.DRUMMIXES ReadDrumMixes() => new RBMid.DRUMMIXES
    {
      Mixes = Arr(() => Arr(ReadTickText))
    };
    private RBMid.GEMTRACK ReadGemTracks() => new RBMid.GEMTRACK
    {
      Gems = Arr(Seq(Skip(4), () => Arr(() => new RBMid.GEMTRACK.GEM
      {
        StartMillis = Float(),
        StartTicks = UInt(),
        LengthMillis = UShort(),
        LengthTicks = UShort(),
        Lanes = Int(),
        IsHopo = Bool(),
        NoTail = Bool(),
        ProCymbal = Int()
      }))),
      HopoThreshold = Int()
    };
    private RBMid.SECTIONS ReadOverdrives() => new RBMid.SECTIONS
    {
      Sections = Arr(() => Arr(() => Arr(() => new RBMid.SECTIONS.SECTION
      {
        StartTicks = UInt(),
        LengthTicks = UInt()
      })))
    };
    private RBMid.VOCALTRACK ReadVocalTrack() => new RBMid.VOCALTRACK
    {
      FakePhraseMarkers = Arr(ReadPhraseMarker),
      AuthoredPhraseMarkers = Arr(ReadPhraseMarker),
      Notes = Arr(() => {
        var note = new RBMid.VOCALTRACK.VOCAL_NOTE
        {
          PhraseIndex = Int(),
          MidiNote = CheckRange(Int(), 0, 127),
          MidiNote2 = CheckRange(Int(), 0, 127),
          StartMillis = Float(),
          StartTick = UInt(),
          LengthMillis = Float(),
          LengthTicks = UShort(),
          Lyric = String(),
          LastNoteInPhrase = Bool(),
          False1 = Check(Bool(), false, nameof(RBMid.VOCALTRACK.VOCAL_NOTE.False1)),
          Unpitched = Bool(),
          UnpitchedGenerous = Bool(),
          RangeDivider = Bool(),
          PhraseFlags = Byte(),
          Portamento = Bool(),
          LyricShift = Bool(),
          ShowLyric = Bool(),
        }; return note; }),
      Percussion = Arr(UInt),
      FreestyleRegions = Arr(() => new RBMid.VOCALTRACK.OD_REGION
      {
        StartMillis = Float(),
        EndMillis = Float()
      })
    };
    private RBMid.VOCALTRACK.PHRASE_MARKER ReadPhraseMarker() =>
      new RBMid.VOCALTRACK.PHRASE_MARKER
      {
        StartMillis = Float(),
        LengthMillis = Float(),
        StartTicks = UInt(),
        LengthTicks = UInt(),
        StartNoteIdx = Int(),
        EndNoteIdx = Int(),
        HasPitchedVox = Bool(),
        HasUnpitchedVox = Bool().Then(Skip(9)),
        // 9 bytes here are zero in every single rbmid I have found. So we are skipping them.
        LowNote = Float(),
        HighNote = Float(),
        PhraseFlags = Byte(),
        PercussionSection = Bool().Then(() => { Check(Int(), 0); Check(Int(), 0); }),
        // 8 bytes here are zero in every single rbmid I have found. So we are skipping them.
      };
    private RBMid.UNKSTRUCT1 ReadUnkstruct1() => new RBMid.UNKSTRUCT1
    {
      Tick = UInt(),
      FloatData = Float()
    };
    private RBMid.VocalTrackRange ReadVocalTrackRange() => new RBMid.VocalTrackRange
    {
      StartMillis = Float(),
      StartTicks = Int(),
      LowNote = Float(),
      HighNote = Float(),
    };
    private RBMid.MAP ReadMap() => new RBMid.MAP
    {
      StartTime = Float(),
      Map = Int()
    };
    private RBMid.HANDPOS ReadHandPos() => new RBMid.HANDPOS
    {
      StartTime = Float(),
      Length = Float(),
      Position = Int(),
      Unknown = Byte()
    };
    private RBMid.MARKUP_SOLO_NOTES ReadMarkupSoloNotes() => new RBMid.MARKUP_SOLO_NOTES
    {
      StartTick = UInt(),
      EndTick = UInt(),
      NoteOffset = Int()
    };
    private RBMid.TWOTICKS ReadTwoTicks() => new RBMid.TWOTICKS
    {
      StartTick = UInt(),
      EndTick = UInt()
    };
    private RBMid.MARKUPCHORD ReadMarkupChord() => new RBMid.MARKUPCHORD
    {
      StartTick = UInt(),
      EndTick = UInt(),
      Pitches = Arr(Int)
    };
    
    private RBMid.RBVREVENTS ReadVREvents() => new RBMid.RBVREVENTS
    {
      BeatmatchSections = Arr(() => new RBMid.RBVREVENTS.BEATMATCH_SECTION
      {
        unk_zero = Check(Int(), 0),
        beatmatch_section = String(),
        StartTick = UInt(),
        EndTick = UInt()
      }),
      UnkStruct1 = Arr(() => new RBMid.RBVREVENTS.UNKSTRUCT1
      {
        Unk1 = Int(),
        StartPercentage = Float(),
        EndPercentage = Float(),
        StartTick = UInt(),
        EndTick = UInt(),
        Unk2 = Int()
      }),
      UnkStruct2 = Arr(() => new RBMid.RBVREVENTS.UNKSTRUCT2
      {
        Unk = Int(),
        Name = String(),
        Tick = UInt()
      }),
      UnkStruct3 = Arr(() => new RBMid.RBVREVENTS.UNKSTRUCT3
      {
        Unk1 = Int(),
        exsandohs = String(),
        StartTick = UInt(),
        EndTick = UInt(),
        Flags = FixedArr(Byte, 7),
        Unk2 = Int()
      }),
      UnkStruct4 = Arr(() => new RBMid.RBVREVENTS.UNKSTRUCT4
      {
        Unk = Int(),
        Name = String(),
        StartTick = UInt(),
        EndTick = UInt()
      }),
      UnkStruct5 = Arr(() => new RBMid.RBVREVENTS.UNKSTRUCT5
      {
        Unk1 = Int(),
        Name = String(),
        ExsOhs = Arr(String),
        StartTick = UInt(),
        EndTick = UInt(),
        Unk2 = Byte()
      }),
      UnknownTicks = Arr(UInt),
      UnkZero2 = Check(Int(), 0),
      UnkStruct6 = Arr(() => new RBMid.RBVREVENTS.UNKSTRUCT6
      {
        Tick = UInt(),
        Unk = Int()
      })
    };
  }
}
