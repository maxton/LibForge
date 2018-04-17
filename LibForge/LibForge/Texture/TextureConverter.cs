using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using LibForge.Extensions;

namespace LibForge.Texture
{
  public class TextureConverter
  {
    static int RGB565ToARGB(ushort input)
    {
      return Color.FromArgb(0xFF,
        (((input >> 11) & 0x1F) * 0xFF / 0x1F),
        (((input >> 5) & 0x3F) * 0xFF / 0x3F),
        (((input >> 0) & 0x1F) * 0xFF / 0x1F)).ToArgb();
    }
    // TODO: Decode DXT5 alpha channel
    public static Bitmap ToBitmap(Texture t, int mipmap)
    {
      var m = t.Mipmaps[mipmap];
      var output = new Bitmap(m.Width, m.Height, PixelFormat.Format32bppArgb);
      int[] imageData = new int[m.Width * m.Height];
      if(m.Data.Length == (imageData.Length * 4))
      {
        Buffer.BlockCopy(m.Data, 0, imageData, 0, m.Data.Length);
      }
      else if(m.Data.Length == imageData.Length)
      {
        DecodeDXT(m, imageData, true);
      }
      else if (m.Data.Length == (imageData.Length / 2))
      {
        DecodeDXT(m, imageData, false);
      }
      else
      {
        throw new Exception($"Don't know what to do with this texture (version={t.Version})");
      }
      // Copy data to bitmap
      {
        var data = output.LockBits(new Rectangle(0, 0, m.Width, m.Height), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
        System.Runtime.InteropServices.Marshal.Copy(imageData, 0, data.Scan0, imageData.Length);
        output.UnlockBits(data);
      }
      return output;
    }

    private static void DecodeDXT(Texture.Mipmap m, int[] imageData, bool DXT5)
    {
      int[] colors = new int[4];
      using (var s = new MemoryStream(m.Data))
      {
        ushort[] c = new ushort[4];
        byte[] iData = new byte[4];
        for (var y = 0; y < m.Height; y += 4)
          for (var x = 0; x < m.Width; x += 4)
          {
            if (DXT5) s.Seek(8, SeekOrigin.Current);
            ushort c0 = s.ReadUInt16LE();
            ushort c1 = s.ReadUInt16LE();
            colors[0] = RGB565ToARGB(c0);
            colors[1] = RGB565ToARGB(c1);
            var color0 = Color.FromArgb(colors[0]);
            var color1 = Color.FromArgb(colors[1]);
            s.Read(iData, 0, 4);
            if (c0 > c1)
            {
              colors[2] = Color.FromArgb(0xFF,
                (color0.R * 2 + color1.R) / 3,
                (color0.G * 2 + color1.G) / 3,
                (color0.B * 2 + color1.B) / 3).ToArgb();
              colors[3] = Color.FromArgb(0xFF,
                (color0.R + (color1.R * 2)) / 3,
                (color0.G + (color1.G * 2)) / 3,
                (color0.B + (color1.B * 2)) / 3).ToArgb();
            }
            else
            {
              colors[2] = Color.FromArgb(0xFF,
                (color0.R + color1.R) / 2,
                (color0.G + color1.G) / 2,
                (color0.B + color1.B) / 2).ToArgb();
              colors[3] = 0;
            }
            var offset = y * m.Width + x;
            for (var i = 0; i < 4; i++)
            {
              for (var j = 0; j < 4; j++)
              {
                var idx = (iData[i] >> (2 * j)) & 0x3;
                imageData[offset + i * m.Width + j] = colors[idx];
              }
            }
          }
      }
    }
  }
}
