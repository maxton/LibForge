using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using LibForge.Midi;

namespace ForgeToolGUI
{
  public partial class RBMidiInspector : Inspector
  {
    public RBMidiInspector(RBMid file)
    {
      InitializeComponent();
      gemDisplay.Image = new Bitmap(gemDisplay.Width, gemDisplay.Height);
      var Midi = new MidiCS.MidiFile(MidiCS.MidiFormat.MultiTrack, file.MidiTracks.ToList(), 480);
      previewState = new GemTrackPreviewState
      {
        rbmid = file,
        tempos = Midi.TempoTimeSigMap,
        diff = difficultySelector.SelectedIndex,
        scroll = 0,
        length = (float)(Midi.Duration * 1000.0),
        zoom = 1,
        enabled = false,
      };

      difficultySelector.Items.Clear();
      for(var track = 0; track < file.GemTracks.Length; track++)
      {
        for (var i = 0; i < file.GemTracks[track].Gems.Length; i++)
        {
          if (file.GemTracks[track].Gems[i].Length == 0)
            continue;
          difficultySelector.Items.Add($"Gem track {track} difficulty {i}");
          SelectorActions.Add(MakeGemtrackAction(track, i));
        }
      }
      for(var track = 0; track < file.VocalTracks.Length; track++)
      {
        difficultySelector.Items.Add($"Vocal track {track}");
        SelectorActions.Add(MakeVocalTrackAction(track));
      }
      button3_Click(this, null);
    }

    private List<Action> SelectorActions = new List<Action>();
    private Action MakeGemtrackAction(int track, int diff)
    {
      return () =>
      {
        previewState.enabled = true;
        RenderGemTrack(previewState.rbmid.GemTracks[track].Gems[diff]);
      };
    }
    private Action MakeVocalTrackAction(int track)
    {
      return () =>
      {
        previewState.enabled = true;
        RenderVocalTrack(previewState.rbmid.VocalTracks[track]);
      };
    }
    
    private class GemTrackPreviewState
    {
      public RBMid rbmid;
      public IList<MidiCS.TimeSigTempoEvent> tempos;
      public int GemTrack;
      public int scroll;
      public float length;
      public int diff;
      public float zoom;
      public bool enabled = false;
      public List<Gem> gems = new List<Gem>();
      public Color clearColor = Color.DarkGray;
      public Brush[] brushes = new[] { Brushes.Black };
    }
    private GemTrackPreviewState previewState;

    private struct Gem
    {
      public float startMillis;
      public float lengthMillis;
      public string label;
      public int offsetFromTop;
      public int offsetFromTop2;
      public int color;
    }

    private void RenderGems()
    {
      if (!previewState.enabled) return;
      const float scale_factor = 0.01f;
      const int height = 10;
      const int interval = 12;
      var textbrush = Brushes.White;
      var virtWidth = previewState.zoom * previewState.length * scale_factor;
      var offset = previewState.scroll / 1000.0f * virtWidth;
      var scale = scale_factor * previewState.zoom;
      using (var g = Graphics.FromImage(gemDisplay.Image))
      {
        g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
        g.Clear(previewState.clearColor);
        var brushes = previewState.brushes;
        var corners = new PointF[4];
        foreach (var gem in previewState.gems)
        {
          var left = gem.startMillis * scale - offset;
          var width = gem.lengthMillis * scale;
          if (left < -width)
            continue;
          if (left > gemDisplay.Image.Width)
            break;
          corners[0].X = left;
          corners[1].X = left;
          corners[3].X = left + width;
          corners[2].X = left + width;
          corners[0].Y = gem.offsetFromTop * interval;
          corners[1].Y = gem.offsetFromTop * interval + height;
          corners[3].Y = gem.offsetFromTop2 * interval;
          corners[2].Y = gem.offsetFromTop2 * interval + height;
          g.FillPolygon(brushes[gem.color], corners);
          if(gem.label != null)
          {
            g.DrawString(gem.label, SystemFonts.DefaultFont, textbrush, left, gem.offsetFromTop * interval);
          }
        }
      }
      gemDisplay.Refresh();
    }

    Brush[] GemBrushes = new[] { Brushes.Green, Brushes.Red, Brushes.Yellow, Brushes.Blue, Brushes.Orange };

    private void RenderGemTrack(RBMid.GEMTRACK.GEM[] track)
    {
      if (!previewState.enabled) return;
      previewState.gems.Clear();
      foreach (var gem in track)
      {
        for (var lane = 0; lane < 5; lane++)
        {
          if (((1 << lane) & gem.Lanes) != 0)
          {
            previewState.gems.Add(new Gem
            {
              startMillis = gem.StartMillis,
              lengthMillis = gem.LengthMillis,
              offsetFromTop = 5 - lane,
              offsetFromTop2 = 5 - lane,
              color = lane,
              label = gem.IsHopo ?  "H" : null
            });
          }
        }
      }
      previewState.brushes = GemBrushes;
      RenderGems();
    }

    private float GetMillis(uint tick)
    {
      var tempo = previewState.tempos.Last(e => e.Tick <= tick);
      return (float)(tempo.Time + ((tick - tempo.Tick) / 480.0) * (60 / tempo.BPM)) * 1000f;
    }

    Brush[] VocalBrushes = { Brushes.Black, Brushes.Red, Brushes.Orange, Brushes.Yellow, Brushes.Green, Brushes.Blue, Brushes.Violet, Brushes.White };
    const int MaxVocalNote = 86;
    private void RenderVocalTrack(RBMid.VOCALTRACK track)
    {
      if (!previewState.enabled) return;
      previewState.gems.Clear();
      foreach (var gem in track.Notes)
      {
        previewState.gems.Add(new Gem
        {
          startMillis = gem.StartMillis,
          lengthMillis = gem.LengthMillis,
          offsetFromTop = MaxVocalNote - gem.MidiNote,
          offsetFromTop2 = MaxVocalNote - gem.MidiNote2,
          color = gem.LastNoteInPhrase ? 1 : (gem.Unpitched ? 2 : 0),
          label = gem.Lyric
        });
      }
      previewState.gems.Add(new Gem
      {
        offsetFromTop = 4,
        label = "Authored Phrases:"
      });
      foreach (var x in track.AuthoredPhraseMarkers)
      {
        var startMillis = GetMillis(x.StartTicks);
        var lengthMllis = GetMillis(x.LengthTicks);
        previewState.gems.Add(new Gem
        {
          startMillis = startMillis,
          lengthMillis = lengthMllis,
          offsetFromTop = 4,
          offsetFromTop2 = 4,
          color = x.PercussionSection ? 1 : 0,
          label = (startMillis / 1000).ToString("0.000"),
        });
        previewState.gems.Add(new Gem
        {
          startMillis = startMillis,
          lengthMillis = lengthMllis,
          offsetFromTop = 5,
          offsetFromTop2 = 5,
          color = x.PhraseFlags,
          label = (startMillis / 1000).ToString("0.000"),
        });
      }
      foreach (var x in track.FakePhraseMarkers)
      {
        previewState.gems.Add(new Gem
        {
          startMillis = x.StartMillis,
          lengthMillis = x.LengthMillis,
          offsetFromTop = 1,
          offsetFromTop2 = 1,
          color = (x.HasPitchedVox ? 1 : 0) + (x.HasUnpitchedVox ? 2 : 0),
          label = null
        });
        previewState.gems.Add(new Gem
        {
          startMillis = x.StartMillis,
          lengthMillis = x.LengthMillis,
          offsetFromTop = 2,
          offsetFromTop2 = 2,
          color = x.PercussionSection ? 1 : 0,
          label = null
        });
        previewState.gems.Add(new Gem
        {
          startMillis = x.StartMillis,
          lengthMillis = x.LengthMillis,
          offsetFromTop = 3,
          offsetFromTop2 = 3,
          color = x.PhraseFlags,
          label = (x.StartMillis / 1000).ToString("0.000")
        });
      }
      previewState.gems.Add(new Gem
      {
        label = "Tacets:"
      });
      foreach(var x in track.FreestyleRegions)
      {
        previewState.gems.Add(new Gem
        {
          startMillis = x.StartMillis,
          lengthMillis = x.EndMillis - x.StartMillis,
          offsetFromTop = 0,
          offsetFromTop2 = 0,
          color = 0,
          label = (x.StartMillis / 1000).ToString("0.000")
        });
      }

      previewState.gems.Sort((x, y) => x.startMillis.CompareTo(y.startMillis));
      previewState.brushes = VocalBrushes;
      RenderGems();
    }

    private void ScrollGemTrack(int value)
    {
      previewState.scroll = value;
      RenderGems();
    }

    private void seekBar_Scroll(object sender, ScrollEventArgs e)
    {
      ScrollGemTrack(e.NewValue);
    }

    private void difficultySelector_SelectedIndexChanged(object sender, EventArgs e)
    {
      SelectorActions[difficultySelector.SelectedIndex]();
    }


    private void gemDisplay_SizeChanged(object sender, EventArgs e)
    {
      if (gemDisplay.Width > 0 && gemDisplay.Height > 0)
      {
        gemDisplay.Image = new Bitmap(gemDisplay.Width, gemDisplay.Height);
        RenderGems();
      }
    }

    private void zoomBar_Scroll(object sender, EventArgs e)
    {
      previewState.zoom = (float)Math.Pow(1.5f, zoomBar.Value / 4f);
      RenderGems();
    }

    private void button3_Click(object sender, EventArgs e)
    {
      splitContainer2.Panel2.Controls.Remove(button3);
      splitContainer2.SplitterDistance = 200;
      splitContainer2.Panel2.Controls.Add(new ObjectInspector(previewState.rbmid) { Dock = DockStyle.Fill });
    }

    private void button1_Click(object sender, EventArgs e)
    {
      var reprocessed = RBMidConverter.ToRBMid(RBMidConverter.ToMid(previewState.rbmid));
      fb.OpenTab(InspectorFactory.GetInspector(reprocessed), Name + " reprocessed");
    }

    private void Button2_Click(object sender, EventArgs e)
    {
      var midi = RBMidConverter.ToMid(previewState.rbmid);
      using (var fod = new SaveFileDialog() { FileName = previewState.rbmid.MidiTrackNames[0] + ".mid" })
      {
        if(fod.ShowDialog() == DialogResult.OK)
        {
          using(var of = System.IO.File.OpenWrite(fod.FileName))
          {
            MidiCS.MidiFileWriter.WriteSMF(midi, of);
          }
        }
      }
    }
  }
}
