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
      throw new NotImplementedException();
    }
  }
}
