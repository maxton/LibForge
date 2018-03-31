using System;
using System.Collections.Generic;
using MidiCS;
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
      private float PreviewStart;
      private float PreviewEnd;
      private uint LastMarkupTick;
      private uint FinalTick = 0;

      public MidiConverter(MidiFile mf)
      {
        this.mf = mf;
        trackHandlers = new Dictionary<string, Action<MidiTrackProcessed>>
        {
          {"PART DRUMS", HandleDrumTrk },
          {"PART BASS", HandleGuitarBass },
          {"PART GUITAR", HandleGuitarBass },
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
        var processedTracks = new MidiHelper().ProcessTracks(mf);
        processedTracks.ForEach(ProcessTrack);
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
          PreviewStartMillis = PreviewStart,
          PreviewEndMillis = PreviewEnd,
          UnknownTwo = 2,
          LastMarkupEventTick = LastMarkupTick,
          NumPlayableTracks = (uint)Lyrics.Count,
          FinalTick = FinalTick,
          UnknownHundred = 100f,
          UnknownNegOne = -1,
          UnknownOne = 1,
          UnknownZeroByte = 0,
          UnknownZero = 0,
          // TODO: When should this not be 0xAA?
          Unknown6 = 0xAA,
        };
        return rb;
      }
      private Dictionary<string, Action<MidiTrackProcessed>> trackHandlers;
      private MidiTrackProcessed currentTrack;

      private void ProcessTrack(MidiTrackProcessed track)
      {
        currentTrack = track;
        MidiTrackNames.Add(track.Name);
        if (MidiTrackNames.Count == 1)
          return;
        else if (trackHandlers.ContainsKey(track.Name))
          trackHandlers[track.Name](track);
      }

      const byte Roll2 = 127;
      const byte Roll1 = 126;
      const byte DrumFillMarkerEnd = 124;
      const byte DrumFillMarkerStart = 120;
      const byte OverdriveMarker = 116;
      const byte ProGreen = 112;
      const byte ProBlue = 111;
      const byte ProYellow = 110;
      const byte SoloMarker = 103;
      const byte ExpertHopoOff = 102;
      const byte ExpertHopoOn = 101;
      const byte ExpertEnd = 100;
      const byte ExpertStart = 96;
      const byte HardHopoOff = 90;
      const byte HardHopoOn = 89;
      const byte HardEnd = 88;
      const byte HardStart = 84;
      const byte MediumEnd = 76;
      const byte MediumStart = 72;
      const byte EasyEnd = 64;
      const byte EasyStart = 60;
      const byte DrumAnimEnd = 51;
      const byte DrumAnimStart = 24;
      private void HandleDrumTrk(MidiTrackProcessed track)
      {
        var drumfills = new List<RBMid.DRUMFILLS.FILL>();
        var fills_unk = new List<RBMid.DRUMFILLS.FILL_LANES>();
        var cymbal_markers = new SortedDictionary<uint, RBMid.CYMBALMARKER.MARKER>();
        var overdrive_markers = new List<RBMid.SECTIONS.SECTION>();
        var solo_markers = new List<RBMid.SECTIONS.SECTION>();
        var gem_tracks = new List<RBMid.GEMTRACK.GEM>[4];
        cymbal_markers[0] = new RBMid.CYMBALMARKER.MARKER
        {
          Tick = 0, Flags = 0
        };
        var marker_ends = new uint[3];
        var endmarkers = new RBMid.CYMBALMARKER.MARKER[3];
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
        bool AddGem(MidiNote e)
        {
          var key = e.Key;
          var lane = 0;
          var diff = 0;
          if(key >= EasyStart && key <= EasyEnd)
          {
            lane = key - EasyStart;
            diff = 0;
          }
          else if(key >= MediumStart && key <= MediumEnd)
          {
            lane = key - MediumStart;
            diff = 1;
          }
          else if (key >= HardStart && key <= HardEnd)
          {
            lane = key - HardStart;
            diff = 2;
          }
          else if (key >= ExpertStart && key <= ExpertEnd)
          {
            lane = key - ExpertStart;
            diff = 3;
          }
          else
          {
            return false;
          }

          if (gem_tracks[diff] == null) gem_tracks[diff] = new List<RBMid.GEMTRACK.GEM>();
          gem_tracks[diff].Add(new RBMid.GEMTRACK.GEM
          {
            StartMillis = (float)e.StartTime * 1000,
            StartTicks = e.StartTicks,
            LengthMillis = (ushort)(e.Length * 1000),
            LengthTicks = (ushort)e.LengthTicks,
            Lanes = 1 << lane,
            IsHopo = false,
            NoTail = true
          });
          return true;
        }
        foreach(var item in track.Items)
        {
          var ticks = item.StartTicks;
          var time = item.StartTime;
          switch (item)
          {
            case MidiNote e:
              if (e.Key == DrumFillMarkerStart)
              {
                fills_unk.Add(new RBMid.DRUMFILLS.FILL_LANES
                {
                  Tick = ticks,
                  Lanes = 0b11111 // TODO: parse each lane
                });
                drumfills.Add(new RBMid.DRUMFILLS.FILL
                {
                  StartTick = ticks,
                  EndTick = ticks + e.LengthTicks, // TODO: this seems to be rounded up to the next note
                  IsBRE = 0
                });
              }
              else if (e.Key >= DrumFillMarkerStart && e.Key <= DrumFillMarkerEnd) { }
              else if (e.Key == OverdriveMarker)
              {
                overdrive_markers.Add(new RBMid.SECTIONS.SECTION
                {
                  StartTicks = ticks,
                  LengthTicks = e.LengthTicks
                });
              }
              else if (e.Key == SoloMarker)
              {
                solo_markers.Add(new RBMid.SECTIONS.SECTION
                {
                  StartTicks = ticks,
                  LengthTicks = e.LengthTicks
                });
              }
              else if (e.Key == ProYellow || e.Key == ProBlue || e.Key == ProGreen)
              {
                var startMarker = cymbal_markers.ContainsKey(ticks) ? cymbal_markers[ticks]
                                                               : new RBMid.CYMBALMARKER.MARKER { Tick = ticks };
                startMarker.Flags |= GetFlag(e.Key);
                for(var i = 0; i < 3; i++)
                {
                  if(endmarkers[i] != null && endmarkers[i].Tick > e.StartTicks)
                  {
                    startMarker.Flags |= GetFlag((byte)(i + 110));
                    endmarkers[i].Flags |= GetFlag(e.Key);
                  }
                }
                cymbal_markers[ticks] = startMarker;

                var endTicks = ticks + e.LengthTicks;
                var endMarker = cymbal_markers.ContainsKey(endTicks) ? cymbal_markers[endTicks]
                                                                     : new RBMid.CYMBALMARKER.MARKER { Tick = endTicks };
                for (var i = 0; i < 3; i++)
                {
                  if (endmarkers[i] != null && endmarkers[i].Tick >= endTicks)
                  {
                    if (endmarkers[i].Tick != endTicks)
                    {
                      endMarker.Flags |= GetFlag((byte)(i + 110));
                    }
                    endmarkers[i].Flags &= ~GetFlag(e.Key);
                  }
                }
                endmarkers[e.Key - 110] = endMarker;
                cymbal_markers[endTicks] = endMarker;
              }
              else if (AddGem(e)) { }  // everything is handled in AddGem
              else if (e.Key >= DrumAnimStart && e.Key <= DrumAnimEnd)
              {

              }
              else if(e.Key == Roll1)
              {

              }
              else if(e.Key == Roll2)
              {

              }
              else
              {
                throw new Exception("Unhandled gem type ?!");
              }
              break;
          }
        }
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
          Lanes = fills_unk.ToArray()
        });
        ProMarkers.Add(new RBMid.CYMBALMARKER
        {
          Markers = cymbal_markers.Values.ToArray()
        });
        LaneMarkers.Add(new RBMid.LANEMARKER());
        TrillMarkers.Add(new RBMid.GTRTRILLS());
        DrumMixes.Add(new RBMid.DRUMMIXES
        {

        });
        GemTracks.Add(new RBMid.GEMTRACK
        {
          Gems = new RBMid.GEMTRACK.GEM[4][] {
            gem_tracks[0].ToArray(), gem_tracks[1].ToArray(),
            gem_tracks[2].ToArray(), gem_tracks[3].ToArray() }
        });
        var sections = new RBMid.SECTIONS.SECTION[6][];
        sections[0] = overdrive_markers.ToArray();
        sections[1] = solo_markers.ToArray();
        OverdriveSoloSections.Add(new RBMid.SECTIONS
        {
          Sections = new RBMid.SECTIONS.SECTION[4][][]
          {
            sections, sections, sections, sections
          }
        });
        HandMap.Add(new RBMid.HANDMAP());
        HandPos.Add(new RBMid.HANDPOS());
        Unktrack.Add(new RBMid.UNKTRACK
        {

        });
      }

      private static Dictionary<string, int> HandMaps = new Dictionary<string, int>
      {
        {"HandMap_Default", 0 },
        {"HandMap_AllBend", 1 },
        {"HandMap_AllChords", 2 },
        {"HandMap_Chord_A", 3 },
        {"HandMap_Chord_C", 4 },
        {"HandMap_Chord_D", 5 },
        {"HandMap_Chord_DropD", 6 },
        {"HandMap_Chord_DropD2", 7 },
        {"HandMap_NoChords", 8 },
        {"HandMap_Solo", 9 },
      };

      const byte TrillMarker = 127;
      private void HandleGuitarBass(MidiTrackProcessed track)
      {
        var drumfills = new List<RBMid.DRUMFILLS.FILL>();
        var fills_unk = new List<RBMid.DRUMFILLS.FILL_LANES>();
        var gem_tracks = new List<RBMid.GEMTRACK.GEM>[4];
        RBMid.GEMTRACK.GEM[] chords = new RBMid.GEMTRACK.GEM[4];
        var trills = new List<RBMid.GTRTRILLS.TRILL>();
        var trill = new RBMid.GTRTRILLS.TRILL();
        var maps = new List<RBMid.HANDMAP.MAP>();

        bool AddGem(MidiNote e)
        {
          var key = e.Key;
          var lane = 0;
          var diff = 0;
          if (key >= EasyStart && key <= EasyEnd)
          {
            lane = key - EasyStart;
            diff = 0;
          }
          else if (key >= MediumStart && key <= MediumEnd)
          {
            lane = key - MediumStart;
            diff = 1;
          }
          else if (key >= HardStart && key <= HardEnd)
          {
            lane = key - HardStart;
            diff = 2;
          }
          else if (key >= ExpertStart && key <= ExpertEnd)
          {
            lane = key - ExpertStart;
            diff = 3;
          }
          else
          {
            return false;
          }

          if (gem_tracks[diff] == null) gem_tracks[diff] = new List<RBMid.GEMTRACK.GEM>();
          if (chords[diff] != null && chords[diff].StartTicks == e.StartTicks)
          {
            chords[diff].Lanes |= (1 << lane);
          }
          else
          {
            bool hopo = false;
            if(chords[diff] != null)
            {
              if(e.StartTicks - chords[diff].StartTicks <= 120)
              {
                hopo = true;
              }
            }
            var chord = new RBMid.GEMTRACK.GEM
            {
              StartMillis = (float)e.StartTime * 1000,
              StartTicks = e.StartTicks,
              LengthMillis = (ushort)(e.Length * 1000),
              LengthTicks = (ushort)e.LengthTicks,
              Lanes = 1 << lane,
              IsHopo = hopo,
              NoTail = e.LengthTicks <= 120
            };
            chords[diff] = chord;
            gem_tracks[diff].Add(chord);
          }
          return true;
        }
        bool AddHopo(MidiNote e)
        {
          var key = e.Key;
          var diff = 0;
          bool force;
          switch (e.Key)
          {
            case ExpertHopoOff:
              diff = 3;
              force = false;
              break;
            case ExpertHopoOn:
              diff = 3;
              force = true;
              break;
            case HardHopoOff:
              diff = 2;
              force = false;
              break;
            case HardHopoOn:
              diff = 2;
              force = true;
              break;
            default:
              return false;
          }
          if(chords[diff] != null)
          {
            if(chords[diff].StartTicks == e.StartTicks)
            {
              chords[diff].IsHopo = force;
            }
            else
            {
              chords[diff] = new RBMid.GEMTRACK.GEM
              {
                StartMillis = (float)e.StartTime * 1000,
                StartTicks = e.StartTicks,
                LengthMillis = (ushort)(e.Length * 1000),
                LengthTicks = (ushort)e.LengthTicks,
                Lanes = 0,
                IsHopo = force,
                NoTail = e.LengthTicks < 120
              };
              gem_tracks[diff].Add(chords[diff]);
            }
          }
          return true;
        }
        foreach (var item in track.Items)
        {
          switch (item)
          {
            case MidiNote e:
              if (e.Key == DrumFillMarkerStart)
              {
                fills_unk.Add(new RBMid.DRUMFILLS.FILL_LANES
                {
                  Tick = e.StartTicks,
                  Lanes = 31
                });
                drumfills.Add(new RBMid.DRUMFILLS.FILL
                {
                  StartTick = e.StartTicks,
                  EndTick = e.StartTicks + e.LengthTicks,
                  IsBRE = 1
                });
              }
              else if (AddGem(e)) { }
              else if (AddHopo(e)) { }
              else if (e.Key == TrillMarker)
              {
                trill = new RBMid.GTRTRILLS.TRILL
                {
                  StartTick = e.StartTicks,
                  EndTick = e.StartTicks + e.LengthTicks,
                  LowFret = 4,
                  HighFret = 0
                };
                trills.Add(trill);
              }
              else if (e.Key >= ExpertStart && e.Key <= ExpertEnd)
              {
                if (trill.EndTick >= e.StartTicks)
                {
                  var note = e.Key - ExpertStart;
                  if (note < trill.LowFret) trill.LowFret = note;
                  if (note > trill.HighFret) trill.HighFret = note;
                }
              }
              break;
            case MidiText e:
              var regex = new System.Text.RegularExpressions.Regex("\\[map (HandMap_[A-Za-z_2]+)\\]");
              var match = regex.Match(e.Text);
              if (match.Success)
              {
                var mapType = HandMaps[match.Captures[0].Value];
                maps.Add(new RBMid.HANDMAP.MAP
                {
                  Map = mapType,
                  StartTime = (float)e.StartTime
                });
              }
              break;
          }
        }

        int Unk = track.Name == "PART BASS" ? 2 : 1;
        Lyrics.Add(new RBMid.LYRICS
        {
          TrackName = track.Name,
          Unknown1 = Unk,
          Unknown2 = Unk,
          Unknown3 = 0
        });
        DrumFills.Add(new RBMid.DRUMFILLS
        {
          Fills = drumfills.ToArray(),
          Lanes = fills_unk.ToArray()
        });
        ProMarkers.Add(new RBMid.CYMBALMARKER
        {
          Markers = new RBMid.CYMBALMARKER.MARKER[]
          {
            new RBMid.CYMBALMARKER.MARKER
            {
              Tick = 0,
              Flags = 0
            }
          }
        });
        LaneMarkers.Add(new RBMid.LANEMARKER
        {
          
        });
        TrillMarkers.Add(new RBMid.GTRTRILLS
        {
          Trills = new RBMid.GTRTRILLS.TRILL[4][]
          {
            null, null, null, trills.ToArray()
          }
        });
        DrumMixes.Add(new RBMid.DRUMMIXES
        {
          Mixes = new RBMid.TICKTEXT[4][]
        });
        GemTracks.Add(new RBMid.GEMTRACK
        {
          Gems = new RBMid.GEMTRACK.GEM[4][]
          {
            gem_tracks[0].ToArray(), gem_tracks[1].ToArray(), gem_tracks[2].ToArray(),
            gem_tracks[3].ToArray()
          }
        });
        OverdriveSoloSections.Add(new RBMid.SECTIONS
        {

        });
        HandMap.Add(new RBMid.HANDMAP
        {
          Maps = maps.ToArray()
        });
        HandPos.Add(new RBMid.HANDPOS
        {

        });
        Unktrack.Add(new RBMid.UNKTRACK
        {

        });
      }

      private void HandleRealKeysXTrk(MidiTrackProcessed track)
      {
        foreach(var item in track.Items)
        {
          switch (item)
          {

          }
        }

        Lyrics.Add(new RBMid.LYRICS
        {
          TrackName = track.Name,
          Unknown1 = 4,
          Unknown2 = 5,
          Unknown3 = 0
        });
        DrumFills.Add(new RBMid.DRUMFILLS());
        ProMarkers.Add(new RBMid.CYMBALMARKER
        {
          Markers = new RBMid.CYMBALMARKER.MARKER[]
          {
            new RBMid.CYMBALMARKER.MARKER
            {
              Tick = 0,
              Flags = 0
            }
          }
        });
        LaneMarkers.Add(new RBMid.LANEMARKER());
        TrillMarkers.Add(new RBMid.GTRTRILLS
        {

        });
        DrumMixes.Add(new RBMid.DRUMMIXES
        {
          Mixes = new RBMid.TICKTEXT[4][]
        });
        GemTracks.Add(new RBMid.GEMTRACK
        {

        });
        OverdriveSoloSections.Add(new RBMid.SECTIONS
        {

        });
        HandMap.Add(new RBMid.HANDMAP());
        HandPos.Add(new RBMid.HANDPOS());
        Unktrack.Add(new RBMid.UNKTRACK());
      }

      private void HandleKeysAnimTrk(MidiTrackProcessed track)
      {
        var anims = new List<RBMid.ANIM.EVENT>();
        foreach(var item in track.Items)
        {
          switch (item)
          {
            case MidiNote e:
              if(e.Key >= 48 && e.Key <= 72)
              {
                anims.Add(new RBMid.ANIM.EVENT
                {
                  StartMillis = (float)(e.StartTime * 1000),
                  StartTick = e.StartTicks,
                  KeyBitfield = 1 << (e.Key - 48),
                  LengthTicks = (ushort)(e.LengthTicks),
                  LengthMillis = (ushort)(e.Length * 1000),
                  // TODO: Usually this is 256, or 0, or 65536 (so maybe it is actually 4 bools?)
                  Unknown2 = 256,
                  // TODO
                  Unknown3 = 0
                });
              }
              break;
          }
        }
        Anims.Add(new RBMid.ANIM
        {
          TrackName = track.Name,
          Events = anims.ToArray(),
          Unknown1 = 1,
          Unknown2 = 120,
          Unknown3 = 120
        });
      }

      private void HandleVocalsTrk(MidiTrackProcessed track)
      {
        var lyrics = new List<RBMid.TICKTEXT>();
        foreach(var item in track.Items)
        {
          switch (item)
          {
            case MidiText e:
              if (e.Text[0] != '[')
              {
                lyrics.Add(new RBMid.TICKTEXT
                {
                  Text = e.Text,
                  Tick = e.StartTicks,
                });
              }
              break;
          }
        }


        Lyrics.Add(new RBMid.LYRICS
        {
          TrackName = track.Name,
          Lyrics = lyrics.ToArray(),
          Unknown1 = 3,
          Unknown2 = 3,
          Unknown3 = 0
        });
        DrumFills.Add(new RBMid.DRUMFILLS());
        ProMarkers.Add(new RBMid.CYMBALMARKER
        {
          Markers = new RBMid.CYMBALMARKER.MARKER[]
          {
            new RBMid.CYMBALMARKER.MARKER
            {
              Tick = 0,
              Flags = 0
            }
          }
        });
        LaneMarkers.Add(new RBMid.LANEMARKER());
        TrillMarkers.Add(new RBMid.GTRTRILLS());
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
        HandMap.Add(new RBMid.HANDMAP());
        HandPos.Add(new RBMid.HANDPOS());
        Unktrack.Add(new RBMid.UNKTRACK());
      }

      private void HandleHarmTrk(MidiTrackProcessed track)
      {
        var lyrics = new List<RBMid.TICKTEXT>();
        foreach(var item in track.Items)
        {
          switch (item)
          {
            case MidiText e:
              if (e.Text[0] != '[')
              {
                lyrics.Add(new RBMid.TICKTEXT
                {
                  Text = e.Text,
                  Tick = e.StartTicks,
                });
              }
              break;
          }
        }


        Lyrics.Add(new RBMid.LYRICS
        {
          TrackName = track.Name,
          Lyrics = lyrics.ToArray(),
          Unknown1 = 3,
          Unknown2 = 3,
          Unknown3 = 0
        });
        DrumFills.Add(new RBMid.DRUMFILLS());
        ProMarkers.Add(new RBMid.CYMBALMARKER
        {
          Markers = new RBMid.CYMBALMARKER.MARKER[1]
          {
            new RBMid.CYMBALMARKER.MARKER
            {
              Tick = 0,
              Flags = 0
            }
          }
        });
        LaneMarkers.Add(new RBMid.LANEMARKER());
        TrillMarkers.Add(new RBMid.GTRTRILLS());
        DrumMixes.Add(new RBMid.DRUMMIXES
        {
          Mixes = new RBMid.TICKTEXT[4][]
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
        HandMap.Add(new RBMid.HANDMAP());
        HandPos.Add(new RBMid.HANDPOS());
        Unktrack.Add(new RBMid.UNKTRACK());
      }

      private void HandleEventsTrk(MidiTrackProcessed track)
      {
        foreach(var item in track.Items)
        {
          var timeMillis = (float)(item.StartTime * 1000);
          switch (item)
          {
            case MidiText e:
              switch (e.Text)
              {
                case "[preview_start]":
                  PreviewStart = timeMillis;
                  break;
                case "[preview_end]":
                  PreviewEnd = timeMillis;
                  break;
                case "[preview]":
                  PreviewStart = timeMillis;
                  PreviewEnd = PreviewStart + 30_000;
                  break;
                case "[coda]":
                  // TODO: This would be better in the Drum track code,
                  // but we don't know if it's a BRE there because the lanes are the same for normal fills
                  var idx = DrumFills[0].Fills.Length - 1;
                  var lastDrumFill = DrumFills[0].Fills[idx];
                  lastDrumFill.IsBRE = 1;
                  DrumFills[0].Fills[idx] = lastDrumFill;
                  break;
              }
              break;
          }
        }
      }

      private void HandleBeatTrk(MidiTrackProcessed track)
      {
        foreach(var item in track.Items)
        {
          switch (item)
          {
            case MidiNote e:
              switch (e.Key)
              {
                case 12:
                case 13:
                  Beats.Add(new RBMid.BEAT
                  {
                    Tick = e.StartTicks,
                    Downbeat = e.Key == 12
                  });
                  break;
              }
              break;
          }
        }
      }

      const byte MarkupNotes1End = 11;
      const byte MarkupNotes1Start = 0;
      const byte MarkupNotes3End = 23;
      const byte MarkupNotes3Start = 12;
      private void HandleMarkupTrk(MidiTrackProcessed track)
      {
        LastMarkupTick = track.LastTick;
        foreach(var item in track.Items)
        {
          switch (item)
          {
            case MidiNote e:
              if(e.Key >= MarkupNotes1Start && e.Key <= MarkupNotes1End)
              {
                MarkupSoloNotes1.Add(new RBMid.MARKUP_SOLO_NOTES
                {
                  StartTick = e.StartTicks,
                  EndTick = e.StartTicks + e.LengthTicks,
                  NoteOffset = e.Key - MarkupNotes1Start
                });
              }
              else if(e.Key >= MarkupNotes3Start && e.Key <= MarkupNotes3End)
              {
                MarkupSoloNotes3.Add(new RBMid.MARKUP_SOLO_NOTES
                {
                  StartTick = e.StartTicks,
                  EndTick = e.StartTicks + e.LengthTicks,
                  NoteOffset = e.Key - MarkupNotes3Start
                });
              }
              break;
          }
        }
      }

      private void HandleVenueTrk(MidiTrackProcessed track)
      {
        foreach (var item in track.Items)
        {
          switch (item)
          {

          }
        }
      }
    }
  }
}
