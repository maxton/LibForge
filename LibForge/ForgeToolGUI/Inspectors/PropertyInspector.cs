using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using LibForge.RBSong;
using LibForge.Midi;
using System.Collections;

namespace ForgeToolGUI
{
  public partial class PropertyInspector : Inspector
  {
    public PropertyInspector(object obj)
    {
      InitializeComponent();
      ObjectPreview(obj);
    }

    object previewObj;
    void ObjectPreview(object obj)
    {
      propertyGrid1.SelectedObject = obj;
      previewObj = obj;
    }
  }
}
