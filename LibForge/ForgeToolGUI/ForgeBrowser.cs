using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
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
    private void SetTitle(string title = null)
    {
      Text = $"{((title?.Length ?? 0) == 0 ? "" : (title + " - "))}ForgeToolGUI (LibForge v{LibForge.Meta.BuildString})";
    }
    public ForgeBrowser()
    {
      state = new ForgeBrowserState();
      InitializeComponent();
      SetTitle();
      var args = Environment.GetCommandLineArgs();
      if (args.Length > 1)
      {
        if (File.Exists(args[1]))
        {
          var file = GameArchives.Util.LocalFile(args[1]);
          if (GameArchives.PFS.PFSPackage.IsPFS(file) != GameArchives.PackageTestResult.NO
            || GameArchives.Ark.ArkPackage.IsArk(file) != GameArchives.PackageTestResult.NO
            || GameArchives.STFS.STFSPackage.IsSTFS(file) != GameArchives.PackageTestResult.NO)
          {
            LoadPackage(args[1]);
          }
          else
          {
            OpenFile(file);
          }
        }
        else if (Directory.Exists(args[1]))
        {
          LoadFolder(args[1]);
        }
      }
    }

    private void LoadPackage(string filename)
    {
      if (state.Loaded) Unload();
      var pkgFile = GameArchives.Util.LocalFile(filename);

      
      if(filename.EndsWith(".pkg"))
      {
        try
        {
          string contentId;
          using (var tempS = pkgFile.GetStream())
          {
            var hdr = new LibOrbisPkg.PKG.PkgReader(tempS).ReadHeader();
            contentId = hdr.content_id;
          }
          state.pkg = GameArchives.PackageReader.ReadPackageFromFile(pkgFile,
            new string(LibOrbisPkg.Util.Crypto.ComputeKeys(
              contentId, "00000000000000000000000000000000", 1)
              .Select(b => (char)b).ToArray()));
        }
        catch (Exception) { }
      }
      else
      {
        state.pkg = GameArchives.PackageReader.ReadPackageFromFile(pkgFile);
      }
      if(state.pkg is GameArchives.PFS.PFSPackage)
      {
        if(state.pkg.RootDirectory.TryGetFile("pfs_image.dat", out var f))
        {
          state.dispose = state.pkg.Dispose;
          state.pkg = GameArchives.PackageReader.ReadPackageFromFile(f);
        }
        if(state.pkg.RootDirectory.TryGetFile("main_ps4.hdr", out var f2))
        {
          state.dispose = state.pkg.Dispose;
          state.pkg = GameArchives.PackageReader.ReadPackageFromFile(f2);
        }
      }
      state.root = state.pkg.RootDirectory;
      SetTitle(filename);
      FinishLoad();
    }

    private void LoadFolder(string path)
    {
      if (state.Loaded) Unload();
      state.root = GameArchives.Util.LocalDir(path);
      SetTitle(path);
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
      SetTitle();
      fileTreeView.Nodes.Clear();
      state.root = null;
      (state.dispose ?? state.pkg.Dispose)?.Invoke();
      state.dispose = null;
      state.pkg = null;
      closePackageMenuItem.Enabled = false;
      state.Loaded = false;
    }

    private void openToolStripMenuItem_Click(object sender, EventArgs e)
    {
      OpenFileDialog of = new OpenFileDialog();
      of.Filter = "Supported Packages (*.hdr, *.dat, *.pkg, *_rb3con)|*.hdr;*.dat;*.pkg;*_rb3con|All files|*.*";
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
          try
          {
            OpenFile(i);
          }
          catch(Exception ex)
          {
            MessageBox.Show("Couldn't load file: " + ex.Message);
          }
          break;
      }
    }

    public void OpenFile(GameArchives.IFile i)
    {
      var inspector = InspectorFactory.GetInspector(InspectorFactory.LoadObject(i));
      if (inspector != null)
      {
        OpenTab(inspector, i.Name);
        fileTreeView.Select();
      }
    }

    public void OpenTab(Inspector c, string name)
    {
      var x = new TabPage(name);
      c.Name = name;
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

    private void convertCONToPKGToolStripMenuItem_Click(object sender, EventArgs e)
    {
      OpenTab(new Inspectors.ConversionInspector(), "CON to PKG Conversion");
    }
  }
  public class ForgeBrowserState
  {
    public bool Loaded = false;
    public GameArchives.AbstractPackage pkg;
    public GameArchives.IDirectory root;
    public Action dispose;
  }
}
