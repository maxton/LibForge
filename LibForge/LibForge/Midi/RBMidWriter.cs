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
      Write(r.ProMarkers, WriteProCymbalMarkers);
      Write(r.LaneMarkers, WriteLaneMarkers);
      Write(r.TrillMarkers, WriteTrillMarkers);
      Write(r.DrumMixes, WriteDrumMixes);
      Write(r.GemTracks, WriteGemTracks);
      Write(r.OverdriveSoloSections, WriteSectionMarkers);
      Write(r.VocalTracks, WriteReadVocalTrack);
      Write(r.UnknownOne);
      Write(r.UnknownNegOne);
      Write(r.UnknownHundred);
      Write(r.Unknown4, WriteUnkstruct1);
      Write(r.Unknown5, WriteUnkstruct2);
      Write(r.Unknown6);
      Write(r.NumPlayableTracks);
      Write(r.FinalTick);
      if(r.Format == RBMid.FORMAT_RBVR)
      {
        Write(r.UnkVrTick);
      }
      Write(r.UnknownZeroByte);
      Write(r.PreviewStartMillis);
      Write(r.PreviewEndMillis);
      Write(r.GuitarHandmap, WriteHandMap);
      Write(r.GuitarLeftHandPos, WriteHandPos);
      Write(r.Unktrack, WriteUnktrack);
      // begin weirdness
      Write(r.MarkupSoloNotes1, WriteSoloNotes);
      Write(r.TwoTicks1, WriteTwoTicks);
      Write(r.MarkupChords1, WriteMarkupChord);
      Write(r.MarkupSoloNotes2, WriteSoloNotes);
      Write(r.MarkupSoloNotes3, WriteSoloNotes);
      Write(r.TwoTicks2, WriteTwoTicks);
      // end weirdness
      if(r.Format == RBMid.FORMAT_RBVR)
      {
        WriteVREvents(r.VREvents);
      }
      Write(r.UnknownTwo);
      Write(r.LastMarkupEventTick);
      Write(r.MidiTracks, WriteMidiTrack);
      Array.ForEach(r.UnknownInts, Write);
      Array.ForEach(r.UnknownFloats, Write);
      Write(r.Tempos, WriteTempo);
      Write(r.TimeSigs, WriteTimesig);
      Write(r.Beats, WriteBeat);
      Write(r.UnknownZero);
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
      Write(obj.Lanes, o => 
      {
        Write(o.Tick);
        Write(o.Lanes);
      });
      Write(obj.Fills, o =>
      {
        Write(o.StartTick);
        Write(o.EndTick);
        Write(o.IsBRE);
      });
    }
    private void WriteAnims(RBMid.ANIM obj)
    {
      Write(obj.TrackName);
      Write(obj.Unknown1);
      Write(obj.Unknown2);
      Write(obj.Events, o =>
      {
        Write(o.StartMillis);
        Write(o.StartTick);
        Write(o.LengthMillis);
        Write(o.LengthTicks);
        Write(o.KeyBitfield);
        Write(o.Unknown2);
        Write(o.Unknown3);
      });
      Write(obj.Unknown3);
    }
    private void WriteProCymbalMarkers(RBMid.CYMBALMARKER obj)
    {
      Write(obj.Markers, o =>
      {
        Write(o.Tick);
        Write((int)o.Flags);
      });
      Write(obj.Unknown1);
      Write(obj.Unknown2);
    }
    private void WriteLaneMarkers(RBMid.LANEMARKER obj)
    {
      Write(obj.Markers, diff => Write(diff, marker => 
      {
        Write(marker.StartTick);
        Write(marker.EndTick);
        Write((uint)marker.Flags);
      }));
    }
    private void WriteTrillMarkers(RBMid.GTRTRILLS obj)
    {
      Write(obj.Trills, x => Write(x, o =>
      {
        Write(o.StartTick);
        Write(o.EndTick);
        Write(o.LowFret);
        Write(o.HighFret);
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
          Write(x.LengthMillis);
          Write(x.LengthTicks);
          Write(x.Lanes);
          Write(x.IsHopo);
          Write(x.NoTail);
          Write(x.Unknown);
        });
      });
      Write(obj.Unknown);
    }
    private void WriteSectionMarkers(RBMid.SECTIONS obj)
    {
      Write(obj.Sections, a => Write(a, b => Write(b, x => {
        Write(x.StartTicks);
        Write(x.LengthTicks);
      })));
    }
    private void WriteReadVocalTrack(RBMid.VOCALTRACK obj)
    {
      Write(obj.PhraseMarkers, WritePhraseMarker);
      Write(obj.PhraseMarkers2, WritePhraseMarker);
      Write(obj.Notes, x =>
      {
        Write(x.PhraseIndex);
        Write(x.MidiNote);
        Write(x.MidiNote2);
        Write(x.StartMillis);
        Write(x.StartTick);
        Write(x.LengthMillis);
        Write(x.LengthTicks);
        Write(x.Lyric);
        Write(x.LastNoteInPhrase);
        Write(x.UnknownFalse);
        Write(x.Unpitched);
        Write(x.UnknownFalse2);
        Write(x.UnkFlag1);
        Write(x.Unknown);
        Write(x.Portamento);
        Write(x.Flag8);
        Write(x.Flag9);
      });
      Write(obj.Percussion, Write);
      Write(obj.Unknown2, x =>
      {
        Write(x.StartMillis);
        Write(x.EndMillis);
      });
    }
    private void WritePhraseMarker(RBMid.VOCALTRACK.PHRASE_MARKER obj)
    {
      Write(obj.StartMillis);
      Write(obj.Length);
      Write(obj.StartTicks);
      Write(obj.LengthTicks);
      Write(obj.StartNoteIdx);
      Write(obj.EndNoteIdx);
      Write(obj.UnkOne);
      Array.ForEach(obj.Unknown6, Write);
    }
    private void WriteUnkstruct1(RBMid.UNKSTRUCT1 obj)
    {
      Write(obj.Tick);
      Write(obj.FloatData);
    }
    private void WriteUnkstruct2(RBMid.UNKSTRUCT2 obj)
    {
      Write(obj.Unknown1);
      Write(obj.Unknown2);
      Write(obj.Unknown3);
      Write(obj.Unknown4);
    }
    private void WriteHandMap(RBMid.HANDMAP obj)
    {
      Write(obj.Maps, x =>
      {
        Write(x.StartTime);
        Write(x.Map);
      });
    }
    private void WriteHandPos(RBMid.HANDPOS obj)
    {
      Write(obj.Events, x =>
      {
        Write(x.StartTime);
        Write(x.Length);
        Write(x.Position);
        Write(x.Unknown);
      });
    }
    private void WriteUnktrack(RBMid.UNKTRACK obj)
    {
      Write(obj.Data, data =>
      {
        Write(data.FloatData);
        Write(data.IntData);
      });
    }
    private void WriteSoloNotes(RBMid.MARKUP_SOLO_NOTES obj)
    {
      Write(obj.StartTick);
      Write(obj.EndTick);
      Write(obj.NoteOffset);
    }
    private void WriteTwoTicks(RBMid.TWOTICKS obj)
    {
      Write(obj.Tick1);
      Write(obj.Tick2);
    }
    private void WriteMarkupChord(RBMid.MARKUPCHORD obj)
    {
      Write(obj.StartTick);
      Write(obj.EndTick);
      Write(obj.Pitches, Write);
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
    private void WriteTempo(RBMid.TEMPO obj)
    {
      Write(obj.StartMillis);
      Write(obj.StartTick);
      Write(obj.Tempo);
    }
    private void WriteTimesig(RBMid.TIMESIG obj)
    {
      Write(obj.Measure);
      Write(obj.Tick);
      Write(obj.Numerator);
      Write(obj.Denominator);
    }
    private void WriteBeat(RBMid.BEAT obj)
    {
      Write(obj.Tick);
      Write(obj.Downbeat ? 1 : 0);
    }
    private void WriteVREvents(RBMid.RBVREVENTS obj)
    {
      Write(obj.BeatmatchSections, e =>
      {
        Write(e.unk_zero);
        Write(e.beatmatch_section);
        Write(e.StartTick);
        Write(e.EndTick);
      });
      Write(obj.UnkStruct1, x =>
      {
        Write(x.Unk1);
        Write(x.StartPercentage);
        Write(x.EndPercentage);
        Write(x.StartTick);
        Write(x.EndTick);
        Write(x.Unk2);
      });
      Write(obj.UnkStruct2, x =>
      {
        Write(x.Unk);
        Write(x.Name);
        Write(x.Tick);
      });
      Write(obj.UnkStruct3, x =>
      {
        Write(x.Unk1);
        Write(x.exsandohs);
        Write(x.StartTick);
        Write(x.EndTick);
        Array.ForEach(x.Flags, Write);
        Write(x.Unk2);
      });
      Write(obj.UnkZero1);
      Write(obj.UnkStruct4, x =>
      {
        Write(x.Unk1);
        Write(x.Name);
        Write(x.ExsOhs, Write);
        Write(x.StartTick);
        Write(x.EndTick);
        Write(x.Unk2);
      });
      Write(obj.UnknownTicks, Write);
      Write(obj.UnkZero2);
      Write(obj.UnkStruct6, x =>
      {
        Write(x.Tick);
        Write(x.Unk);
      });
    }
  }
}
