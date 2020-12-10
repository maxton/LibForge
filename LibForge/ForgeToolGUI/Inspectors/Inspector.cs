using LibForge.CSV;
using LibForge.Lipsync;
using LibForge.Mesh;
using LibForge.Midi;
using LibForge.Milo;
using LibForge.RBSong;
using LibForge.SongData;
using LibForge.Texture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace ForgeToolGUI
{
  static class InspectorFactory
  {
    /// <summary>
    /// Returns an inspector for the object or null if there isn't one.
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static Inspector GetInspector(object obj)
    {
      switch (obj)
      {
        case Texture i:
          return new ImageInspector(TextureConverter.ToBitmap(i, 0));
        case string s:
          return new StringInspector(s);
        case SongData d:
          return new SongDataInspector(d);
        case RBMid m:
          return new RBMidiInspector(m);
        case HxMesh m:
          return new MeshInspector(m);
        case MiloFile mf:
          return new PropertyInspector(mf);
        case LibForge.Fuser.FuserAsset a:
          return new Inspectors.FuserInspector(a);
        case object o:
          return new ObjectInspector(obj);
      }
      return null;
    }


    public static object LoadObject(GameArchives.IFile i)
    {
      if (i.Name.Contains(".bmp_") || i.Name.Contains(".png_"))
      {
        using (var s = i.GetStream())
        {
          try
          {
            return TextureReader.ReadStream(s);
          }
          catch (Exception ex)
          {
            System.Windows.Forms.MessageBox.Show("Couldn't load texture: " + ex.Message);
            return null;
          }
        }
      }
      else if (i.Name.Contains("_dta_") || i.Name.EndsWith(".dtb"))
      {
        using (var s = i.GetStream())
        {
          var data = DtxCS.DTX.FromDtb(s);
          var sb = new StringBuilder();
          foreach (var x in data.Children)
          {
            sb.AppendLine(x.ToString(0));
          }
          return sb.ToString();
        }
      }
      else if (i.Name.EndsWith(".dta") || i.Name.EndsWith(".moggsong"))
      {
        using (var s = i.GetStream())
        using (var r = new System.IO.StreamReader(s))
        {
          return r.ReadToEnd();
        }
      }
      else if (i.Name.Contains(".songdta"))
      {
        using (var s = i.GetStream())
        {
          var songData = SongDataReader.ReadStream(s);
          return songData;
        }
      }
      else if (i.Name.Contains(".fbx"))
      {
        using (var s = i.GetStream())
        {
          return HxMeshReader.ReadStream(s);
        }
      }
      else if (i.Name.Contains(".rbmid_"))
      {
        using (var s = i.GetStream())
        {
          return RBMidReader.ReadStream(s);
        }
      }
      else if (i.Name.Contains(".lipsync"))
      {
        using (var s = i.GetStream())
        {
          return new LipsyncReader(s).Read();
        }
      }
      else if (i.Name.Contains(".rbsong"))
      {
        using (var s = i.GetStream())
        {
          var rbsong = new RBSongResource();
          rbsong.Load(s);
          return rbsong;
        }
      }
      else if (i.Name.Contains(".gp4"))
      {
        using (var s = i.GetStream())
        {
          return LibOrbisPkg.GP4.Gp4Project.ReadFrom(s);
        }
      }
      else if (i.Name.Contains(".pkg"))
      {
        using (var s = i.GetStream())
        {
          return new LibOrbisPkg.PKG.PkgReader(s).ReadHeader();
        }
      }
      else if(i.Name.EndsWith(".milo_xbox"))
      {
        using (var s = i.GetStream())
        {
          return MiloFile.ReadFromStream(s);
        }
      }
      else if(i.Name.Contains(".csv"))
      {
        using (var s = i.GetStream())
        {

          return CsvData.LoadFile(s).ToString();
        }
      }
      else if (i.Name.EndsWith(".uexp"))
      {
        using (var s = i.GetStream())
        {
          return LibForge.Fuser.FuserAsset.Read(s);
        }
      }
      else
      {
        return null;
      }
    }
  }

  public class Inspector : System.Windows.Forms.UserControl
  {
    protected ForgeBrowser fb;
    public void SetBrowser(ForgeBrowser f)
    {
      fb = f;
    }
  }
}
