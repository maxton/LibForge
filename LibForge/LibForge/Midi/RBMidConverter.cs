using System;
using System.Collections.Generic;
using MidiCS;
using MidiCS.Events;
using System.Linq;

namespace LibForge.Midi
{
  public class RBMidConverter
  {
    public static RBMid ToRBMid(MidiFile mf)
    {
      return (new MidiConverter(mf)).ToRBMid();
    }
    public static MidiFile ToMid(RBMid m)
    {
      return new MidiFile(MidiFormat.MultiTrack, new List<MidiTrack>(m.MidiTracks), 480);
    }

    private class MidiConverter
    {
      private MidiFile mf;
      private RBMid rb;

      private List<RBMid.LYRICS> Lyrics;
      private List<RBMid.DRUMFILLS> DrumFills;
      private List<RBMid.ANIM> Anims;
      private List<RBMid.CYMBALMARKER> ProMarkers;
      private List<RBMid.LANEMARKER> LaneMarkers;
      private List<RBMid.GTRTRILLS> TrillMarkers;
      private List<RBMid.DRUMMIXES> DrumMixes;
      private List<RBMid.GEMTRACK> GemTracks;
      private List<RBMid.SECTIONS> OverdriveSoloSections;
      private List<RBMid.VOCALTRACK> VocalTracks;
      private List<RBMid.UNKSTRUCT1> Unknown4;
      private List<RBMid.UNKSTRUCT2> Unknown5;
      private List<RBMid.HANDMAP> HandMap;
      private List<RBMid.HANDPOS> HandPos;
      private List<RBMid.UNKTRACK> Unktrack;
      private List<RBMid.MARKUP_SOLO_NOTES> MarkupSoloNotes1, MarkupSoloNotes2, MarkupSoloNotes3;
      private List<RBMid.TWOTICKS> TwoTicks1, TwoTicks2;
      private List<RBMid.MARKUPCHORD> MarkupChords1;
      private List<RBMid.TEMPO> Tempos;
      private List<RBMid.TIMESIG> TimeSigs;
      private List<RBMid.BEAT> Beats;
      private List<string> MidiTrackNames;

      public MidiConverter(MidiFile mf)
      {
        this.mf = mf;
        trackHandlers = new Dictionary<string, Action<MidiTrack>>
        {
          {"PART DRUMS", HandleDrumTrk },
          {"PART BASS", HandleBassTrk },
          {"PART GUITAR", HandleGtrTrk },
          {"PART REAL_KEYS_X", HandleRealKeysXTrk },
          {"PART KEYS_ANIM_RH", HandleKeysAnimTrk },
          {"PART KEYS_ANIM_LH", HandleKeysAnimTrk },
          {"PART VOCALS", HandleVocalsTrk },
          {"HARM1", HandleHarmTrk },
          {"HARM2", HandleHarmTrk },
          {"HARM3", HandleHarmTrk },
          {"EVENTS", HandleEventsTrk },
          {"BEAT", HandleBeatTrk },
          {"MARKUP", HandleMarkupTrk },
          {"VENUE", HandleVenueTrk }
        };
      }

      public RBMid ToRBMid()
      {
        Lyrics = new List<RBMid.LYRICS>();
        DrumFills = new List<RBMid.DRUMFILLS>();
        Anims = new List<RBMid.ANIM>();
        ProMarkers = new List<RBMid.CYMBALMARKER>();
        LaneMarkers = new List<RBMid.LANEMARKER>();
        TrillMarkers = new List<RBMid.GTRTRILLS>();
        DrumMixes = new List<RBMid.DRUMMIXES>();
        GemTracks = new List<RBMid.GEMTRACK>();
        OverdriveSoloSections = new List<RBMid.SECTIONS>();
        VocalTracks = new List<RBMid.VOCALTRACK>();
        Unknown4 = new List<RBMid.UNKSTRUCT1>();
        Unknown5 = new List<RBMid.UNKSTRUCT2>();
        HandMap = new List<RBMid.HANDMAP>();
        HandPos = new List<RBMid.HANDPOS>();
        Unktrack = new List<RBMid.UNKTRACK>();
        MarkupSoloNotes1 = new List<RBMid.MARKUP_SOLO_NOTES>();
        TwoTicks1 = new List<RBMid.TWOTICKS>();
        MarkupChords1 = new List<RBMid.MARKUPCHORD>();
        MarkupSoloNotes2 = new List<RBMid.MARKUP_SOLO_NOTES>();
        MarkupSoloNotes3 = new List<RBMid.MARKUP_SOLO_NOTES>();
        TwoTicks2 = new List<RBMid.TWOTICKS>();
        Tempos = new List<RBMid.TEMPO>();
        TimeSigs = new List<RBMid.TIMESIG>();
        Beats = new List<RBMid.BEAT>();
        MidiTrackNames = new List<string>();
        mf.Tracks.ForEach(ProcessTrack);
        var lastTimeSig = mf.TempoTimeSigMap[0];
        var measure = 0;
        foreach (var tempo in mf.TempoTimeSigMap)
        {
          Tempos.Add(new RBMid.TEMPO
          {
            StartTick = (uint)tempo.Tick,
            StartMillis = (float)(tempo.Time * 1000.0),
            Tempo = (int)(60_000_000 / tempo.BPM)
          });
          if(tempo.NewTimeSig)
          {
            if (tempo.Tick > 0)
            {
              var elapsed = tempo.Tick - lastTimeSig.Tick;
              var ticksPerBeat = (480 * 4) / lastTimeSig.Denominator;
              measure += (int)(elapsed / ticksPerBeat / lastTimeSig.Numerator);
            }
            TimeSigs.Add(new RBMid.TIMESIG
            {
              Numerator = tempo.Numerator,
              Denominator = tempo.Denominator,
              Tick = (uint)tempo.Tick,
              Measure = measure
            });
            lastTimeSig = tempo;
          }
        }
        rb = new RBMid
        {
          Format = 0x10,
          Lyrics = Lyrics.ToArray(),
          DrumFills = DrumFills.ToArray(),
          Anims = Anims.ToArray(),
          ProMarkers = ProMarkers.ToArray(),
          LaneMarkers = LaneMarkers.ToArray(),
          TrillMarkers = TrillMarkers.ToArray(),
          DrumMixes = DrumMixes.ToArray(),
          GemTracks = GemTracks.ToArray(),
          OverdriveSoloSections = OverdriveSoloSections.ToArray(),
          VocalTracks = VocalTracks.ToArray(),
          Unknown4 = Unknown4.ToArray(),
          Unknown5 = Unknown5.ToArray(),
          GuitarHandmap = HandMap.ToArray(),
          GuitarLeftHandPos = HandPos.ToArray(),
          Unktrack = Unktrack.ToArray(),
          MarkupSoloNotes1 = MarkupSoloNotes1.ToArray(),
          TwoTicks1 = TwoTicks1.ToArray(),
          MarkupChords1 = MarkupChords1.ToArray(),
          MarkupSoloNotes2 = MarkupSoloNotes2.ToArray(),
          MarkupSoloNotes3 = MarkupSoloNotes3.ToArray(),
          TwoTicks2 = TwoTicks2.ToArray(),
          MidiTracks = mf.Tracks.ToArray(),
          Tempos = Tempos.ToArray(),
          TimeSigs = TimeSigs.ToArray(),
          Beats = Beats.ToArray(),
          UnknownInts = new int[9],
          UnknownFloats = new float[4],
          MidiTrackNames = MidiTrackNames.ToArray(),
          PreviewEndMillis = 1.0f,
          UnknownTwo = 2,
          NumPlayableTracks = (uint)Lyrics.Count,
        };
        return rb;
      }
      private Dictionary<string, Action<MidiTrack>> trackHandlers;
      private MidiTrack currentTrack;

      private void ProcessTrack(MidiTrack track)
      {
        currentTrack = track;
        MidiTrackNames.Add(track.Name);
        if (MidiTrackNames.Count == 1)
          HandleFirstTrack(track);
        else if (trackHandlers.ContainsKey(track.Name))
          trackHandlers[track.Name](track);
      }

      private void HandleFirstTrack(MidiTrack track)
      {
        
      }

      private TimeSigTempoEvent GetTempo(uint tick)
      {
        var idx = 0;
        for(; idx < mf.TempoTimeSigMap.Count; idx++)
        {
          if(mf.TempoTimeSigMap[idx].Tick > tick)
          {
            break;
          }
        }
        idx--;
        return mf.TempoTimeSigMap[idx];
      }
      private void EachMessage(MidiTrack track, Action<IMidiMessage, uint, float> action)
      {
        var ticks = 0U;
        var tempoIdx = 0;
        foreach(var msg in track.Messages)
        {
          ticks += msg.DeltaTime;
          var tempo = GetTempo(ticks);
          var time = tempo.Time + ((ticks - tempo.Tick) / 480.0) * (60 / tempo.BPM);
          action(msg, ticks, (float)time);
        }
      }

      const byte Roll2 = 127;
      const byte Roll1 = 126;
      const byte DrumFillMarkerStart = 120;
      const byte OverdriveMarker = 116;
      const byte ProGreen = 112;
      const byte ProBlue = 111;
      const byte ProYellow = 110;
      const byte SoloMarker = 103;
      const byte ExpertStart = 96;
      const byte HardStart = 84;
      const byte MediumStart = 72;
      const byte EasyStart = 60;
      const byte DrumAnimEnd = 51;
      const byte DrumAnimStart = 24;
      private void HandleDrumTrk(MidiTrack track)
      {
        var drumfills = new List<RBMid.DRUMFILLS.DRUMFILL>();
        var fill = new RBMid.DRUMFILLS.DRUMFILL();
        var fills_unk = new List<RBMid.DRUMFILLS.FILLS_UNK>();
        var cymbal_markers = new SortedDictionary<uint, RBMid.CYMBALMARKER.MARKER>();
        cymbal_markers[0] = default;
        RBMid.CYMBALMARKER.MARKER.FLAGS GetFlag(byte key)
        {
          switch (key)
          {
            case ProBlue:
              return RBMid.CYMBALMARKER.MARKER.FLAGS.ProBlue;
            case ProYellow:
              return RBMid.CYMBALMARKER.MARKER.FLAGS.ProYellow;
            case ProGreen:
              return RBMid.CYMBALMARKER.MARKER.FLAGS.ProGreen;
          }
          return 0;
        }
        EachMessage(track, (msg, ticks, time) =>
        {
          switch (msg)
          {
            case NoteOnEvent e:
              if (e.Key == DrumFillMarkerStart)
              {
                fills_unk.Add(new RBMid.DRUMFILLS.FILLS_UNK
                {
                  Tick = ticks,
                  Unknown = 31
                });
                fill.StartTick = ticks;
              }
              else if (e.Key == OverdriveMarker)
              {

              }
              else if (e.Key == ProYellow || e.Key == ProBlue || e.Key == ProGreen)
              {
                var marker = cymbal_markers.ContainsKey(ticks) ? cymbal_markers[ticks] : new RBMid.CYMBALMARKER.MARKER { Tick = ticks };
                marker.Flags |= GetFlag(e.Key);
                cymbal_markers[ticks] = marker;
              }
              break;
            case NoteOffEvent e:
              if (e.Key == DrumFillMarkerStart)
              {
                drumfills.Add(new RBMid.DRUMFILLS.DRUMFILL
                {
                  StartTick = fill.StartTick,
                  EndTick = ticks,
                  IsBRE = 0
                });
              }
              else if (e.Key == ProYellow || e.Key == ProBlue || e.Key == ProGreen)
              {
                var marker = cymbal_markers.ContainsKey(ticks) ? cymbal_markers[ticks] : new RBMid.CYMBALMARKER.MARKER { Tick = ticks };
                marker.Flags &= ~GetFlag(e.Key);
                cymbal_markers[ticks] = marker;
              }
              break;
          }
        });
        Lyrics.Add(new RBMid.LYRICS
        {
          TrackName = track.Name,
          Unknown1 = 0,
          Unknown2 = 0,
          Unknown3 = 1
        });
        DrumFills.Add(new RBMid.DRUMFILLS
        {
          Fills = drumfills.ToArray(),
          Unknown = fills_unk.ToArray()
        });
        ProMarkers.Add(new RBMid.CYMBALMARKER
        {
          Markers = cymbal_markers.Values.ToArray()
        });
        LaneMarkers.Add(new RBMid.LANEMARKER
        {
          
        });
        TrillMarkers.Add(new RBMid.GTRTRILLS
        {

        });
        DrumMixes.Add(new RBMid.DRUMMIXES
        {

        });
        GemTracks.Add(new RBMid.GEMTRACK
        {

        });
        OverdriveSoloSections.Add(new RBMid.SECTIONS
        {

        });
        HandMap.Add(new RBMid.HANDMAP
        {

        });
        HandPos.Add(new RBMid.HANDPOS
        {

        });
        Unktrack.Add(new RBMid.UNKTRACK
        {

        });
      }

      private void HandleBassTrk(MidiTrack track)
      {
        EachMessage(track, (msg, ticks, time) =>
        {
          switch (msg)
          {

          }
        });

        Lyrics.Add(new RBMid.LYRICS
        {
          TrackName = track.Name,
          Unknown1 = 2,
          Unknown2 = 2,
          Unknown3 = 0
        });
        DrumFills.Add(new RBMid.DRUMFILLS
        {

        });
        ProMarkers.Add(new RBMid.CYMBALMARKER
        {

        });
        LaneMarkers.Add(new RBMid.LANEMARKER
        {

        });
        TrillMarkers.Add(new RBMid.GTRTRILLS
        {

        });
        DrumMixes.Add(new RBMid.DRUMMIXES
        {

        });
        GemTracks.Add(new RBMid.GEMTRACK
        {

        });
        OverdriveSoloSections.Add(new RBMid.SECTIONS
        {

        });
        HandMap.Add(new RBMid.HANDMAP
        {

        });
        HandPos.Add(new RBMid.HANDPOS
        {

        });
        Unktrack.Add(new RBMid.UNKTRACK
        {

        });
      }

      private void HandleGtrTrk(MidiTrack track)
      {
        EachMessage(track, (msg, ticks, time) =>
        {
          switch (msg)
          {

          }
        });

        Lyrics.Add(new RBMid.LYRICS
        {
          TrackName = track.Name,
          Unknown1 = 1,
          Unknown2 = 1,
          Unknown3 = 0
        });
        DrumFills.Add(new RBMid.DRUMFILLS
        {
        });
        ProMarkers.Add(new RBMid.CYMBALMARKER
        {

        });
        LaneMarkers.Add(new RBMid.LANEMARKER
        {

        });
        TrillMarkers.Add(new RBMid.GTRTRILLS
        {

        });
        DrumMixes.Add(new RBMid.DRUMMIXES
        {

        });
        GemTracks.Add(new RBMid.GEMTRACK
        {

        });
        OverdriveSoloSections.Add(new RBMid.SECTIONS
        {

        });
        HandMap.Add(new RBMid.HANDMAP
        {

        });
        HandPos.Add(new RBMid.HANDPOS
        {

        });
        Unktrack.Add(new RBMid.UNKTRACK
        {

        });
      }

      private void HandleRealKeysXTrk(MidiTrack track)
      {
        EachMessage(track, (msg, ticks, time) =>
        {
          switch (msg)
          {

          }
        });

        Lyrics.Add(new RBMid.LYRICS
        {
          TrackName = track.Name,
          Unknown1 = 4,
          Unknown2 = 5,
          Unknown3 = 0
        });
        DrumFills.Add(new RBMid.DRUMFILLS
        {
        });
        ProMarkers.Add(new RBMid.CYMBALMARKER
        {

        });
        LaneMarkers.Add(new RBMid.LANEMARKER
        {

        });
        TrillMarkers.Add(new RBMid.GTRTRILLS
        {

        });
        DrumMixes.Add(new RBMid.DRUMMIXES
        {

        });
        GemTracks.Add(new RBMid.GEMTRACK
        {

        });
        OverdriveSoloSections.Add(new RBMid.SECTIONS
        {

        });
        HandMap.Add(new RBMid.HANDMAP
        {

        });
        HandPos.Add(new RBMid.HANDPOS
        {

        });
        Unktrack.Add(new RBMid.UNKTRACK
        {

        });
      }

      private void HandleKeysAnimTrk(MidiTrack track)
      {
        var anims = new List<RBMid.ANIM.EVENT>();
        var notes = new int[25];
        EachMessage(track, (msg, ticks, time) =>
        {
          switch (msg)
          {
            case NoteOnEvent e:
              if(e.Key >= 48 && e.Key <= 72)
              {
                notes[e.Key - 48] = anims.Count;
                anims.Add(new RBMid.ANIM.EVENT
                {
                  StartMillis = time * 1000,
                  StartTick = ticks,
                  KeyBitfield = 1 << (e.Key - 48)
                });
              }
              break;
            case NoteOffEvent e:
              if(e.Key >= 48 && e.Key <= 72)
              {
                var anim = anims[notes[e.Key - 48]];
                anim.LengthTicks = (ushort)(ticks - anim.StartTick);
                // TODO: Where does this value actually come from?
                anim.OtherLength = anim.LengthTicks;
                // TODO: Usually this is 256, or 0, or 65536 (so maybe it is actually 4 bools?)
                anim.Unknown2 = 256;
                // TODO
                anim.Unknown3 = 0;
                anims[notes[e.Key - 48]] = anim;
              }
              break;
          }
        });
        Anims.Add(new RBMid.ANIM
        {
          TrackName = track.Name,
          Events = anims.ToArray(),
          Unknown1 = 1,
          Unknown2 = 120,
          Unknown3 = 120
        });
      }

      private void HandleVocalsTrk(MidiTrack track)
      {
        var lyrics = new List<RBMid.TICKTEXT>();
        EachMessage(track, (msg, ticks, time) =>
        {
          switch (msg)
          {
            case TrackName e:
              break;
            case MetaTextEvent e:
              if (e.Text[0] != '[')
              {
                lyrics.Add(new RBMid.TICKTEXT
                {
                  Text = e.Text,
                  Tick = ticks,
                });
              }
              break;
          }
        });


        Lyrics.Add(new RBMid.LYRICS
        {
          TrackName = track.Name,
          Lyrics = lyrics.ToArray(),
          Unknown1 = 3,
          Unknown2 = 3,
          Unknown3 = 0
        });
        DrumFills.Add(new RBMid.DRUMFILLS
        {
        });
        ProMarkers.Add(new RBMid.CYMBALMARKER
        {
          Markers = new RBMid.CYMBALMARKER.MARKER[1]
        });
        LaneMarkers.Add(new RBMid.LANEMARKER
        {

        });
        TrillMarkers.Add(new RBMid.GTRTRILLS
        {

        });
        DrumMixes.Add(new RBMid.DRUMMIXES
        {
          Mixes = new RBMid.TICKTEXT[4][]
        });
        GemTracks.Add(new RBMid.GEMTRACK
        {
          Gems = new RBMid.GEMTRACK.GEM[4][]
        });
        OverdriveSoloSections.Add(new RBMid.SECTIONS
        {
          Sections = new RBMid.SECTIONS.SECTION[4][][]
        });
        VocalTracks.Add(new RBMid.VOCALTRACK
        {
          
        });
        HandMap.Add(new RBMid.HANDMAP
        {

        });
        HandPos.Add(new RBMid.HANDPOS
        {

        });
        Unktrack.Add(new RBMid.UNKTRACK
        {

        });
      }

      private void HandleHarmTrk(MidiTrack track)
      {
        var lyrics = new List<RBMid.TICKTEXT>();
        EachMessage(track, (msg, ticks, time) =>
        {
          switch (msg)
          {
            case TrackName e:
              break;
            case MetaTextEvent e:
              if (e.Text[0] != '[')
              {
                lyrics.Add(new RBMid.TICKTEXT
                {
                  Text = e.Text,
                  Tick = ticks,
                });
              }
              break;
          }
        });


        Lyrics.Add(new RBMid.LYRICS
        {
          TrackName = track.Name,
          Lyrics = lyrics.ToArray(),
          Unknown1 = 3,
          Unknown2 = 3,
          Unknown3 = 0
        });
        DrumFills.Add(new RBMid.DRUMFILLS
        {
        });
        ProMarkers.Add(new RBMid.CYMBALMARKER
        {

        });
        LaneMarkers.Add(new RBMid.LANEMARKER
        {

        });
        TrillMarkers.Add(new RBMid.GTRTRILLS
        {

        });
        DrumMixes.Add(new RBMid.DRUMMIXES
        {

        });
        GemTracks.Add(new RBMid.GEMTRACK
        {

        });
        OverdriveSoloSections.Add(new RBMid.SECTIONS
        {

        });
        VocalTracks.Add(new RBMid.VOCALTRACK
        {

        });
        HandMap.Add(new RBMid.HANDMAP
        {

        });
        HandPos.Add(new RBMid.HANDPOS
        {

        });
        Unktrack.Add(new RBMid.UNKTRACK
        {

        });
      }

      private void HandleEventsTrk(MidiTrack track)
      {
        EachMessage(track, (msg, ticks, time) =>
        {
          switch (msg)
          {

          }
        });
      }

      private void HandleBeatTrk(MidiTrack track)
      {
        EachMessage(track, (msg, ticks, time) =>
        {
          switch (msg)
          {
            case NoteOnEvent e:
              switch (e.Key)
              {
                case 12:
                case 13:
                  Beats.Add(new RBMid.BEAT
                  {
                    Tick = ticks,
                    Downbeat = e.Key == 12
                  });
                  break;
              }
              break;
          }
        });
      }

      private void HandleMarkupTrk(MidiTrack track)
      {
        EachMessage(track, (msg, ticks, time) =>
        {
          switch (msg)
          {

          }
        });
      }

      private void HandleVenueTrk(MidiTrack track)
      {
        EachMessage(track, (msg, ticks, time) =>
        {
          switch (msg)
          {

          }
        });
      }
    }
  }
}
