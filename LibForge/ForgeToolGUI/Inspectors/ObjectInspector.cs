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

namespace ForgeToolGUI
{
  public partial class ObjectInspector : Inspector
  {
    public ObjectInspector(object obj)
    {
      InitializeComponent();
      ObjectPreview(obj);
    }

    private void treeView1_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
    {
      if (treeView1.SelectedNode.Tag is RBMid.GEMTRACK)
      {
        PreviewGemTrack((RBMid.GEMTRACK)treeView1.SelectedNode.Tag);
      }
    }

    private void PreviewGemTrack(RBMid.GEMTRACK track)
    {
      if (fb == null) return;
      var inspector = InspectorFactory.GetInspector(track);
      if(inspector != null)
      {
        fb.OpenTab(inspector, "Gems: " + Parent.Name);
        (inspector as GemTrackInspector).PreviewGemTrack(track);
      }
    }

    object previewObj;
    void ObjectPreview(object obj)
    {
      treeView1.Nodes.Clear();
      AddObjectNodes(obj, treeView1.Nodes);
      previewObj = obj;
    }

    /// <summary>
    /// Adds the given object's public fields to the given TreeNodeCollection.
    /// </summary>
    void AddObjectNodes(object obj, TreeNodeCollection nodes)
    {
      if (obj == null) return;
      var fields = obj.GetType().GetFields();
      foreach (var f in fields)
      {
        if (f.IsLiteral) continue;
        if (f.FieldType.IsPrimitive || f.FieldType == typeof(string) || f.FieldType.IsEnum)
        {
          nodes.Add(f.Name + " = " + f.GetValue(obj).ToString());
        }
        else if (f.FieldType.IsArray)
        {
          AddArrayNodes(f.GetValue(obj) as Array, f.Name, nodes);
        }
        else
        {
          var node = new TreeNode(f.Name);
          AddObjectNodes(f.GetValue(obj), node.Nodes);
          nodes.Add(node);
        }
      }
    }

    void AddForgeVal(string name, Value value, TreeNodeCollection nodes)
    {
      if (value is StructValue)
      {
        var no = new TreeNode($"{name}: Struct");
        foreach (var x in (value as StructValue).Props)
        {
          AddForgeProp(x, no.Nodes);
        }
        nodes.Add(no);
      }
      else if (value is ArrayValue)
      {
        var arr = value as ArrayValue;
        var no = new TreeNode($"{name}: {(arr.Type as ArrayType).ElementType.InternalType}[] ({arr.Data.Length})");
        for (var i = 0; i < arr.Data.Length; i++)
        {
          AddForgeVal(name + "[" + i + "]", arr.Data[i], no.Nodes);
        }
        nodes.Add(no);
      }
      else if (value is PropRef)
      {
        var driv = value as PropRef;
        nodes.Add($"{name}: DrivenProp [{driv.ClassName} {driv.PropertyName}] ({driv.Unknown1},{driv.Unknown2}, {driv.Unknown3})");
      }
      else
      {
        var data = value.GetType().GetField("Data").GetValue(value);
        nodes.Add(name + ": " + value.Type.InternalType.ToString() + " = " + data.ToString());
      }
    }

    void AddForgeProp(Property prop, TreeNodeCollection nodes)
    {
      if (prop.Value == null) return;
      AddForgeVal(prop.Name, prop.Value, nodes);
    }

    /// <summary>
    /// Adds the given array to the given TreeNodeCollection.
    /// </summary>
    void AddArrayNodes(Array arr, string name, TreeNodeCollection nodes)
    {
      var node = new TreeNode($"{name} ({arr.Length})");
      var eType = arr.GetType().GetElementType();
      if (eType.IsPrimitive || eType == typeof(string) || eType.IsEnum)
        for (var i = 0; i < arr.Length; i++)
        {
          var n = new TreeNode($"{name}[{i}] = {arr.GetValue(i)}");
          node.Nodes.Add(n);
        }
      else for (var i = 0; i < arr.Length; i++)
        {
          var myName = $"{name}[{i}]";
          if (eType.IsArray)
            AddArrayNodes(arr.GetValue(i) as Array, myName, node.Nodes);
          else
          {
            var obj = arr.GetValue(i);

            if (obj is Property)
            {
              AddForgeProp(obj as Property, node.Nodes);
              continue;
            }
            if (obj is Value)
            {
              AddForgeVal(myName, obj as Value, node.Nodes);
              continue;
            }

            System.Reflection.FieldInfo nameField;
            if (null != (nameField = obj.GetType().GetField("Name")))
            {
              myName += $" (Name: {nameField.GetValue(obj)})";
            }
            var n = new TreeNode(myName);
            var item = arr.GetValue(i);
            AddObjectNodes(item, n.Nodes);
            if (item is RBMid.GEMTRACK)
            {
              n.Tag = item;
            }
            node.Nodes.Add(n);
          }
        }
      nodes.Add(node);
    }
  }
}
