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
  public class MidiFileResourceWriter : WriterBase<MidiFileResource>
  {
    public static void WriteStream(MidiFileResource r, Stream s)
    {
      new MidiFileResourceWriter(s).WriteStream(r);
    }
    private MidiFileResourceWriter(Stream s) : base(s) { }
    public override void WriteStream(MidiFileResource r)
    {
      Write(r.MidiSongResourceMagic);
      Write(r.LastTrackFinalTick);
      Write(r.MidiTracks, WriteMidiTrack);
      if (r.FuserRevision != null)
      {
        Write(r.FuserRevision.Value);
      }
      Write(r.FinalTick);
      Write(r.Measures);
      Array.ForEach(r.Unknown, Write);
      Write(r.FinalTickMinusOne);
      Array.ForEach(r.UnknownFloats, Write);
      Write(r.Tempos, WriteTempo);
      Write(r.TimeSigs, WriteTimesig);
      Write(r.Beats, WriteBeat);
      Write(r.UnknownZero);
      if (r.FuserRevision != null)
      {
        Write(r.FuserRevision2.Value);
        Write(r.FuserData, WriteFuserData);
      }
      Write(r.MidiTrackNames, Write);
    }
    private bool first_track = true;
    private List<string> track_strings;
    private void WriteMidiTrack(MidiTrack obj)
    {
      track_strings = new List<string>();

      Write((byte)1);
      if (first_track || obj.Name == "EVENTS")
        Write(-1);
      else
        Write(0);
      first_track = false;
      // Subtract 1 for the end-of-track event
      Write(obj.Messages.Count - 1);
      uint ticks = 0;
      foreach (var m in obj.Messages)
      {
        byte kind, d1, d2, d3;
        switch (m)
        {
          case NoteOffEvent e:
            kind = 1;
            d1 = (byte)(0x80 | e.Channel);
            d2 = e.Key;
            d3 = e.Velocity;
            break;
          case NoteOnEvent e:
            kind = 1;
            d1 = (byte)(0x90 | e.Channel);
            d2 = e.Key;
            d3 = e.Velocity;
            break;
          case ControllerEvent e:
            kind = 1;
            d1 = (byte)(0xB0 | e.Channel);
            d2 = e.Controller;
            d3 = e.Value;
            break;
          case ProgramChgEvent e:
            kind = 1;
            d1 = (byte)(0xC0 | e.Channel);
            d2 = e.Program;
            d3 = 0;
            break;
          case ChannelPressureEvent e:
            kind = 1;
            d1 = (byte)(0xD0 | e.Channel);
            d2 = e.Pressure;
            d3 = 0;
            break;
          case PitchBendEvent e:
            kind = 1;
            d1 = (byte)(0xE0 | e.Channel);
            d2 = (byte)(e.Bend & 0xFF);
            d3 = (byte)(e.Bend >> 8);
            break;
          case TempoEvent e:
            kind = 2;
            d1 = (byte)(e.MicrosPerQn >> 16);
            d2 = (byte)(e.MicrosPerQn & 0xFFU);
            d3 = (byte)((e.MicrosPerQn >> 8) & 0xFFU);
            break;
          case TimeSignature e:
            kind = 4;
            d1 = e.Numerator;
            d2 = (byte)(1 << e.Denominator);
            d3 = 0;
            break;
          case MetaTextEvent e:
            kind = 8;
            var idx = GetString(e.Text);
            d2 = (byte)(idx & 0xFF);
            d3 = (byte)(idx >> 8);
            switch (e)
            {
              case TextEvent x:
                d1 = 1;
                break;
              case TrackName x:
                d1 = 3;
                break;
              case Lyric x:
                d1 = 5;
                break;
              default:
                d1 = 1;
                break;
            }
            break;
          case EndOfTrackEvent e:
            continue;
          default:
            throw new Exception("Unknown Midi Message type");
        }
        ticks += m.DeltaTime;
        Write(ticks);
        Write(kind);
        Write(d1);
        Write(d2);
        Write(d3);
      }
      Write(track_strings.Count);
      track_strings.ForEach(Write);
    }
    private int GetString(string s)
    {
      var idx = track_strings.Count;
      track_strings.Add(s);
      return idx;
    }
    private void WriteTempo(MidiFileResource.TEMPO obj)
    {
      Write(obj.StartMillis);
      Write(obj.StartTick);
      Write(obj.Tempo);
    }
    private void WriteTimesig(MidiFileResource.TIMESIG obj)
    {
      Write(obj.Measure);
      Write(obj.Tick);
      Write(obj.Numerator);
      Write(obj.Denominator);
    }
    private void WriteBeat(MidiFileResource.BEAT obj)
    {
      Write(obj.Tick);
      Write(obj.Downbeat ? 1 : 0);
    }

    private void WriteFuserData(MidiFileResource.FUSER_DATA obj)
    {
      Write(obj.data.Length - 8);
      s.Write(obj.data, 0, obj.data.Length);
    }
  }
}
