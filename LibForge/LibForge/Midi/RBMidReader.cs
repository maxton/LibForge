using System;
using System.Collections.Generic;
using System.IO;
using LibForge.Util;
using MidiCS;
using MidiCS.Events;

namespace LibForge.Midi
{
  public class RBMidReader : ReaderBase
  {
    public static RBMid ReadStream(Stream s)
    {
      return new RBMidReader(s).Read();
    }
    public RBMidReader(Stream s) : base(s) { }
    
    private RBMid Read()
    {
      return new RBMid
      {
        Format = Int(),
        Lyrics = Arr(ReadLyrics),
        DrumFills = Arr(ReadDrumFills),
        Anims = Arr(ReadAnims),
        ProMarkers = Arr(ReadMarkers),
        UnkTrack = Arr(ReadUnktrack),
        UnkTrack2 = Arr(ReadUnktrack2),
        DrumMixes = Arr(ReadDrumMixes),
        GemTracks = Arr(ReadGemTracks),
        UnkTrack3 = Arr(ReadUnktrack3),
        VocalTracks = Arr(ReadVocalTrack),
        Unknown1 = Int(),
        Unknown2 = Int(),
        Unknown3 = Float(),
        Unknown4 = Int(),
        Unknown5 = Int(),
        Unknown6 = Int(),
        Unknown7 = Int(),
        Unknown8 = Float(),
        Unknown9 = Float(),
        Unknown10 = Int(),
        Unknown11 = Int(),
        Unknown12 = Int(),
        Unknown13 = Byte(),
        Unknown14 = Float(),
        Unknown15 = Float(),
        GuitarHandmap = Arr(ReadHandMap),
        GuitarLeftHandPos = Arr(ReadHandPos),
        Unktrack4 = Arr(ReadUnktrack4),
        Unkstruct = Arr(ReadUnkstruct).Then(Skip(4)),
        UnkMarkup = Arr(ReadUnkmarkup).Then(Skip(4)),
        Unkstruct2 = Arr(ReadUnkstruct).Then(Skip(12)),
        MidiTracks = Arr(ReadMidiTrack).Then(Skip(13*4)),
        Tempos = Arr(ReadTempo),
        TimeSigs = Arr(ReadTimesig),
        Beats = Arr(ReadBeat).Then(Skip(4)),
        MidiTrackNames = Arr(String)
      };
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
      Unknown = Arr(() => new RBMid.DRUMFILLS.FILLS_UNK
      {
        Tick = UInt(),
        Unknown = UInt()
      }),
      Fills = Arr(() => new RBMid.DRUMFILLS.DRUMFILL
      {
        StartTick = UInt(),
        EndTick = UInt(),
        Unknown = Byte()
      })
    };
    private RBMid.ANIM ReadAnims() => new RBMid.ANIM
    {
      TrackName = String(),
      Unknown1 = Int(),
      Unknown2 = Int(),
      Events = Arr(() => new RBMid.ANIM.EVENT
      {
        Time = Float(),
        Tick = UInt(),
        Unknown1 = Int(),
        KeyBitfield = Int(),
        Unknown2 = Int(),
        Unknown3 = Short()
      }),
      Unknown3 = Int()
    };
    private RBMid.MARKERS ReadMarkers() => new RBMid.MARKERS
    {
      Markers = Arr(() => new RBMid.MARKERS.MARKER
      {
        Tick = UInt(),
        Flags = (RBMid.MARKERS.MARKER.FLAGS)Int()
      }),
      Unknown1 = Int(),
      Unknown2 = Int()
    };
    private RBMid.UNKTRACK ReadUnktrack()
    {
      var ret = default(RBMid.UNKTRACK);
      var num = Int();
      if(num > 0)
      {
        for (var i = 0; i < num; i++) {
          var data = Int();
          if (data > 0)
          {
            Skip(8);
          }
        }
        Skip(4)();
      }
      return ret;
    }
    private RBMid.UNKTRACK2 ReadUnktrack2() => new RBMid.UNKTRACK2
    {
      Data = Arr(() => Arr(() => new RBMid.UNKTRACK2.DATA
      {
        Tick1 = UInt(),
        Tick2 = UInt(),
        Unknown1 = Int(),
        Unknown2 = Int()
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
        Unknown = Short(),
        Unknown2 = Short(),
        Unknown3 = Byte(),
        Unknown4 = Int(),
        Unknown5 = Byte(),
        Unknown6 = Int()
      }))),
      Unknown = Int()
    };
    private RBMid.UNKTRACK3 ReadUnktrack3() => new RBMid.UNKTRACK3
    {
      Unknown = Arr(() => Arr(() => Arr(() => new RBMid.UNKTRACK3.EVENT
      {
        Tick1 = UInt(),
        Tick2 = UInt()
      })))
    };
    private RBMid.VOCALTRACK ReadVocalTrack() => new RBMid.VOCALTRACK
    {
      PhraseMarkers = Arr(ReadPhraseMarker),
      PhraseMarkers2 = Arr(ReadPhraseMarker),
      Notes = Arr(() => new RBMid.VOCALTRACK.VOCAL_NOTE
      {
        Type = Int(),
        MidiNote = Int(),
        MidiNote2 = Int(),
        StartMillis = Float(),
        StartTick = UInt(),
        Length = Float(),
        Unknown1 = Short(),
        Lyric = String(),
        Unknown2 = Arr(Byte, 9)
      }),
      Unknown1 = Arr(Int),
      Unknown2 = Arr(() => new RBMid.VOCALTRACK.UNKNOWN
      {
        Unknown1 = Float(),
        Unknown2 = Float()
      })
    };
    private RBMid.VOCALTRACK.PHRASE_MARKER ReadPhraseMarker() =>
      new RBMid.VOCALTRACK.PHRASE_MARKER
      {
        StartMillis = Float(),
        Length = Float(),
        Unknown1 = Int(),
        Unknown2 = Int(),
        Unknown3 = Int(),
        Unknown4 = Int(),
        Unknown5 = Int(),
        Unknown6 = Arr(Byte, 25)
      };
    private RBMid.HANDMAP ReadHandMap() => new RBMid.HANDMAP
    {
      Maps = Arr(() => new RBMid.HANDMAP.MAP
      {
        StartMillis = Float(),
        Map = Int()
      })
    };
    private RBMid.HANDPOS ReadHandPos() => new RBMid.HANDPOS
    {
      Events = Arr(() => new RBMid.HANDPOS.POS
      {
        StartMillis = Float(),
        EndMillis = Float(),
        Position = Int(),
        Unknown = Byte()
      })
    };
    private RBMid.UNKTRACK4 ReadUnktrack4() => new RBMid.UNKTRACK4
    {
      Unknown = Arr(Float)
    };
    private RBMid.UNKSTRUCT ReadUnkstruct() => new RBMid.UNKSTRUCT
    {
      StartTick = UInt(),
      EndTick = UInt(),
      Unknown = Int()
    };
    private RBMid.UNKMARKUP ReadUnkmarkup() => new RBMid.UNKMARKUP
    {
      StartTick = UInt(),
      EndTick = UInt(),
      Unknown = Arr(Int)
    };
    private uint midiTick;
    private string trackName;
    private string[] trackStrings;
    private MidiTrack ReadMidiTrack()
    {
      var unk = Byte();
      var unk2 = Int();
      var num_events = Int();
      midiTick = 0;
      trackName = "";
      var start = s.Position;
      s.Position += num_events * 8;
      trackStrings = Arr(String);
      var end = s.Position;
      s.Position = start;
      var msgs = new List<IMidiMessage>(Arr(ReadMessage, num_events));
      msgs.Add(new EndOfTrackEvent(0));
      s.Position = end;
      return new MidiTrack(msgs, midiTick, trackName);
    }
    private IMidiMessage ReadMessage()
    {
      var tick = UInt();
      var deltaTime = tick - midiTick;
      midiTick = tick;
      var kind = Byte();
      switch (kind)
      {
        // Midi Messages
        case 1:
          var tc = Byte();
          var channel = (byte)(tc & 0xF);
          var type = tc >> 4;
          var note = Byte();
          var velocity = Byte();
          switch (type)
          {
            case 8:
              return new NoteOffEvent(deltaTime, channel, note, velocity);
            case 9:
              return new NoteOnEvent(deltaTime, channel, note, velocity);
            default:
              throw new NotImplementedException($"Message type {type}");
          }
        // Tempo
        case 2:
          var tempo_msb = (uint)Byte();
          var tempo_lsb = UShort();
          return new TempoEvent(deltaTime, tempo_msb << 16 | tempo_lsb);
        // Time Signature
        case 4:
          var num = Byte();
          var denom = Byte();
          var denom_pow2 = (byte)Math.Log(denom, 2);
          Skip(1)();
          return new TimeSignature(deltaTime, num, denom_pow2, 24, 8);
        // Text
        case 8:
          var ttype = Byte();
          var txt = trackStrings[Short()];
          switch (ttype)
          {
            case 1:
              return new TextEvent(deltaTime, txt);
            case 3:
              trackName = txt;
              return new TrackName(deltaTime, txt);
            case 5:
              return new Lyric(deltaTime, txt);
            default:
              throw new NotImplementedException($"Text event {ttype} not implemented");
          }
        default:
          throw new NotImplementedException($"Message kind {kind} not yet known");
      }
    }
    private RBMid.TEMPO ReadTempo() => new RBMid.TEMPO
    {
      StartMillis = Float(),
      StartTick = UInt(),
      Tempo = Int()
    };
    private RBMid.TIMESIG ReadTimesig() => new RBMid.TIMESIG
    {
      Unknown = Int(),
      Tick = UInt(),
      Numerator = Short(),
      Denominator = Short()
    };
    private RBMid.BEAT ReadBeat() => new RBMid.BEAT
    {
      Tick = UInt(),
      Downbeat = Int() != 0
    };
  }
}
