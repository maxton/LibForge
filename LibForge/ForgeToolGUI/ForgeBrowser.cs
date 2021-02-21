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
        LoadAnything(args[1]);
      } else
      {
        OpenTab(new Inspectors.StartupInspector(), "Welcome");
      }
    }

    private void LoadAnything(string filename)
    {
      if (File.Exists(filename))
      {
        var file = GameArchives.Util.LocalFile(filename);
        if (GameArchives.PFS.PFSPackage.IsPFS(file) != GameArchives.PackageTestResult.NO
          || GameArchives.Ark.ArkPackage.IsArk(file) != GameArchives.PackageTestResult.NO
          || GameArchives.STFS.STFSPackage.IsSTFS(file) != GameArchives.PackageTestResult.NO)
        {
          LoadPackage(filename);
        }
        else
        {
          OpenFile(file);
        }
      }
      else if (Directory.Exists(filename))
      {
        LoadFolder(filename);
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

    public void openPackage_Click(object sender, EventArgs e)
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


    public void openFolder_Click(object sender, EventArgs e)
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

    public void openFile_Click(object sender, EventArgs e)
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
    public void OpenConverter()
    {
      OpenTab(new Inspectors.ConversionInspector(), "CON to PKG Conversion");
    }
    private void convertCONToPKGToolStripMenuItem_Click(object sender, EventArgs e)
    {
      OpenConverter();
    }

    private void fileTreeView_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
    {
      ActiveNode = e.Node;
      if (e.Button != MouseButtons.Right)
        return;
      extractToolStripMenuItem.Enabled = e.Node.Tag is GameArchives.IFile;
    }

    private void SaveFile(string prompt, Action<Stream> fileWriter, string fileTypes = null, string defaultPath = null)
    {
      using (var sfd = new SaveFileDialog() { Title = prompt, Filter = fileTypes, FileName = defaultPath })
      {
        if(sfd.ShowDialog() == DialogResult.OK)
        {
          using (var f = File.OpenWrite(sfd.FileName))
          {
            fileWriter(f);
          }
        }
      }
    }

    TreeNode ActiveNode = null;
    private void extractToolStripMenuItem_Click(object sender, EventArgs e)
    {
      if (ActiveNode?.Tag is GameArchives.IFile f)
      {
        SaveFile(
          "Extract file...",
          s => {
            using (var stream = f.GetStream()) stream.CopyTo(s);
          },
          defaultPath: f.Name);
      }
    }

    private void openToolStripMenuItem_Click_1(object sender, EventArgs e)
    {
      if (ActiveNode == null) return;
      if (ActiveNode.GetNodeCount(false) > 0)
      {
        ActiveNode.Expand();
      }
      else if (ActiveNode.Tag is GameArchives.IFile f)
      {
        OpenFile(f);
      }
    }

    private void ForgeBrowser_DragOver(object sender, DragEventArgs e)
    {
      e.Effect = DragDropEffects.Copy;
    }

    private void ForgeBrowser_DragDrop(object sender, DragEventArgs e)
    {
      if (e.Data.GetData(DataFormats.FileDrop) is string[] files)
      {
        foreach(var f in files)
        {
          LoadAnything(f);
        }
      }
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
