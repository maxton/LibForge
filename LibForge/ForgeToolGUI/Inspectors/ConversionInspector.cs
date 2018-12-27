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
    private STFSPackage con;
    public ConversionInspector()
    {
      InitializeComponent();
    }

    private void pickFileButton_Click(object sender, EventArgs e)
    {
      using (var ofd = new OpenFileDialog())
      {
        if(ofd.ShowDialog() == DialogResult.OK)
        {
          try
          {
            if (LoadCon(ofd.FileName))
            {
              selectedFileLabel.Text = ofd.FileName;
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

    private bool LoadCon(string filename)
    {
      con = STFSPackage.OpenFile(GameArchives.Util.LocalFile(filename));
      if (con.Type != STFSType.CON)
      {
        MessageBox.Show("Error: given file was not a CON file");
        return false;
      }
      var songs = PkgCreator.ConvertDLCPackage(con.RootDirectory.GetDirectory("songs"));

      var shortname = new Regex("[^a-zA-Z0-9]").Replace(songs[0].SongData.Shortname, "");
      var pkgName = shortname.ToUpper().Substring(0, Math.Min(shortname.Length, 10)).PadRight(10, 'X');
      string pkgNum = (songs[0].SongData.SongId % 10000).ToString().PadLeft(4, '0');
      idBox.Text ="CU" + pkgName + pkgNum;
      descriptionBox.Text = $"Custom: \"{songs[0].SongData.Name} - {songs[0].SongData.Artist}\"";
      groupBox2.Enabled = true;
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
          var songs = PkgCreator.ConvertDLCPackage(con.RootDirectory.GetDirectory("songs"));
          log("Building PKG...");
          PkgCreator.BuildPkg(songs, contentIdTextBox.Text, descriptionBox.Text, euCheckBox.Checked, sfd.FileName, log);
        }
      }
    }
  }
}
