namespace ForgeToolGUI.Inspectors
{
  partial class ConversionInspector
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
      this.pickFileButton = new System.Windows.Forms.Button();
      this.groupBox1 = new System.Windows.Forms.GroupBox();
      this.clearButton = new System.Windows.Forms.Button();
      this.listBox1 = new System.Windows.Forms.ListBox();
      this.groupBox2 = new System.Windows.Forms.GroupBox();
      this.volumeAdjustCheckBox = new System.Windows.Forms.CheckBox();
      this.euCheckBox = new System.Windows.Forms.CheckBox();
      this.label3 = new System.Windows.Forms.Label();
      this.label2 = new System.Windows.Forms.Label();
      this.descriptionBox = new System.Windows.Forms.TextBox();
      this.idBox = new System.Windows.Forms.TextBox();
      this.groupBox3 = new System.Windows.Forms.GroupBox();
      this.buildButton = new System.Windows.Forms.Button();
      this.contentIdTextBox = new System.Windows.Forms.Label();
      this.label4 = new System.Windows.Forms.Label();
      this.label5 = new System.Windows.Forms.Label();
      this.logBox = new System.Windows.Forms.TextBox();
      this.groupBox1.SuspendLayout();
      this.groupBox2.SuspendLayout();
      this.groupBox3.SuspendLayout();
      this.SuspendLayout();
      // 
      // pickFileButton
      // 
      this.pickFileButton.Location = new System.Drawing.Point(6, 19);
      this.pickFileButton.Name = "pickFileButton";
      this.pickFileButton.Size = new System.Drawing.Size(75, 23);
      this.pickFileButton.TabIndex = 0;
      this.pickFileButton.Text = "Pick File(s)";
      this.pickFileButton.UseVisualStyleBackColor = true;
      this.pickFileButton.Click += new System.EventHandler(this.pickFileButton_Click);
      // 
      // groupBox1
      // 
      this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.groupBox1.Controls.Add(this.clearButton);
      this.groupBox1.Controls.Add(this.listBox1);
      this.groupBox1.Controls.Add(this.pickFileButton);
      this.groupBox1.Location = new System.Drawing.Point(4, 3);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new System.Drawing.Size(473, 205);
      this.groupBox1.TabIndex = 1;
      this.groupBox1.TabStop = false;
      this.groupBox1.Text = "Step 1: Pick CON(s)";
      // 
      // clearButton
      // 
      this.clearButton.Location = new System.Drawing.Point(86, 19);
      this.clearButton.Name = "clearButton";
      this.clearButton.Size = new System.Drawing.Size(75, 23);
      this.clearButton.TabIndex = 4;
      this.clearButton.Text = "Clear";
      this.clearButton.UseVisualStyleBackColor = true;
      this.clearButton.Click += new System.EventHandler(this.clearButton_Click);
      // 
      // listBox1
      // 
      this.listBox1.AllowDrop = true;
      this.listBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.listBox1.FormattingEnabled = true;
      this.listBox1.Location = new System.Drawing.Point(6, 48);
      this.listBox1.Name = "listBox1";
      this.listBox1.Size = new System.Drawing.Size(461, 147);
      this.listBox1.TabIndex = 3;
      this.listBox1.DragDrop += new System.Windows.Forms.DragEventHandler(this.listBox1_DragDrop);
      this.listBox1.DragEnter += new System.Windows.Forms.DragEventHandler(this.listBox1_DragEnter);
      this.listBox1.KeyUp += new System.Windows.Forms.KeyEventHandler(this.listBox1_KeyUp);
      // 
      // groupBox2
      // 
      this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.groupBox2.Controls.Add(this.volumeAdjustCheckBox);
      this.groupBox2.Controls.Add(this.euCheckBox);
      this.groupBox2.Controls.Add(this.label3);
      this.groupBox2.Controls.Add(this.label2);
      this.groupBox2.Controls.Add(this.descriptionBox);
      this.groupBox2.Controls.Add(this.idBox);
      this.groupBox2.Enabled = false;
      this.groupBox2.Location = new System.Drawing.Point(4, 214);
      this.groupBox2.Name = "groupBox2";
      this.groupBox2.Size = new System.Drawing.Size(473, 112);
      this.groupBox2.TabIndex = 2;
      this.groupBox2.TabStop = false;
      this.groupBox2.Text = "Step 2: Choose Options";
      // 
      // volumeAdjustCheckBox
      // 
      this.volumeAdjustCheckBox.AutoSize = true;
      this.volumeAdjustCheckBox.Checked = true;
      this.volumeAdjustCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
      this.volumeAdjustCheckBox.Location = new System.Drawing.Point(193, 90);
      this.volumeAdjustCheckBox.Name = "volumeAdjustCheckBox";
      this.volumeAdjustCheckBox.Size = new System.Drawing.Size(147, 17);
      this.volumeAdjustCheckBox.TabIndex = 6;
      this.volumeAdjustCheckBox.Text = "Adjust audio mix for RB4?";
      this.volumeAdjustCheckBox.UseVisualStyleBackColor = true;
      // 
      // euCheckBox
      // 
      this.euCheckBox.AutoSize = true;
      this.euCheckBox.Location = new System.Drawing.Point(86, 90);
      this.euCheckBox.Name = "euCheckBox";
      this.euCheckBox.Size = new System.Drawing.Size(101, 17);
      this.euCheckBox.TabIndex = 5;
      this.euCheckBox.Text = "Build for SCEE?";
      this.euCheckBox.UseVisualStyleBackColor = true;
      this.euCheckBox.CheckedChanged += new System.EventHandler(this.euCheckBox_CheckedChanged);
      // 
      // label3
      // 
      this.label3.AutoSize = true;
      this.label3.Location = new System.Drawing.Point(21, 48);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(63, 13);
      this.label3.TabIndex = 4;
      this.label3.Text = "Description:";
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(9, 22);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(71, 13);
      this.label2.TabIndex = 3;
      this.label2.Text = "ID (16 chars):";
      // 
      // descriptionBox
      // 
      this.descriptionBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.descriptionBox.Location = new System.Drawing.Point(86, 45);
      this.descriptionBox.MaxLength = 127;
      this.descriptionBox.Multiline = true;
      this.descriptionBox.Name = "descriptionBox";
      this.descriptionBox.Size = new System.Drawing.Size(381, 39);
      this.descriptionBox.TabIndex = 2;
      // 
      // idBox
      // 
      this.idBox.Location = new System.Drawing.Point(86, 19);
      this.idBox.Name = "idBox";
      this.idBox.Size = new System.Drawing.Size(177, 20);
      this.idBox.TabIndex = 0;
      this.idBox.TextChanged += new System.EventHandler(this.idBox_TextChanged);
      // 
      // groupBox3
      // 
      this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.groupBox3.Controls.Add(this.buildButton);
      this.groupBox3.Controls.Add(this.contentIdTextBox);
      this.groupBox3.Controls.Add(this.label4);
      this.groupBox3.Enabled = false;
      this.groupBox3.Location = new System.Drawing.Point(4, 332);
      this.groupBox3.Name = "groupBox3";
      this.groupBox3.Size = new System.Drawing.Size(473, 40);
      this.groupBox3.TabIndex = 3;
      this.groupBox3.TabStop = false;
      this.groupBox3.Text = "Step 3: Build PKG";
      // 
      // buildButton
      // 
      this.buildButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.buildButton.Location = new System.Drawing.Point(392, 11);
      this.buildButton.Name = "buildButton";
      this.buildButton.Size = new System.Drawing.Size(75, 23);
      this.buildButton.TabIndex = 2;
      this.buildButton.Text = "Build";
      this.buildButton.UseVisualStyleBackColor = true;
      this.buildButton.Click += new System.EventHandler(this.buildButton_Click);
      // 
      // contentIdTextBox
      // 
      this.contentIdTextBox.AutoSize = true;
      this.contentIdTextBox.Location = new System.Drawing.Point(98, 16);
      this.contentIdTextBox.Name = "contentIdTextBox";
      this.contentIdTextBox.Size = new System.Drawing.Size(0, 13);
      this.contentIdTextBox.TabIndex = 1;
      // 
      // label4
      // 
      this.label4.AutoSize = true;
      this.label4.Location = new System.Drawing.Point(6, 16);
      this.label4.Name = "label4";
      this.label4.Size = new System.Drawing.Size(86, 13);
      this.label4.TabIndex = 0;
      this.label4.Text = "PKG Content ID:";
      // 
      // label5
      // 
      this.label5.AutoSize = true;
      this.label5.Location = new System.Drawing.Point(3, 375);
      this.label5.Name = "label5";
      this.label5.Size = new System.Drawing.Size(28, 13);
      this.label5.TabIndex = 6;
      this.label5.Text = "Log:";
      // 
      // logBox
      // 
      this.logBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.logBox.Location = new System.Drawing.Point(3, 391);
      this.logBox.Multiline = true;
      this.logBox.Name = "logBox";
      this.logBox.ReadOnly = true;
      this.logBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
      this.logBox.Size = new System.Drawing.Size(474, 44);
      this.logBox.TabIndex = 5;
      // 
      // ConversionInspector
      // 
      this.AllowDrop = true;
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.label5);
      this.Controls.Add(this.logBox);
      this.Controls.Add(this.groupBox3);
      this.Controls.Add(this.groupBox2);
      this.Controls.Add(this.groupBox1);
      this.Name = "ConversionInspector";
      this.Size = new System.Drawing.Size(480, 438);
      this.groupBox1.ResumeLayout(false);
      this.groupBox2.ResumeLayout(false);
      this.groupBox2.PerformLayout();
      this.groupBox3.ResumeLayout(false);
      this.groupBox3.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Button pickFileButton;
    private System.Windows.Forms.GroupBox groupBox1;
    private System.Windows.Forms.GroupBox groupBox2;
    private System.Windows.Forms.GroupBox groupBox3;
    private System.Windows.Forms.TextBox idBox;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.TextBox descriptionBox;
    private System.Windows.Forms.CheckBox euCheckBox;
    private System.Windows.Forms.Label contentIdTextBox;
    private System.Windows.Forms.CheckBox volumeAdjustCheckBox;
    private System.Windows.Forms.ListBox listBox1;
    private System.Windows.Forms.Label label5;
    private System.Windows.Forms.TextBox logBox;
    private System.Windows.Forms.Button clearButton;
    private System.Windows.Forms.Button buildButton;
    private System.Windows.Forms.Label label4;
  }
}
