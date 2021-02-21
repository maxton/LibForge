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

    void ClearState()
    {
      listBox1.Items.Clear();
      dtas.Clear();
      UpdateState();
    }
    void UpdateState()
    {
      if (dtas.Count == 0)
      {
        groupBox2.Enabled = false;
        groupBox3.Enabled = false;
        idBox.Text = "";
        descriptionBox.Text = "";
        return;
      }
      var dtaList = dtas.Values.SelectMany(x => x).ToList();
      dtaList.Sort((a, b) => a.Shortname.CompareTo(b.Shortname));
      groupBox2.Enabled = true;
      idBox.Text = PkgCreator.GenId(dtaList);
      descriptionBox.Text = PkgCreator.GenDesc(dtaList);
    }

    private void pickFileButton_Click(object sender, EventArgs e)
    {
      using (var ofd = new OpenFileDialog() { Multiselect = true })
      {
        if(ofd.ShowDialog() == DialogResult.OK)
        {
          LoadCons(ofd.FileNames);
        }
      }
    }

    Dictionary<string, List<LibForge.SongData.SongData>> dtas = new Dictionary<string, List<LibForge.SongData.SongData>>();
    private bool LoadCon(string filename)
    {
      using (var con = STFSPackage.OpenFile(GameArchives.Util.LocalFile(filename)))
      {
        if (con.Type != STFSType.CON)
        {
          throw new Exception($"File is not a CON file.");
        }
        var datas = PkgCreator.GetSongMetadatas(con.RootDirectory.GetDirectory("songs"));
        if (datas.Count > 0)
        {
          dtas[filename] = datas;
          listBox1.Items.Add(filename);
        }
      }
      return true;
    }
    private void LoadCons(string[] filenames)
    {
      foreach (var filename in filenames)
      {
        try
        {
          LoadCon(filename);
        }
        catch (Exception e)
        {
          logBox.AppendText($"Error loading {filename}: {e.Message}" + Environment.NewLine);
        }
      }
      UpdateState();
    }
    void RemoveCon(string filename)
    {
      listBox1.Items.Remove(filename);
      dtas.Remove(filename);
    }

    private void idBox_TextChanged(object sender, EventArgs e)
    {
      updateContentId();
    }

    private void updateContentId()
    {
      var txt = new Regex("[^a-zA-Z0-9]").Replace(idBox.Text, "").ToUpper();
      contentIdTextBox.Text = euCheckBox.Checked ? $"EP8802-CUSA02901_00-{txt}" : $"UP8802-CUSA02084_00-{txt}";
      groupBox3.Enabled = contentIdTextBox.Text.Length == 36;
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
          var cons = listBox1.Items.OfType<string>().Select(f => STFSPackage.OpenFile(GameArchives.Util.LocalFile(f))).ToList();
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

    private void listBox1_DragEnter(object sender, DragEventArgs e)
    {
      e.Effect = DragDropEffects.Copy;
    }

    private void listBox1_DragDrop(object sender, DragEventArgs e)
    {
      if (e.Data.GetData(DataFormats.FileDrop) is string[] files)
      {
        LoadCons(files);
      }
    }

    private void clearButton_Click(object sender, EventArgs e)
    {
      ClearState();
    }

    private void listBox1_KeyUp(object sender, KeyEventArgs e)
    {
      if (e.KeyCode == Keys.Delete || e.KeyCode == Keys.Back)
      {
        RemoveCon(listBox1.SelectedItem as string);
        UpdateState();
      }
    }
  }
}
