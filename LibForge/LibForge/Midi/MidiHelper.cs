using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MidiCS;
using MidiCS.Events;

namespace LibForge.Midi
{
  public class MidiHelper
  {
    private MidiCS.MidiFile file;

    public List<MidiTrackProcessed> ProcessTracks(MidiCS.MidiFile file)
    {
      this.file = file;
      return file.Tracks.Select(processTrack).ToList();
    }

    private MidiCS.TimeSigTempoEvent GetTempo(uint tick)
    {
      var idx = 0;
      for (; idx < file.TempoTimeSigMap.Count; idx++)
      {
        if (file.TempoTimeSigMap[idx].Tick > tick)
        {
          break;
        }
      }
      idx--;
      return file.TempoTimeSigMap[idx];
    }
    private MidiTrackProcessed processTrack(MidiCS.MidiTrack track)
    {
      var items = new List<MidiItem>();
      var notesOn = new Dictionary<int, MidiNote>();
      var ticks = 0u;
      var finalTick = 0u;
      var finalTime = 0d;
      foreach (var msg in track.Messages)
      {
        ticks += msg.DeltaTime;
        var tempo = GetTempo(ticks);
        var time = tempo.Time + ((ticks - tempo.Tick) / 480.0) * (60 / tempo.BPM);
        switch (msg)
        {
          case NoteOnEvent e when e.Velocity != 0:
            {
              var note = new MidiNote
              {
                StartTicks = ticks,
                StartTime = time,
                Channel = e.Channel,
                Key = e.Key,
                Velocity = e.Velocity,
                CurrentTimeSig = tempo
              };
              notesOn[e.Channel << 8 | e.Key] = note;
              items.Add(note);
            }
            break;
          case NoteOnEvent e when e.Velocity == 0:
            {
              var note = notesOn[e.Channel << 8 | e.Key];
              note.Length = time - note.StartTime;
              note.LengthTicks = ticks - note.StartTicks;
            }
            break;
          case NoteOffEvent e:
            {
              var note = notesOn[e.Channel << 8 | e.Key];
              note.Length = time - note.StartTime;
              note.LengthTicks = ticks - note.StartTicks;
            }
            break;
          case TrackName e:
            // We already know the track name and we don't want this
            // grouped in with other text events
            break;
          case MetaTextEvent e:
            items.Add(new MidiText
            {
              StartTicks = ticks,
              StartTime = time,
              Text = e.Text,
              CurrentTimeSig = tempo
            });
            break;
          default:
            continue;
        }

        if (ticks > finalTick)
        {
          finalTick = ticks;
          finalTime = time;
        }
      }

      return new MidiTrackProcessed
      {
        Name = track.Name,
        LastTick = finalTick,
        LastTime = finalTime,
        Items = items
      };
    }

    public static List<IMidiMessage> ToAbsolute(List<IMidiMessage> messages)
    {
      var msgs = new List<IMidiMessage>();
      var abstime = 0u;
      foreach (var msg in messages)
      {
        abstime += msg.DeltaTime;
        switch (msg)
        {
          case NoteOnEvent e:
            if (e.Velocity == 0)
            {
              msgs.Add(new NoteOffEvent(abstime, e.Channel, e.Key, e.Velocity));
            }
            else
            {
              msgs.Add(new NoteOnEvent(abstime, e.Channel, e.Key, e.Velocity));
            }
            break;
          case NoteOffEvent e:
            msgs.Add(new NoteOffEvent(abstime, e.Channel, e.Key, e.Velocity));
            break;
          case NotePressureEvent e:
            msgs.Add(new NotePressureEvent(abstime, e.Channel, e.Key, e.Pressure));
            break;
          case ControllerEvent e:
            msgs.Add(new ControllerEvent(abstime, e.Channel, e.Controller, e.Value));
            break;
          case ProgramChgEvent e:
            msgs.Add(new ProgramChgEvent(abstime, e.Channel, e.Program));
            break;
          case ChannelPressureEvent e:
            msgs.Add(new ChannelPressureEvent(abstime, e.Channel, e.Pressure));
            break;
          case PitchBendEvent e:
            msgs.Add(new PitchBendEvent(abstime, e.Channel, e.Bend));
            break;
          case SysexEvent e:
            msgs.Add(new SysexEvent(abstime, e.Data));
            break;
          case SequenceNumber e:
            msgs.Add(new SequenceNumber(abstime, e.Number));
            break;
          case TextEvent e:
            msgs.Add(new TextEvent(abstime, e.Text));
            break;
          case CopyrightNotice e:
            msgs.Add(new CopyrightNotice(abstime, e.Text));
            break;
          case TrackName e:
            msgs.Add(new TrackName(abstime, e.Text));
            break;
          case InstrumentName e:
            msgs.Add(new InstrumentName(abstime, e.Text));
            break;
          case Lyric e:
            msgs.Add(new Lyric(abstime, e.Text));
            break;
          case Marker e:
            msgs.Add(new Marker(abstime, e.Text));
            break;
          case CuePoint e:
            msgs.Add(new CuePoint(abstime, e.Text));
            break;
          case ChannelPrefix e:
            msgs.Add(new ChannelPrefix(abstime, e.Channel));
            break;
          case EndOfTrackEvent e:
            msgs.Add(new EndOfTrackEvent(abstime));
            break;
          case TempoEvent e:
            msgs.Add(new TempoEvent(abstime, e.MicrosPerQn));
            break;
          case SmtpeOffset e:
            msgs.Add(new SmtpeOffset(abstime, e.Hours, e.Minutes, e.Seconds, e.Frames, e.FrameHundredths));
            break;
          case TimeSignature e:
            msgs.Add(new TimeSignature(abstime, e.Numerator, e.Denominator, e.ClocksPerTick, e.ThirtySecondNotesPer24Clocks));
            break;
          case KeySignature e:
            msgs.Add(new KeySignature(abstime, e.Sharps, e.Tonality));
            break;
          case SequencerSpecificEvent e:
            msgs.Add(new SequencerSpecificEvent(abstime, e.Data));
            break;
        }
      }
      return msgs;
    }

    public static List<IMidiMessage> ToRelative(List<IMidiMessage> messages)
    {
      var msgs = new List<IMidiMessage>();
      var lasttime = 0u;
      foreach (var msg in messages)
      {
        var deltatime = msg.DeltaTime - lasttime;
        lasttime = msg.DeltaTime;
        switch (msg)
        {
          case NoteOnEvent e:
            msgs.Add(new NoteOnEvent(deltatime, e.Channel, e.Key, e.Velocity));
            break;
          case NoteOffEvent e:
            msgs.Add(new NoteOffEvent(deltatime, e.Channel, e.Key, e.Velocity));
            break;
          case NotePressureEvent e:
            msgs.Add(new NotePressureEvent(deltatime, e.Channel, e.Key, e.Pressure));
            break;
          case ControllerEvent e:
            msgs.Add(new ControllerEvent(deltatime, e.Channel, e.Controller, e.Value));
            break;
          case ProgramChgEvent e:
            msgs.Add(new ProgramChgEvent(deltatime, e.Channel, e.Program));
            break;
          case ChannelPressureEvent e:
            msgs.Add(new ChannelPressureEvent(deltatime, e.Channel, e.Pressure));
            break;
          case PitchBendEvent e:
            msgs.Add(new PitchBendEvent(deltatime, e.Channel, e.Bend));
            break;
          case SysexEvent e:
            msgs.Add(new SysexEvent(deltatime, e.Data));
            break;
          case SequenceNumber e:
            msgs.Add(new SequenceNumber(deltatime, e.Number));
            break;
          case TextEvent e:
            msgs.Add(new TextEvent(deltatime, e.Text));
            break;
          case CopyrightNotice e:
            msgs.Add(new CopyrightNotice(deltatime, e.Text));
            break;
          case TrackName e:
            msgs.Add(new TrackName(deltatime, e.Text));
            break;
          case InstrumentName e:
            msgs.Add(new InstrumentName(deltatime, e.Text));
            break;
          case Lyric e:
            msgs.Add(new Lyric(deltatime, e.Text));
            break;
          case Marker e:
            msgs.Add(new Marker(deltatime, e.Text));
            break;
          case CuePoint e:
            msgs.Add(new CuePoint(deltatime, e.Text));
            break;
          case ChannelPrefix e:
            msgs.Add(new ChannelPrefix(deltatime, e.Channel));
            break;
          case EndOfTrackEvent e:
            msgs.Add(new EndOfTrackEvent(deltatime));
            break;
          case TempoEvent e:
            msgs.Add(new TempoEvent(deltatime, e.MicrosPerQn));
            break;
          case SmtpeOffset e:
            msgs.Add(new SmtpeOffset(deltatime, e.Hours, e.Minutes, e.Seconds, e.Frames, e.FrameHundredths));
            break;
          case TimeSignature e:
            msgs.Add(new TimeSignature(deltatime, e.Numerator, e.Denominator, e.ClocksPerTick, e.ThirtySecondNotesPer24Clocks));
            break;
          case KeySignature e:
            msgs.Add(new KeySignature(deltatime, e.Sharps, e.Tonality));
            break;
          case SequencerSpecificEvent e:
            msgs.Add(new SequencerSpecificEvent(deltatime, e.Data));
            break;
        }
      }
      return msgs;
    }

  }

  public class MidiTrackProcessed
  {
    public string Name;
    public uint LastTick;
    public double LastTime;
    public List<MidiItem> Items;
  }

  public abstract class MidiItem
  {
    public double StartTime;
    public uint StartTicks;
    public uint Measure;
    public MidiCS.TimeSigTempoEvent CurrentTimeSig;
  }

  public class MidiNote : MidiItem
  {
    public double Length;
    public uint LengthTicks;
    public byte Channel;
    public byte Key;
    public byte Velocity;
  }
  
  public class MidiText : MidiItem
  {
    public string Text;
  }
}
