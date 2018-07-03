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
        Midi = file,
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
          difficultySelector.Items.Add($"Track {track} difficulty {i}");
        }
      }
    }
    
    public void PreviewGemTrack()
    {
      previewState.enabled = true;
    }
    
    private class GemTrackPreviewState
    {
      public RBMid Midi;
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
      public int color;
    }

    private void RenderGems()
    {
      if (!previewState.enabled) return;
      const float scale_factor = 0.01f;
      var virtWidth = previewState.zoom * previewState.length * scale_factor;
      var offset = previewState.scroll / 1000.0f * virtWidth;
      var scale = scale_factor * previewState.zoom;
      using (var g = Graphics.FromImage(gemDisplay.Image))
      {
        g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
        g.Clear(previewState.clearColor);
        var brushes = previewState.brushes;
        foreach (var gem in previewState.gems)
        {
          var left = gem.startMillis * scale - offset;
          var width = gem.lengthMillis * scale;
          if (left < -width)
            continue;
          if (left > gemDisplay.Image.Width)
            break;
          g.FillRectangle(brushes[gem.color], left, gem.offsetFromTop * 15, width, 10);
          if(gem.label != null)
          {
            g.DrawString(gem.label, SystemFonts.DefaultFont, brushes[gem.color], left, gem.offsetFromTop * 15 + 10);
          }
        }
      }
      gemDisplay.Refresh();
    }

    Brush[] GemBrushes = new[] { Brushes.Green, Brushes.Red, Brushes.Yellow, Brushes.Blue, Brushes.Orange };

    private void RenderGemTrack()
    {
      if (!previewState.enabled) return;
      previewState.gems.Clear();
      var track = previewState.Midi.GemTracks[previewState.GemTrack].Gems[previewState.diff];
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
              color = lane,
              label = gem.IsHopo ?  "H" : null
            });
          }
        }
      }
      previewState.brushes = GemBrushes;
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
      previewState.GemTrack = difficultySelector.SelectedIndex >> 2;
      previewState.diff = difficultySelector.SelectedIndex & 3;
      previewState.enabled = true;
      RenderGemTrack();
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
      splitContainer2.Panel2.Controls.Add(new ObjectInspector(previewState.Midi) { Dock = DockStyle.Fill });
    }
  }
}
