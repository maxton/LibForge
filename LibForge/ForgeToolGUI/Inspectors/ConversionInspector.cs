using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GameArchives.STFS;
using LibForge.Util;
using System.Text.RegularExpressions;

namespace ForgeToolGUI.Inspectors
{
  public partial class ConversionInspector : Inspector
  {
    public ConversionInspector()
    {
      InitializeComponent();
    }

    List<string> conFilenames = new List<string>();

    private void pickFileButton_Click(object sender, EventArgs e)
    {
      conFilenames.Clear();
      listBox1.Items.Clear();
      groupBox2.Enabled = false;
      groupBox3.Enabled = false;
      using (var ofd = new OpenFileDialog() { Multiselect = true })
      {
        if(ofd.ShowDialog() == DialogResult.OK)
        {
          try
          {
            if (LoadCons(ofd.FileNames))
            {
              groupBox2.Enabled = true;
              listBox1.Items.AddRange(conFilenames.ToArray());
            }
          }
          catch(Exception ex)
          {
            new ErrorWindow(
              "Oops! Couldn't load that CON!"+Environment.NewLine
              +"LibForge version: " + LibForge.Meta.BuildString+Environment.NewLine
              +"Error: " + ex.Message,
              "Error Parsing CON",
              "Stack Trace: " + ex.StackTrace).ShowDialog(this);
          }
        }
      }
    }

    private bool LoadCons(string[] filenames)
    {
      var dtas = new List<LibForge.SongData.SongData>();
      foreach (var filename in filenames)
      {

        using (var con = STFSPackage.OpenFile(GameArchives.Util.LocalFile(filename)))
        {
          if (con.Type != STFSType.CON)
          {
            MessageBox.Show("Error: given file was not a CON file");
            return false;
          }
          var datas = PkgCreator.GetSongMetadatas(con.RootDirectory.GetDirectory("songs"));
          if (datas.Count > 0)
          {
            dtas.AddRange(datas);
            conFilenames.Add(filename);
          }
        }
      }
      if (dtas.Count == 0)
      {
        MessageBox.Show("Error: no songs selected");
        return false;
      }
      idBox.Text = PkgCreator.GenId(dtas);
      descriptionBox.Text = PkgCreator.GenDesc(dtas);
      return true;
    }

    private void idBox_TextChanged(object sender, EventArgs e)
    {
      groupBox3.Enabled = idBox.Text.Length == 16;
      updateContentId();
    }

    private void updateContentId()
    {
      var txt = idBox.Text;
      contentIdTextBox.Text = euCheckBox.Checked ? $"EP8802-CUSA02901_00-{txt}" : $"UP8802-CUSA02084_00-{txt}";
    }

    private void euCheckBox_CheckedChanged(object sender, EventArgs e)
    {
      updateContentId();
    }

    private void buildButton_Click(object sender, EventArgs e)
    {
      using (var sfd = new SaveFileDialog() { FileName = contentIdTextBox.Text + ".pkg" })
      {
        if (sfd.ShowDialog() == DialogResult.OK)
        {
          Action<string> log = x => logBox.AppendText(x + Environment.NewLine);
          log("Converting DLC files...");
          var cons = conFilenames.Select(f => STFSPackage.OpenFile(GameArchives.Util.LocalFile(f)));
          var songs = new List<LibForge.DLCSong>();
          foreach (var con in cons)
          {
            songs.AddRange(PkgCreator.ConvertDLCPackage(
               con.RootDirectory.GetDirectory("songs"),
               volumeAdjustCheckBox.Checked,
               s => log($"Warning ({con.FileName}): " + s)));
          }
          log("Building PKG...");
          PkgCreator.BuildPkg(songs, contentIdTextBox.Text, descriptionBox.Text, euCheckBox.Checked, sfd.FileName, log);
          foreach(var con in cons)
          {
            con.Dispose();
          }
        }
      }
    }
  }
}
