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
  public partial class StringInspector : Inspector
  {
    public StringInspector(string str = "")
    {
      InitializeComponent();
      textBox.Text = str;
    }
  }
}
