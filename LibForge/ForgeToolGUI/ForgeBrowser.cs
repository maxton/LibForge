using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using LibForge.Mesh;
using LibForge.Midi;
using LibForge.RBSong;
using LibForge.SongData;
using LibForge.Texture;

namespace ForgeToolGUI
{
  public partial class ForgeBrowser : Form
  {
    public ForgeBrowserState state;
    public ForgeBrowser()
    {
      state = new ForgeBrowserState();
      InitializeComponent();
    }

    private void LoadPackage(string filename)
    {
      if (state.Loaded) Unload();
      state.pkg = GameArchives.PackageReader.ReadPackageFromFile(GameArchives.Util.LocalFile(filename));
      state.root = state.pkg.RootDirectory;
      FinishLoad();
    }

    private void LoadFolder(string path)
    {
      if (state.Loaded) Unload();
      state.root = GameArchives.Util.LocalDir(path);
      FinishLoad();
    }

    private void FinishLoad()
    {
      closePackageMenuItem.Enabled = true;
      void AddNodes(GameArchives.IDirectory dir, TreeNodeCollection nodes)
      {
        foreach (var d in dir.Dirs)
        {
          var node = new TreeNode(d.Name);
          nodes.Add(node);
          AddNodes(d, node.Nodes);
        }
        foreach (var f in dir.Files)
        {
          var node = new TreeNode(f.Name);
          node.Tag = f;
          nodes.Add(node);
        }
      }
      AddNodes(state.root, fileTreeView.Nodes);
      state.Loaded = true;
    }

    private void Unload()
    {
      if (!state.Loaded) return;
      fileTreeView.Nodes.Clear();
      state.root = null;
      if (state.pkg != null)
      {
        state.pkg.Dispose();
        state.pkg = null;
      }
      closePackageMenuItem.Enabled = false;
      state.Loaded = false;
    }

    private void openToolStripMenuItem_Click(object sender, EventArgs e)
    {
      OpenFileDialog of = new OpenFileDialog();
      of.Filter = "Supported Packages (*.hdr, *.dat)|*.hdr;*.dat";
      if(of.ShowDialog(this) == DialogResult.OK)
      {
        LoadPackage(of.FileName);
      }
    }

    private void closeToolStripMenuItem_Click(object sender, EventArgs e)
    {
      Unload();
    }

    private void exitToolStripMenuItem_Click(object sender, EventArgs e)
    {
      Application.Exit();
    }

    private void previewSelectedNode(object sender, EventArgs e)
    {
      switch (fileTreeView.SelectedNode?.Tag)
      {
        case null:
          break;
        case GameArchives.IFile i:
          OpenFile(i);
          break;
      }
    }

    public void OpenFile(GameArchives.IFile i)
    {
      var inspector = InspectorFactory.GetInspector(InspectorFactory.LoadObject(i));
      if (inspector != null)
      {
        OpenTab(inspector, i.Name);
      }
      fileTreeView.Select();
    }

    public void OpenTab(Inspector c, string name)
    {
      var x = new TabPage(name);
      x.Controls.Add(c);
      c.SetBrowser(this);
      c.Dock = DockStyle.Fill;
      tabControl1.TabPages.Add(x);
      tabControl1.SelectedTab = x;
    }


    private void toolStripMenuItem1_Click(object sender, EventArgs e)
    {
      // Sorry.
      var of = new FolderBrowserDialog();
      of.ShowNewFolderButton = false;
      if(of.ShowDialog(this) == DialogResult.OK)
      {
        LoadFolder(of.SelectedPath);
      }
    }

    private void tabControl1_MouseClick(object sender, MouseEventArgs e)
    {
      if(e.Button == MouseButtons.Middle)
      {
        var tab =
          tabControl1.TabPages.Cast<TabPage>()
            .Where((t, i) => tabControl1.GetTabRect(i).Contains(e.Location))
            .FirstOrDefault();
        if (tab != null)
          tabControl1.TabPages.Remove(tab);
      }
    }

    private void fileTreeView_KeyPress(object sender, KeyPressEventArgs e)
    {
      if(e.KeyChar == '\r')
      {
        previewSelectedNode(sender, e);
        e.Handled = true;
      }
    }

    private void toolStripMenuItem2_Click(object sender, EventArgs e)
    {
      OpenFileDialog of = new OpenFileDialog();
      if (of.ShowDialog(this) == DialogResult.OK)
      {
        OpenFile(GameArchives.Util.LocalFile(of.FileName));
      }
    }

    private void toolStripMenuItem3_Click(object sender, EventArgs e)
    {
      if (tabControl1.SelectedTab != null)
        tabControl1.TabPages.Remove(tabControl1.SelectedTab);
    }
  }
  public class ForgeBrowserState
  {
    public bool Loaded = false;
    public GameArchives.AbstractPackage pkg;
    public GameArchives.IDirectory root;
  }
}
