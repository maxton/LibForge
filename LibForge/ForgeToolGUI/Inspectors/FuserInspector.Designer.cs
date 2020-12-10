
namespace ForgeToolGUI.Inspectors
{
  partial class FuserInspector
  {
    /// <summary> 
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary> 
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
      if (disposing && (components != null))
      {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Component Designer generated code

    /// <summary> 
    /// Required method for Designer support - do not modify 
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      this.components = new System.ComponentModel.Container();
      this.listBox1 = new System.Windows.Forms.ListBox();
      this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
      this.label1 = new System.Windows.Forms.Label();
      this.exportMidiButton = new System.Windows.Forms.Button();
      this.exportMoggButton = new System.Windows.Forms.Button();
      this.exportFusionButton = new System.Windows.Forms.Button();
      this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
      this.button1 = new System.Windows.Forms.Button();
      this.SuspendLayout();
      // 
      // listBox1
      // 
      this.listBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.listBox1.ContextMenuStrip = this.contextMenuStrip1;
      this.listBox1.FormattingEnabled = true;
      this.listBox1.Location = new System.Drawing.Point(3, 27);
      this.listBox1.Name = "listBox1";
      this.listBox1.Size = new System.Drawing.Size(485, 121);
      this.listBox1.TabIndex = 0;
      this.listBox1.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
      // 
      // contextMenuStrip1
      // 
      this.contextMenuStrip1.Name = "contextMenuStrip1";
      this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(3, 11);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(58, 13);
      this.label1.TabIndex = 1;
      this.label1.Text = "Resources";
      // 
      // exportMidiButton
      // 
      this.exportMidiButton.Enabled = false;
      this.exportMidiButton.Location = new System.Drawing.Point(6, 155);
      this.exportMidiButton.Name = "exportMidiButton";
      this.exportMidiButton.Size = new System.Drawing.Size(126, 23);
      this.exportMidiButton.TabIndex = 2;
      this.exportMidiButton.Text = "Export Midi";
      this.exportMidiButton.UseVisualStyleBackColor = true;
      this.exportMidiButton.Click += new System.EventHandler(this.exportMidiButton_Click);
      // 
      // exportMoggButton
      // 
      this.exportMoggButton.Enabled = false;
      this.exportMoggButton.Location = new System.Drawing.Point(6, 184);
      this.exportMoggButton.Name = "exportMoggButton";
      this.exportMoggButton.Size = new System.Drawing.Size(126, 23);
      this.exportMoggButton.TabIndex = 3;
      this.exportMoggButton.Text = "Export Mogg";
      this.exportMoggButton.UseVisualStyleBackColor = true;
      this.exportMoggButton.Click += new System.EventHandler(this.exportMoggButton_Click);
      // 
      // exportFusionButton
      // 
      this.exportFusionButton.Enabled = false;
      this.exportFusionButton.Location = new System.Drawing.Point(6, 213);
      this.exportFusionButton.Name = "exportFusionButton";
      this.exportFusionButton.Size = new System.Drawing.Size(126, 23);
      this.exportFusionButton.TabIndex = 4;
      this.exportFusionButton.Text = "Export Fusion Patch";
      this.exportFusionButton.UseVisualStyleBackColor = true;
      this.exportFusionButton.Click += new System.EventHandler(this.exportFusionButton_Click);
      // 
      // propertyGrid1
      // 
      this.propertyGrid1.Location = new System.Drawing.Point(138, 155);
      this.propertyGrid1.Name = "propertyGrid1";
      this.propertyGrid1.Size = new System.Drawing.Size(350, 215);
      this.propertyGrid1.TabIndex = 5;
      // 
      // button1
      // 
      this.button1.Enabled = false;
      this.button1.Location = new System.Drawing.Point(6, 242);
      this.button1.Name = "button1";
      this.button1.Size = new System.Drawing.Size(126, 35);
      this.button1.TabIndex = 6;
      this.button1.Text = "Export Fusion Patch (serialize)";
      this.button1.UseVisualStyleBackColor = true;
      this.button1.Click += new System.EventHandler(this.button1_Click);
      // 
      // FuserInspector
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.button1);
      this.Controls.Add(this.propertyGrid1);
      this.Controls.Add(this.exportFusionButton);
      this.Controls.Add(this.exportMoggButton);
      this.Controls.Add(this.exportMidiButton);
      this.Controls.Add(this.label1);
      this.Controls.Add(this.listBox1);
      this.Name = "FuserInspector";
      this.Size = new System.Drawing.Size(491, 373);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.ListBox listBox1;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
    private System.Windows.Forms.Button exportMoggButton;
    private System.Windows.Forms.Button exportFusionButton;
    private System.Windows.Forms.Button exportMidiButton;
    private System.Windows.Forms.PropertyGrid propertyGrid1;
    private System.Windows.Forms.Button button1;
  }
}
