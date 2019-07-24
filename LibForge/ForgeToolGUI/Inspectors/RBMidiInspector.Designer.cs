namespace ForgeToolGUI
{
  partial class RBMidiInspector
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
      this.splitContainer2 = new System.Windows.Forms.SplitContainer();
      this.button1 = new System.Windows.Forms.Button();
      this.difficultySelector = new System.Windows.Forms.ComboBox();
      this.gemDisplay = new System.Windows.Forms.PictureBox();
      this.trackBar = new System.Windows.Forms.HScrollBar();
      this.zoomBar = new System.Windows.Forms.TrackBar();
      this.button3 = new System.Windows.Forms.Button();
      this.button2 = new System.Windows.Forms.Button();
      ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
      this.splitContainer2.Panel1.SuspendLayout();
      this.splitContainer2.Panel2.SuspendLayout();
      this.splitContainer2.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.gemDisplay)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.zoomBar)).BeginInit();
      this.SuspendLayout();
      // 
      // splitContainer2
      // 
      this.splitContainer2.BackColor = System.Drawing.SystemColors.ControlDarkDark;
      this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
      this.splitContainer2.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
      this.splitContainer2.Location = new System.Drawing.Point(0, 0);
      this.splitContainer2.Name = "splitContainer2";
      this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
      // 
      // splitContainer2.Panel1
      // 
      this.splitContainer2.Panel1.BackColor = System.Drawing.SystemColors.ControlLight;
      this.splitContainer2.Panel1.Controls.Add(this.button2);
      this.splitContainer2.Panel1.Controls.Add(this.button1);
      this.splitContainer2.Panel1.Controls.Add(this.difficultySelector);
      this.splitContainer2.Panel1.Controls.Add(this.gemDisplay);
      this.splitContainer2.Panel1.Controls.Add(this.trackBar);
      this.splitContainer2.Panel1.Controls.Add(this.zoomBar);
      // 
      // splitContainer2.Panel2
      // 
      this.splitContainer2.Panel2.Controls.Add(this.button3);
      this.splitContainer2.Size = new System.Drawing.Size(627, 415);
      this.splitContainer2.SplitterDistance = 358;
      this.splitContainer2.TabIndex = 8;
      // 
      // button1
      // 
      this.button1.Location = new System.Drawing.Point(411, 3);
      this.button1.Name = "button1";
      this.button1.Size = new System.Drawing.Size(75, 23);
      this.button1.TabIndex = 8;
      this.button1.Text = "Reprocess";
      this.button1.UseVisualStyleBackColor = true;
      this.button1.Click += new System.EventHandler(this.button1_Click);
      // 
      // difficultySelector
      // 
      this.difficultySelector.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.difficultySelector.FormattingEnabled = true;
      this.difficultySelector.Location = new System.Drawing.Point(3, 3);
      this.difficultySelector.Name = "difficultySelector";
      this.difficultySelector.Size = new System.Drawing.Size(180, 21);
      this.difficultySelector.TabIndex = 5;
      this.difficultySelector.SelectedIndexChanged += new System.EventHandler(this.difficultySelector_SelectedIndexChanged);
      // 
      // gemDisplay
      // 
      this.gemDisplay.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.gemDisplay.Location = new System.Drawing.Point(0, 30);
      this.gemDisplay.Name = "gemDisplay";
      this.gemDisplay.Size = new System.Drawing.Size(627, 295);
      this.gemDisplay.TabIndex = 4;
      this.gemDisplay.TabStop = false;
      this.gemDisplay.SizeChanged += new System.EventHandler(this.gemDisplay_SizeChanged);
      // 
      // trackBar
      // 
      this.trackBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.trackBar.Location = new System.Drawing.Point(3, 328);
      this.trackBar.Maximum = 1000;
      this.trackBar.Name = "trackBar";
      this.trackBar.Size = new System.Drawing.Size(624, 30);
      this.trackBar.TabIndex = 6;
      this.trackBar.Scroll += new System.Windows.Forms.ScrollEventHandler(this.seekBar_Scroll);
      // 
      // zoomBar
      // 
      this.zoomBar.LargeChange = 1;
      this.zoomBar.Location = new System.Drawing.Point(210, 3);
      this.zoomBar.Maximum = 40;
      this.zoomBar.Minimum = -8;
      this.zoomBar.Name = "zoomBar";
      this.zoomBar.Size = new System.Drawing.Size(195, 45);
      this.zoomBar.TabIndex = 7;
      this.zoomBar.TickFrequency = 4;
      this.zoomBar.Scroll += new System.EventHandler(this.zoomBar_Scroll);
      // 
      // button3
      // 
      this.button3.Dock = System.Windows.Forms.DockStyle.Fill;
      this.button3.Location = new System.Drawing.Point(0, 0);
      this.button3.Name = "button3";
      this.button3.Size = new System.Drawing.Size(627, 53);
      this.button3.TabIndex = 2;
      this.button3.Text = "Populate Structure View";
      this.button3.UseVisualStyleBackColor = true;
      this.button3.Click += new System.EventHandler(this.button3_Click);
      // 
      // button2
      // 
      this.button2.Location = new System.Drawing.Point(492, 3);
      this.button2.Name = "button2";
      this.button2.Size = new System.Drawing.Size(75, 23);
      this.button2.TabIndex = 9;
      this.button2.Text = "Export .mid";
      this.button2.UseVisualStyleBackColor = true;
      this.button2.Click += new System.EventHandler(this.Button2_Click);
      // 
      // RBMidiInspector
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.splitContainer2);
      this.Name = "RBMidiInspector";
      this.Size = new System.Drawing.Size(627, 415);
      this.splitContainer2.Panel1.ResumeLayout(false);
      this.splitContainer2.Panel1.PerformLayout();
      this.splitContainer2.Panel2.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
      this.splitContainer2.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.gemDisplay)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.zoomBar)).EndInit();
      this.ResumeLayout(false);

    }

    #endregion
    private System.Windows.Forms.SplitContainer splitContainer2;
    private System.Windows.Forms.ComboBox difficultySelector;
    private System.Windows.Forms.PictureBox gemDisplay;
    private System.Windows.Forms.HScrollBar trackBar;
    private System.Windows.Forms.TrackBar zoomBar;
    private System.Windows.Forms.Button button3;
    private System.Windows.Forms.Button button1;
    private System.Windows.Forms.Button button2;
  }
}
