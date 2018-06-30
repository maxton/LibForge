using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ForgeToolGUI
{
  public partial class ImageInspector : Inspector
  {
    public ImageInspector(Image i)
    {
      InitializeComponent();
      pictureBox1.Width = i.Width;
      pictureBox1.Height = i.Height;
      pictureBox1.Image = i;
    }
  }
}
