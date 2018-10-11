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
      var magic = Int();
      if(magic != 6 && magic != 4)
      {
        throw new Exception($"Unknown texture magic {magic}");
      }
      var version = Int();
      var hdrData = magic == 6 ? FixedArr(Byte, version == 0xC ? 0x7Cu : 0xACu) : FixedArr(Byte, 0xA8);
      var MipmapLevels = UInt();
      var Mipmaps = FixedArr(() => new Texture.Mipmap
      {
        Width = Int(),
        Height = Int(),
        Flags = version == 0xC ? Int().Then(Skip(8)) : Int()
      }, MipmapLevels);
      UInt();
      for(var i = 0; i < Mipmaps.Length; i++)
      {
        Mipmaps[i].Data = Arr(Byte);
      }
      var footerData = FixedArr(Byte, 0x1C);
      return new Texture
      {
        Version = magic,
        Mipmaps = Mipmaps,
        HeaderData = hdrData,
        FooterData = footerData
      };
    }
  }
}
