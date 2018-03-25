using System;
using System.Collections.Generic;
using System.IO;
using LibForge.Util;
using MidiCS;
using MidiCS.Events;

namespace LibForge.Midi
{
  public class RBMidWriter : WriterBase
  {
    public static void WriteStream(RBMid r, Stream s)
    {
      new RBMidWriter(s).WriteStream(r);
    }
    private RBMidWriter(Stream s) : base(s) { }
    private void WriteStream(RBMid r)
    {
      Write(r.Format);
      Write(r.Lyrics, WriteLyrics);
      Write(r.DrumFills, WriteDrumFills);
      Write(r.Anims, WriteAnims);
      Write(r.ProMarkers, WriteMarkers);
      Write(r.UnkTrack, WriteUnktrack);
      Write(r.UnkTrack2, WriteUnktrack2);
      Write(r.DrumMixes, WriteDrumMixes);
      Write(r.GemTracks, WriteGemTracks);
      Write(r.UnkTrack3, WriteUnktrack3);
      Write(r.VocalTracks, WriteReadVocalTrack);
      Write(r.Unknown1);
      Write(r.Unknown2);
      Write(r.Unknown3);
      Write(r.Unknown4);
      Write(r.Unknown5);
      Write(r.Unknown6);
      Write(r.Unknown7);
      Write(r.Unknown8);
      Write(r.Unknown9);
      Write(r.Unknown10);
      Write(r.Unknown11);
      Write(r.Unknown12);
      Write(r.Unknown13);
      Write(r.PreviewStartMillis);
      Write(r.PreviewEndMillis);
      Write(r.GuitarHandmap, WriteHandMap);
      Write(r.GuitarLeftHandPos, WriteHandPos);
      Write(r.Unktrack4, WriteUnktrack4);
      Write(r.Unkstruct, WriteUnkstruct);
      Write(r.Unkstruct2, WriteUnkstruct);
      if (r.Unkstruct2.Length > 0) Write(0);
      Write(r.UnkMarkup, WriteUnkmarkup);
      Write(r.UnkMarkup2, WriteUnkmarkup);
      if (r.UnkMarkup2.Length > 0) Write(0);
      Write(r.Unkstruct3, WriteUnkstruct);
      Write(r.Unknown16);
      Write(r.Unknown17);
      Write(r.Unknown18);
      Write(r.MidiTracks, WriteMidiTrack);
      Array.ForEach(r.Unknown19, Write);
      Write(r.Tempos, WriteTempo);
      Write(r.TimeSigs, WriteTimesig);
      Write(r.Beats, WriteBeat);
      Write(r.Unknown20);
      Write(r.MidiTrackNames, Write);
    }

    private void WriteTickText(RBMid.TICKTEXT obj)
    {
      Write(obj.Tick);
      Write(obj.Text);
    }
    private void WriteLyrics(RBMid.LYRICS obj)
    {
      Write(obj.TrackName);
      Write(obj.Lyrics, WriteTickText);
      Write(obj.Unknown1);
      Write(obj.Unknown2);
      Write(obj.Unknown3);
    }
    private void WriteDrumFills(RBMid.DRUMFILLS obj)
    {
      Write(obj.Unknown, o => 
      {
        Write(o.Tick);
        Write(o.Unknown);
      });
      Write(obj.Fills, o =>
      {
        Write(o.StartTick);
        Write(o.EndTick);
        Write(o.Unknown);
      });
    }
    private void WriteAnims(RBMid.ANIM obj)
    {
      Write(obj.TrackName);
      Write(obj.Unknown1);
      Write(obj.Unknown2);
      Write(obj.Events, o =>
      {
        Write(o.Time);
        Write(o.Tick);
        Write(o.Unknown1);
        Write(o.KeyBitfield);
        Write(o.Unknown2);
        Write(o.Unknown3);
      });
      Write(obj.Unknown3);
    }
    private void WriteMarkers(RBMid.MARKERS obj)
    {
      Write(obj.Markers, o =>
      {
        Write(o.Tick);
        Write((int)o.Flags);
      });
      Write(obj.Unknown1);
      Write(obj.Unknown2);
    }
    // TODO: Fix when we actually define this
    private void WriteUnktrack(RBMid.UNKTRACK obj)
    {
      Write(0);
    }
    private void WriteUnktrack2(RBMid.UNKTRACK2 obj)
    {
      Write(obj.Data, x => Write(x, o =>
      {
        Write(o.Tick1);
        Write(o.Tick2);
        Write(o.Unknown1);
        Write(o.Unknown2);
      }));
    }
    private void WriteDrumMixes(RBMid.DRUMMIXES obj)
    {
      Write(obj.Mixes, o => Write(o, WriteTickText));
    }
    private void WriteGemTracks(RBMid.GEMTRACK obj)
    {
      Write(obj.Gems, gems =>
      {
        Write(0xAA);
        Write(gems, x =>
        {
          Write(x.StartMillis);
          Write(x.StartTicks);
          Write(x.Unknown);
          Write(x.Unknown2);
          Write(x.Unknown3);
          Write(x.Unknown4);
          Write(x.Unknown5);
          Write(x.Unknown6);
        });
      });
      Write(obj.Unknown);
    }
    private void WriteUnktrack3(RBMid.UNKTRACK3 obj)
    {
      Write(obj.Unknown, a => Write(a, b => Write(b, x => {
        Write(x.Tick1);
        Write(x.Tick2);
      })));
    }
    private void WriteReadVocalTrack(RBMid.VOCALTRACK obj)
    {
      Write(obj.PhraseMarkers, WritePhraseMarker);
      Write(obj.PhraseMarkers2, WritePhraseMarker);
      Write(obj.Notes, x =>
      {
        Write(x.Type);
        Write(x.MidiNote);
        Write(x.MidiNote2);
        Write(x.StartMillis);
        Write(x.StartTick);
        Write(x.Length);
        Write(x.Unknown1);
        Write(x.Lyric);
        Array.ForEach(x.Unknown2, Write);
      });
      Write(obj.Unknown1, Write);
      Write(obj.Unknown2, x =>
      {
        Write(x.Unknown1);
        Write(x.Unknown2);
      });
    }
    private void WritePhraseMarker(RBMid.VOCALTRACK.PHRASE_MARKER obj)
    {
      Write(obj.StartMillis);
      Write(obj.Length);
      Write(obj.Unknown1);
      Write(obj.Unknown2);
      Write(obj.Unknown3);
      Write(obj.Unknown4);
      Write(obj.Unknown5);
      Array.ForEach(obj.Unknown6, Write);
    }
    private void WriteHandMap(RBMid.HANDMAP obj)
    {
      Write(obj.Maps, x =>
      {
        Write(x.StartMillis);
        Write(x.Map);
      });
    }
    private void WriteHandPos(RBMid.HANDPOS obj)
    {
      Write(obj.Events, x =>
      {
        Write(x.StartMillis);
        Write(x.EndMillis);
        Write(x.Position);
        Write(x.Unknown);
      });
    }
    private void WriteUnktrack4(RBMid.UNKTRACK4 obj)
    {
      Write(obj.Unknown, Write);
    }
    private void WriteUnkstruct(RBMid.UNKSTRUCT obj)
    {
      Write(obj.StartTick);
      Write(obj.EndTick);
      Write(obj.Unknown);
    }
    private void WriteUnkmarkup(RBMid.UNKMARKUP obj)
    {
      Write(obj.StartTick);
      Write(obj.EndTick);
      Write(obj.Unknown, Write);
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
    private void WriteTempo(RBMid.TEMPO obj)
    {
      Write(obj.StartMillis);
      Write(obj.StartTick);
      Write(obj.Tempo);
    }
    private void WriteTimesig(RBMid.TIMESIG obj)
    {
      Write(obj.Unknown);
      Write(obj.Tick);
      Write(obj.Numerator);
      Write(obj.Denominator);
    }
    private void WriteBeat(RBMid.BEAT obj)
    {
      Write(obj.Tick);
      Write(obj.Downbeat ? 1 : 0);
    }
  }
}
