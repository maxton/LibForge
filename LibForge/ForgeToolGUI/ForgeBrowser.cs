using System;
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
      comboBox1.SelectedIndex = 3;
      pictureBox2.Image = new Bitmap(pictureBox2.Width, pictureBox2.Height);
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
      closeToolStripMenuItem.Enabled = true;
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
      closeToolStripMenuItem.Enabled = false;
      state.Loaded = false;
    }

    private void openToolStripMenuItem_Click(object sender, EventArgs e)
    {
      OpenFileDialog of = new OpenFileDialog();
      of.Filter = "Ark Header (*.hdr)|*.hdr|PFS Image (*.dat)|*.dat";
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
              fileTreeView.Select();
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
          else if(i.Name.Contains("_dta_") || i.Name.EndsWith(".dtb"))
          {
            using (var s = i.GetStream())
            {
              var data = DtxCS.DTX.FromDtb(s);
              tabControl1.SelectTab(1);
              fileTreeView.Select();
              var sb = new StringBuilder();
              foreach (var x in data.Children)
              {
                sb.AppendLine(x.ToString(0));
              }
              dataTextBox.Text = sb.ToString();
            }
          }
          else if(i.Name.EndsWith(".dta") || i.Name.EndsWith(".moggsong"))
          {
            using (var s = i.GetStream())
            using (var r = new System.IO.StreamReader(s))
            {
              tabControl1.SelectTab(1);
              fileTreeView.Select();
              dataTextBox.Text = r.ReadToEnd();
            }
          }
          else if(i.Name.Contains(".songdta"))
          {
            using (var s = i.GetStream())
            {
              var songData = SongDataReader.ReadStream(s);
              songDataInspector1.UpdateValues(songData);
              tabControl1.SelectTab(2);
              fileTreeView.Select();
            }
          }
          else if(i.Name.Contains(".fbx"))
          {
            using (var s = i.GetStream())
            {
              var mesh = HxMeshReader.ReadStream(s);
              meshTextBox.Text = HxMeshConverter.ToObj(mesh);
              tabControl1.SelectTab(3);
              fileTreeView.Select();
            }
          }
          else if(i.Name.Contains(".rbmid_"))
          {
            using (var s = i.GetStream())
            {
              var rbmid = RBMidReader.ReadStream(s);
              ObjectPreview(rbmid);
            }
          }
          else if(i.Name.Contains(".lipsync"))
          {
            using (var s = i.GetStream())
            {
              var lipsync = new LibForge.Lipsync.LipsyncReader(s).Read();
              ObjectPreview(lipsync);
            }
          }
          else if (i.Name.Contains(".rbsong"))
          {
            using (var s = i.GetStream())
            {
              var rbsong = new LibForge.RBSong.RBSongReader(s).Read();
              ObjectPreview(rbsong);
            }
          }
          break;
      }
    }

    object previewObj;
    void ObjectPreview(object obj)
    {
      treeView1.Nodes.Clear();
      AddObjectNodes(obj, treeView1.Nodes);
      tabControl1.SelectTab(4);
      fileTreeView.Select();
      previewObj = obj;
    }

    /// <summary>
    /// Adds the given object's public fields to the given TreeNodeCollection.
    /// </summary>
    void AddObjectNodes(object obj, TreeNodeCollection nodes)
    {
      if (obj == null) return;
      var fields = obj.GetType().GetFields();
      foreach(var f in fields)
      {
        if (f.IsLiteral) continue;
        if (f.FieldType.IsPrimitive || f.FieldType == typeof(string) || f.FieldType.IsEnum)
        {
          nodes.Add(f.Name + " = " + f.GetValue(obj).ToString());
        }
        else if(f.FieldType.IsArray)
        {
          AddArrayNodes(f.GetValue(obj) as Array, f.Name, nodes);
        }
        else
        {
          var node = new TreeNode(f.Name);
          AddObjectNodes(f.GetValue(obj), node.Nodes);
          nodes.Add(node);
        }
      }
    }

    void AddForgeVal(string name, Value value, TreeNodeCollection nodes)
    {
      if(value is StructValue)
      {
        var no = new TreeNode($"{name}: Struct");
        foreach (var x in (value as StructValue).Props)
        {
          AddForgeProp(x, no.Nodes);
        }
        nodes.Add(no);
      }
      else if(value is ArrayValue)
      {
        var arr = value as ArrayValue;
        var no = new TreeNode($"{name}: {(arr.Type as ArrayType).ElementType.InternalType}[] ({arr.Data.Length})");
        for(var i = 0; i < arr.Data.Length; i++)
        {
          AddForgeVal(name + "[" + i + "]", arr.Data[i], no.Nodes);
        }
        nodes.Add(no);
      }
      else if(value is PropRef)
      {
        var driv = value as PropRef;
        nodes.Add($"{name}: DrivenProp [{driv.ClassName} {driv.PropertyName}] ({driv.Unknown1},{driv.Unknown2}, {driv.Unknown3})");
      }
      else
      {
        var data = value.GetType().GetField("Data").GetValue(value);
        nodes.Add(name + ": " + value.Type.InternalType.ToString() + " = " + data.ToString());
      }
    }

    void AddForgeProp(Property prop, TreeNodeCollection nodes)
    {
      if (prop.Value == null) return;
      AddForgeVal(prop.Name, prop.Value, nodes);
    }

    /// <summary>
    /// Adds the given array to the given TreeNodeCollection.
    /// </summary>
    void AddArrayNodes(Array arr, string name, TreeNodeCollection nodes)
    {
      var node = new TreeNode($"{name} ({arr.Length})");
      var eType = arr.GetType().GetElementType();
      if (eType.IsPrimitive || eType == typeof(string) || eType.IsEnum)
        for (var i = 0; i < arr.Length; i++)
        {
          var n = new TreeNode($"{name}[{i}] = {arr.GetValue(i)}");
          node.Nodes.Add(n);
        }
      else for (var i = 0; i < arr.Length; i++)
        {
          var myName = $"{name}[{i}]";
          if (eType.IsArray)
            AddArrayNodes(arr.GetValue(i) as Array, myName, node.Nodes);
          else
          {
            var obj = arr.GetValue(i);

            if (obj is Property)
            {
              AddForgeProp(obj as Property, node.Nodes);
              continue;
            }
            if (obj is Value)
            {
              AddForgeVal(myName, obj as Value, node.Nodes);
              continue;
            }

            System.Reflection.FieldInfo nameField;
            if(null != (nameField = obj.GetType().GetField("Name")))
            {
              myName += $" (Name: {nameField.GetValue(obj)})";
            }
            var n = new TreeNode(myName);
            var item = arr.GetValue(i);
            AddObjectNodes(item, n.Nodes);
            if(item is RBMid.GEMTRACK)
            {
              n.Tag = item;
            }
            node.Nodes.Add(n);
          }
        }
      nodes.Add(node);
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

    private void treeView1_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
    {
      if (treeView1.SelectedNode.Tag is RBMid.GEMTRACK)
      {
        PreviewGemTrack((RBMid.GEMTRACK)treeView1.SelectedNode.Tag);
      }
    }

    private void PreviewGemTrack(RBMid.GEMTRACK track)
    {
      var lastGem = track.Gems[3][track.Gems[3].Length - 1];
      var length = lastGem.StartMillis + lastGem.LengthMillis;
      previewState = new GemTrackPreviewState
      {
        GemTrack = track,
        diff = comboBox1.SelectedIndex,
        scroll = 0,
        length = length,
        zoom = 1,
        enabled = true,
      };
      tabControl1.SelectedTab = tabPage6;
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
      using (var g = Graphics.FromImage(pictureBox2.Image))
      {
        g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
        g.Clear(Color.White);
        foreach (var gem in track)
        {
          var left = gem.StartMillis * scale - offset;
          if (left < 0)
            continue;
          if (left > pictureBox2.Image.Width)
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
      pictureBox2.Refresh();
    }

    private void ScrollGemTrack(int value)
    {
      previewState.scroll = value;
      RenderGemTrack();
    }

    private void hScrollBar1_Scroll(object sender, ScrollEventArgs e)
    {
      ScrollGemTrack(e.NewValue);
    }

    private void hScrollBar1_ValueChanged(object sender, EventArgs e)
    {
      ScrollGemTrack(hScrollBar1.Value);
    }

    private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
    {
      previewState.diff = comboBox1.SelectedIndex;
      RenderGemTrack();
    }

    private void pictureBox2_SizeChanged(object sender, EventArgs e)
    {
      if (pictureBox1.Width > 0 && pictureBox1.Height > 0)
      {
        pictureBox2.Image = new Bitmap(pictureBox2.Width, pictureBox2.Height);
        RenderGemTrack();
      }
    }

    private void trackBar1_Scroll(object sender, EventArgs e)
    {
      previewState.zoom = (float)Math.Pow(1.5f, trackBar1.Value/4f);
      RenderGemTrack();
    }
  }
  public class ForgeBrowserState
  {
    public bool Loaded = false;
    public GameArchives.AbstractPackage pkg;
    public GameArchives.IDirectory root;
  }
}
