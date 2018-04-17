using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using LibForge.Texture;

namespace ForgeToolGUI
{
  public partial class ArkFileBrowser : Form
  {
    public ArkFileBrowserState state;
    public ArkFileBrowser()
    {
      state = new ArkFileBrowserState();
      InitializeComponent();
    }

    private void LoadArk(string filename)
    {
      if (state.ark != null) CloseArk();
      state.ark = GameArchives.Ark.ArkPackage.OpenFile(GameArchives.Util.LocalFile(filename));
      closeToolStripMenuItem.Enabled = true;
      void AddNodes(GameArchives.IDirectory dir, TreeNodeCollection nodes)
      {
        foreach (var d in dir.Dirs)
        {
          var node = new TreeNode(d.Name);
          nodes.Add(node);
          AddNodes(d, node.Nodes);
        }
        foreach(var f in dir.Files)
        {
          var node = new TreeNode(f.Name);
          node.Tag = f;
          nodes.Add(node);
        }
      }
      AddNodes(state.ark.RootDirectory, treeView1.Nodes);
    }

    private void CloseArk()
    {
      if (state.ark == null) return;
      treeView1.Nodes.Clear();
      state.ark.Dispose();
      state.ark = null;
      closeToolStripMenuItem.Enabled = false;
    }

    private void openToolStripMenuItem_Click(object sender, EventArgs e)
    {
      OpenFileDialog of = new OpenFileDialog();
      of.Filter = "Ark Header (*.hdr)|*.hdr";
      if(of.ShowDialog(this) == DialogResult.OK)
      {
        LoadArk(of.FileName);
      }
    }

    private void closeToolStripMenuItem_Click(object sender, EventArgs e)
    {
      CloseArk();
    }

    private void exitToolStripMenuItem_Click(object sender, EventArgs e)
    {
      Application.Exit();
    }

    private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
    {
      switch (e.Node.Tag)
      {
        case null:
          break;
        case GameArchives.IFile i:
          if (i.Name.Contains(".bmp_") || i.Name.Contains(".png_"))
          {
            using (var s = i.GetStream())
            {
              var tex = TextureReader.ReadStream(s);
              pictureBox1.Width = tex.Mipmaps[0].Width;
              pictureBox1.Height = tex.Mipmaps[0].Height;
              tabControl1.SelectTab(0);
              treeView1.Select();
              try
              {
                pictureBox1.Image = TextureConverter.ToBitmap(tex, 0);
              }
              catch(Exception ex)
              {
                MessageBox.Show("Couldn't load texture: " + ex.Message);
              }
            }
          }
          else if(i.Name.Contains("_dta_"))
          {
            using (var s = i.GetStream())
            {
              var data = DtxCS.DTX.FromDtb(s);
              tabControl1.SelectTab(1);
              treeView1.Select();
              var sb = new StringBuilder();
              foreach (var x in data.Children)
              {
                sb.AppendLine(x.ToString(0));
              }
              dataTextBox.Text = sb.ToString();
            }
          }
          else if(i.Name.Contains(".songdta"))
          {
            using (var s = i.GetStream())
            {
              var songData = LibForge.SongData.SongDataReader.ReadStream(s);
              songDataInspector1.UpdateValues(songData);
              tabControl1.SelectTab(2);
              treeView1.Select();
            }
          }
          break;
      }
    }
  }
  public class ArkFileBrowserState
  {
    public GameArchives.Ark.ArkPackage ark;
  }
}
