using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ForgeToolGUI
{
  public partial class ErrorWindow : Form
  {
    public ErrorWindow(string message = "", string title = "Error", string details = "")
    {
      InitializeComponent();
      Text = title;
      label1.Text = message;
      textBox1.Text = details;
    }
  }
}
