using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using LibForge.Util;

namespace LibForge.Texture
{
  public class TextureReader : ReaderBase<Texture>
  {
    public static Texture ReadStream(Stream s)
    {
      return new TextureReader(s).Read();
    }
    public TextureReader(Stream s) : base(s) { }

    public override Texture Read()
    {
      Check(Int(), 6);
      s.Position = 0x80;
      var bpp = Int();
      s.Position = 0xB4;
      var MipmapLevels = UInt();
      var Mipmaps = FixedArr(() => new Texture.Mipmap
      {
        Width = Int(),
        Height = Int(),
        Flags = Int()
      }, MipmapLevels);
      Check(UInt(), 6u);
      for(var i = 0; i < Mipmaps.Length; i++)
      {
        Mipmaps[i].Data = Arr(Byte);
      }
      return new Texture
      {
        BitsPerPixel = bpp,
        Mipmaps = Mipmaps
      };
    }
  }
}
