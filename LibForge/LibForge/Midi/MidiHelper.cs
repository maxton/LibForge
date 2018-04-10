using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
      foreach (var msg in track.Messages)
      {
        ticks += msg.DeltaTime;
        if (ticks > finalTick) finalTick = ticks;
        var tempo = GetTempo(ticks);
        var time = tempo.Time + ((ticks - tempo.Tick) / 480.0) * (60 / tempo.BPM);
        switch (msg)
        {
          case NoteOnEvent e:
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
        }
      }

      return new MidiTrackProcessed
      {
        Name = track.Name,
        LastTick = finalTick,
        Items = items
      };
    }
  }

  public class MidiTrackProcessed
  {
    public string Name;
    public uint LastTick;
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
