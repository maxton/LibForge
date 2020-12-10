using LibForge.Util;
using MidiCS;
using MidiCS.Events;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace LibForge.Midi
{
  public class MidiFileResourceReader : ReaderBase<MidiFileResource>
  {
    public MidiFileResourceReader(Stream s) : base(s)
    {
    }

    public override MidiFileResource Read()
    {
      var r = new MidiFileResource();
      Read(r);
      return r;
    }

    public void Read(MidiFileResource r)
    {
      r.MidiSongResourceMagic = Check(Int(), 2);
      r.LastTrackFinalTick = UInt();
      r.MidiTracks = Arr(ReadMidiTrack);
      var finalTickOrRev = UInt();
      if (finalTickOrRev == 0x56455223) // '#REV'
      {
        r.FuserRevision = Int();
        r.FinalTick = UInt();
      } else
      {
        r.FinalTick = finalTickOrRev;
      }
      r.Measures = UInt();
      r.Unknown = FixedArr(UInt, 6);
      r.FinalTickMinusOne = Check(UInt(), r.FinalTick - 1);
      r.UnknownFloats = FixedArr(Float, 4);
      r.Tempos = Arr(ReadTempo);
      r.TimeSigs = Arr(ReadTimesig);
      r.Beats = Arr(ReadBeat);
      r.UnknownZero = Check(Int(), 0);
      if (r.FuserRevision == 2)
      {
        r.FuserRevision2 = Int();
        r.FuserData = Arr(ReadFuserData);
      }
      r.MidiTrackNames = CheckedArr(String, (uint)r.MidiTracks.Length);
    }

    private uint midiTick;
    private string trackName;
    private string[] trackStrings;
    private MidiTrack ReadMidiTrack()
    {
      var unk = Byte();
      var unk2 = Int();
      var num_events = UInt();
      midiTick = 0;
      trackName = "";
      var start = s.Position;
      s.Position += num_events * 8;
      trackStrings = Arr(String);
      var end = s.Position;
      s.Position = start;
      var msgs = new List<IMidiMessage>(FixedArr(ReadMessage, num_events));
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
            case 11: // seen in touchofgrey and others, assuming ctrl chg
              return new ControllerEvent(deltaTime, channel, note, velocity);
            case 12: // seen in foreplaylongtime, assuming prgmchg
              return new ProgramChgEvent(deltaTime, channel, note);
            case 13: // seen in huckleberrycrumble, assuming channel pressure
              return new ChannelPressureEvent(deltaTime, channel, note);
            case 14: // seen in theballadofirahayes, assuming pitch bend
              return new PitchBendEvent(deltaTime, channel, (ushort)(note | (velocity << 8)));
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
            case 2:
              return new CopyrightNotice(deltaTime, txt);
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
    private MidiFileResource.TEMPO ReadTempo() => new RBMid.TEMPO
    {
      StartMillis = Float(),
      StartTick = UInt(),
      Tempo = Int()
    };
    private MidiFileResource.TIMESIG ReadTimesig() => new RBMid.TIMESIG
    {
      Measure = Int(),
      Tick = UInt(),
      Numerator = Short(),
      Denominator = Short()
    };
    private MidiFileResource.BEAT ReadBeat() => new RBMid.BEAT
    {
      Tick = UInt(),
      Downbeat = Int() != 0
    };
    private MidiFileResource.FUSER_DATA ReadFuserData()
    {
      var unk_count = UInt();
      return new MidiFileResource.FUSER_DATA()
      {
        data = FixedArr(Byte, unk_count + 8)
      };
    }
  }
}
