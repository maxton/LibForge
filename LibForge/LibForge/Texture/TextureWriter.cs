using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using LibForge.Util;

namespace LibForge.Texture
{
  public class TextureWriter : WriterBase
  {
    public static void WriteStream(Texture r, Stream s)
    {
      new TextureWriter(s).WriteStream(r);
    }
    private TextureWriter(Stream s) : base(s) { }
    private void WriteStream(Texture r)
    {
      throw new NotImplementedException();
    }
  }
}
