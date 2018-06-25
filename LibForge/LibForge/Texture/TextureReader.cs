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
      var version = Int();
      if(version != 6 && version != 4)
      {
        throw new Exception($"Unknown texture version {version}");
      }
      var hdrData = version == 6 ? FixedArr(Byte, 0xB0) : FixedArr(Byte, 0xA8);
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
      var footerData = FixedArr(Byte, 0x1C);
      return new Texture
      {
        Version = version,
        Mipmaps = Mipmaps,
        HeaderData = hdrData,
        FooterData = footerData
      };
    }
  }
}
