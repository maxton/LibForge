using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using LibForge.Fuser;
using LibForge.Util;

namespace ForgeToolGUI.Inspectors
{
  public partial class FuserInspector : Inspector
  {
    class ListItem
    {
      public ResourceFile file;
      public override string ToString()
      {
        return $"{file.Filename} ({file.Type})";
      }
    }
    public FuserInspector(FuserAsset asset = null)
    {
      InitializeComponent();
      if (asset != null)
      {
        foreach (var resource in asset.ResourceFiles)
        {
          listBox1.Items.Add(new ListItem { file = resource });
        }
      }
    }

    private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
    {
      exportMidiButton.Enabled = listBox1.SelectedItem != null && (listBox1.SelectedItem as ListItem).file is MidiFileResource;
      exportMoggButton.Enabled = listBox1.SelectedItem != null && (listBox1.SelectedItem as ListItem).file is MoggSampleResource;
      button1.Enabled = exportFusionButton.Enabled = listBox1.SelectedItem != null && (listBox1.SelectedItem as ListItem).file is FusionPatchResource;
      if (exportFusionButton.Enabled)
      {
        var r = (listBox1.SelectedItem as ListItem).file as FusionPatchResource;
        var array = DtxCS.DTX.FromDtaString(r.data);
        propertyGrid1.SelectedObject = array.Deserialize<FusionPatch>();
      }
    }
    private void SaveFile<T>(Action<T,Stream> writer) where T : ResourceFile
    {
      if (listBox1.SelectedItem != null && (listBox1.SelectedItem as ListItem).file is T value)
      {
        using (var sfd = new SaveFileDialog() { FileName = value.Filename })
        {
          if (sfd.ShowDialog() == DialogResult.OK)
          {
            using (var s = File.OpenWrite(sfd.FileName))
            {
              writer(value, s);
            }
          }
        }
      }
    }
    private void exportMidiButton_Click(object sender, EventArgs e)
    {
      SaveFile<MidiFileResource>((mfr, s) =>
      {
        var midiFile = new MidiCS.MidiFile(MidiCS.MidiFormat.MultiTrack, mfr.MidiFile.MidiTracks.ToList(), 480);
        MidiCS.MidiFileWriter.WriteSMF(midiFile, s);
      });
    }
    private void exportMoggButton_Click(object sender, EventArgs e)
    {
      SaveFile<MoggSampleResource>((msr, s) =>
      {
        s.Write(msr.MoggFileData, 0, msr.MoggFileData.Length);
      });
    }
    private void exportFusionButton_Click(object sender, EventArgs e)
    {
      SaveFile<FusionPatchResource>((fpr, s) =>
      {
        using (var tw = new StreamWriter(s))
        {
          tw.Write(fpr.data);
        }
      });
    }

    private void button1_Click(object sender, EventArgs e)
    {
      SaveFile<FusionPatchResource>((fpr, s) =>
      {
        var array = DtxCS.DTX.FromDtaString(fpr.data);
        var patch = array.Deserialize<FusionPatch>();
        var serialized = patch.Serialize().ToFileString();
        using (var tw = new StreamWriter(s))
        {
          tw.Write(serialized);
        }
      });
    }
  }
}
