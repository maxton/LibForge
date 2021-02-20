using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using LibForge.Engine;
using LibForge.RBSong;
using LibForge.Midi;
using System.Collections;

namespace ForgeToolGUI
{
  public partial class ObjectInspector : Inspector
  {
    public ObjectInspector(object obj)
    {
      InitializeComponent();
      ObjectPreview(obj);
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
          var val = f.GetValue(obj);
          if (val != null)
          {
            nodes.Add(f.Name + " = " + val.ToString());
          }
        }
        else if (f.FieldType.IsArray)
        {
          AddArrayNodes(f.GetValue(obj) as Array, f.Name, nodes);
        }
        else if (f.FieldType.IsGenericType && f.FieldType.GetGenericTypeDefinition() == typeof(List<>))
        {
          var internalType = f.FieldType.GetGenericArguments()[0];
          AddArrayNodes((f.GetValue(obj) as IList).Cast<object>().ToArray(), f.Name, nodes);
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
      if (value is StructValue sv)
      {
        var no = new TreeNode($"{name}: Struct");
        no.Tag = new Action(() =>
        {
          no.Nodes.Clear();
          foreach (var x in sv.Props)
          {
            AddForgeProp(x, no.Nodes);
          }
        });
        no.Nodes.Add("Loading...");
        nodes.Add(no);
      }
      else if (value is ArrayValue arr)
      {
        var no = new TreeNode($"{name}: {(arr.Type as ArrayType).ElementType.InternalType}[] ({arr.Data.Length})");
        no.Tag = new Action(() =>
        {
          no.Nodes.Clear();
          for (var i = 0; i < arr.Data.Length; i++)
          {
            AddForgeVal(name + "[" + i + "]", arr.Data[i], no.Nodes);
          }
        });
        no.Nodes.Add("Loading...");
        nodes.Add(no);
      }
      else if (value is DrivenProp)
      {
        var driv = value as DrivenProp;
        nodes.Add($"{name}: DrivenProp [{driv.ClassName} {driv.PropertyName}] ({driv.Unknown1},{driv.Unknown2}, {driv.Unknown3})");
      }
      else if (value is ColorValue c)
      {
        nodes.Add($"{name}: Color ({c.R},{c.G},{c.B},{c.A}) [{c.Unk1},{c.Unk2},{c.Unk3},{c.Unk4}]");
      }
      else
      {
        var data = value.GetType().GetField("Data")?.GetValue(value);
        nodes.Add(name + ": " + value.Type.InternalType.ToString() + " = " + data);
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
      if (arr == null)
      {
        nodes.Add(new TreeNode($"{name} (null)"));
        return;
      }
      var node = new TreeNode($"{name} ({arr.Length})");
      node.Tag = new Action(() => {
        node.Nodes.Clear();
        var eType = arr.GetType().GetElementType();
        if (eType.IsPrimitive || eType == typeof(string) || eType.IsEnum)
        {
          var nodeList = new TreeNode[arr.Length];
          for (var i = 0; i < arr.Length; i++)
          {
            nodeList[i] = new TreeNode($"{name}[{i}] = {arr.GetValue(i)}");
          }
          node.Nodes.AddRange(nodeList);
        }
        else
        {
          for (var i = 0; i < arr.Length; i++)
          {
            var myName = $"{name}[{i}]";
            if (eType.IsArray)
              AddArrayNodes(arr.GetValue(i) as Array, myName, node.Nodes);
            else
            {
              var obj = arr.GetValue(i);
              if (obj == null)
                continue;
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
              node.Nodes.Add(n);
            }
          }
        }
      });
      node.Nodes.Add("Loading...");
      nodes.Add(node);
    }

    private void TreeView1_AfterExpand(object sender, TreeViewEventArgs e)
    {
      if (e.Node.Tag is Action x)
      {
        x.Invoke();
        e.Node.Tag = null;
      }
    }
  }
}
