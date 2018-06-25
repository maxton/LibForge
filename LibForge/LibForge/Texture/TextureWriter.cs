using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using LibForge.Util;

namespace LibForge.Texture
{
  public class TextureWriter : WriterBase<Texture>
  {
    public static void WriteStream(Texture r, Stream s)
    {
      new TextureWriter(s).WriteStream(r);
    }
    private TextureWriter(Stream s) : base(s) { }
    public override void WriteStream(Texture r)
    {
      Write(r.Version);
      s.Write(r.HeaderData, 0, r.HeaderData.Length);
      Write(r.Mipmaps, level =>
      {
        Write(level.Width);
        Write(level.Height);
        Write(level.Flags);
      });
      Write(6);
      foreach(var map in r.Mipmaps)
      {
        Write(map.Data.Length);
        s.Write(map.Data, 0, map.Data.Length);
      }
      s.Write(r.FooterData, 0, r.FooterData.Length);
    }
  }
}
