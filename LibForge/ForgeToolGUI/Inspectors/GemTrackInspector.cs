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
  public partial class GemTrackInspector : Inspector
  {
    public GemTrackInspector()
    {
      InitializeComponent();
      difficultySelector.SelectedIndex = 3;
      gemDisplay.Image = new Bitmap(gemDisplay.Width, gemDisplay.Height);
    }
    
    public void PreviewGemTrack(RBMid.GEMTRACK track)
    {
      var lastGem = track.Gems[3][track.Gems[3].Length - 1];
      var length = lastGem.StartMillis + lastGem.LengthMillis;
      previewState = new GemTrackPreviewState
      {
        GemTrack = track,
        diff = difficultySelector.SelectedIndex,
        scroll = 0,
        length = length,
        zoom = 1,
        enabled = true,
      };
      RenderGemTrack();
    }
    
    private class GemTrackPreviewState
    {
      public RBMid.GEMTRACK GemTrack;
      public int scroll;
      public float length;
      public int diff;
      public float zoom;
      public bool enabled = false;
    }
    private GemTrackPreviewState previewState = new GemTrackPreviewState();

    private void RenderGemTrack()
    {
      if (!previewState.enabled) return;
      const float scale_factor = 0.01f;
      var track = previewState.GemTrack.Gems[previewState.diff];
      if (track.Length == 0) return;
      var virtWidth = previewState.zoom * previewState.length * scale_factor;
      var offset = previewState.scroll / 1000.0f * virtWidth;
      var scale = scale_factor * previewState.zoom;
      using (var g = Graphics.FromImage(gemDisplay.Image))
      {
        g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
        g.Clear(Color.White);
        foreach (var gem in track)
        {
          var left = gem.StartMillis * scale - offset;
          if (left < 0)
            continue;
          if (left > gemDisplay.Image.Width)
            break;
          var width = gem.LengthMillis * scale;
          for (var lane = 0; lane < 5; lane++)
          {
            var brush = gem.ProCymbal == 0 ? Brushes.Blue : Brushes.Red;
            if (((1 << lane) & gem.Lanes) != 0)
            {
              g.FillRectangle(brush, left, 75 - lane * 15, width, 10);
            }
          }
        }
      }
      gemDisplay.Refresh();
    }

    private void ScrollGemTrack(int value)
    {
      previewState.scroll = value;
      RenderGemTrack();
    }

    private void seekBar_Scroll(object sender, ScrollEventArgs e)
    {
      ScrollGemTrack(e.NewValue);
    }

    private void difficultySelector_SelectedIndexChanged(object sender, EventArgs e)
    {
      previewState.diff = difficultySelector.SelectedIndex;
      RenderGemTrack();
    }


    private void gemDisplay_SizeChanged(object sender, EventArgs e)
    {
      if (gemDisplay.Width > 0 && gemDisplay.Height > 0)
      {
        gemDisplay.Image = new Bitmap(gemDisplay.Width, gemDisplay.Height);
        RenderGemTrack();
      }
    }

    private void zoomBar_Scroll(object sender, EventArgs e)
    {
      previewState.zoom = (float)Math.Pow(1.5f, zoomBar.Value / 4f);
      RenderGemTrack();
    }
  }
}
