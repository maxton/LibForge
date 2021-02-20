using System;
using System.Collections.Generic;
using System.IO;
using LibForge.Util;
using MidiCS;
using MidiCS.Events;

namespace LibForge.Midi
{
  public class RBMidWriter : WriterBase<RBMid>
  {
    public static void WriteStream(RBMid r, Stream s)
    {
      new RBMidWriter(s).WriteStream(r);
    }
    private RBMidWriter(Stream s) : base(s) { }
    public override void WriteStream(RBMid r)
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
      Write(r.VocalRange, WriteVocalRange);
      Write(r.HopoThreshold);
      Write(r.NumPlayableTracks);
      Write(r.FinalEventTick);
      if(r.Format == RBMid.FORMAT_RBVR)
      {
        Write(r.UnkVrTick);
      }
      Write(r.UnknownZeroByte);
      Write(r.PreviewStartMillis);
      Write(r.PreviewEndMillis);
      Write(r.HandMaps, x => Write(x, WriteMap));
      Write(r.GuitarLeftHandPos, x => Write(x, WriteHandPos));
      Write(r.StrumMaps, x => Write(x, WriteMap));
      // begin weirdness
      Write(r.MarkupSoloNotes1, WriteSoloNotes);
      Write(r.MarkupLoop1, WriteTwoTicks);
      Write(r.MarkupChords1, WriteMarkupChord);
      Write(r.MarkupSoloNotes2, WriteSoloNotes);
      Write(r.MarkupSoloNotes3, WriteSoloNotes);
      Write(r.MarkupLoop2, WriteTwoTicks);
      // end weirdness
      if(r.Format == RBMid.FORMAT_RBVR)
      {
        WriteVREvents(r.VREvents);
      }
      MidiFileResourceWriter.WriteStream(r, s);
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
    private void WriteProCymbalMarkers(RBMid.TOMMARKER obj)
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
        Write((uint)marker.Lanes);
      }));
    }
    private void WriteTrillMarkers(RBMid.GTRTRILLS obj)
    {
      Write(obj.Trills, x => Write(x, o =>
      {
        Write(o.StartTick);
        Write(o.EndTick);
        Write(o.FirstFret);
        Write(o.SecondFret);
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
          Write(x.ProCymbal);
        });
      });
      Write(obj.HopoThreshold);
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
      Write(obj.FakePhraseMarkers, WritePhraseMarker);
      Write(obj.AuthoredPhraseMarkers, WritePhraseMarker);
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
        Write(x.False1);
        Write(x.Unpitched);
        Write(x.UnpitchedGenerous);
        Write(x.RangeDivider);
        Write(x.PhraseFlags);
        Write(x.Portamento);
        Write(x.LyricShift);
        Write(x.ShowLyric);
      });
      Write(obj.Percussion, Write);
      Write(obj.FreestyleRegions, x =>
      {
        Write(x.StartMillis);
        Write(x.EndMillis);
      });
    }
    private void WritePhraseMarker(RBMid.VOCALTRACK.PHRASE_MARKER obj)
    {
      Write(obj.StartMillis);
      Write(obj.LengthMillis);
      Write(obj.StartTicks);
      Write(obj.LengthTicks);
      Write(obj.StartNoteIdx);
      Write(obj.EndNoteIdx);
      Write(obj.HasPitchedVox);
      Write(obj.HasUnpitchedVox);
      s.Position += 9;
      Write(obj.LowNote);
      Write(obj.HighNote);
      Write(obj.PhraseFlags);
      Write(obj.PercussionSection);
      s.Position += 8;
    }
    private void WriteUnkstruct1(RBMid.UNKSTRUCT1 obj)
    {
      Write(obj.Tick);
      Write(obj.FloatData);
    }
    private void WriteVocalRange(RBMid.VocalTrackRange obj)
    {
      Write(obj.StartMillis);
      Write(obj.StartTicks);
      Write(obj.LowNote);
      Write(obj.HighNote);
    }
    private void WriteMap(RBMid.MAP obj)
    {
      Write(obj.StartTime);
      Write(obj.Map);
    }
    private void WriteHandPos(RBMid.HANDPOS obj)
    {
      Write(obj.StartTime);
      Write(obj.Length);
      Write(obj.Position);
      Write(obj.Unknown);
    }
    private void WriteSoloNotes(RBMid.MARKUP_SOLO_NOTES obj)
    {
      Write(obj.StartTick);
      Write(obj.EndTick);
      Write(obj.NoteOffset);
    }
    private void WriteTwoTicks(RBMid.TWOTICKS obj)
    {
      Write(obj.StartTick);
      Write(obj.EndTick);
    }
    private void WriteMarkupChord(RBMid.MARKUPCHORD obj)
    {
      Write(obj.StartTick);
      Write(obj.EndTick);
      Write(obj.Pitches, Write);
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
      Write(obj.UnkStruct4, x =>
      {
        Write(x.Unk);
        Write(x.Name);
        Write(x.StartTick);
        Write(x.EndTick);
      });
      Write(obj.UnkStruct5, x =>
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
