namespace ForgeToolGUI
{
  partial class GemTrackInspector
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
      this.gemDisplay = new System.Windows.Forms.PictureBox();
      this.zoomBar = new System.Windows.Forms.TrackBar();
      this.hScrollBar1 = new System.Windows.Forms.HScrollBar();
      this.difficultySelector = new System.Windows.Forms.ComboBox();
      ((System.ComponentModel.ISupportInitialize)(this.gemDisplay)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.zoomBar)).BeginInit();
      this.SuspendLayout();
      // 
      // pictureBox2
      // 
      this.gemDisplay.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
      | System.Windows.Forms.AnchorStyles.Left)
      | System.Windows.Forms.AnchorStyles.Right)));
      this.gemDisplay.Location = new System.Drawing.Point(0, 30);
      this.gemDisplay.Name = "pictureBox2";
      this.gemDisplay.Size = new System.Drawing.Size(408, 188);
      this.gemDisplay.TabIndex = 4;
      this.gemDisplay.TabStop = false;
      this.gemDisplay.SizeChanged += new System.EventHandler(this.gemDisplay_SizeChanged);
      // 
      // trackBar1
      // 
      this.zoomBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
      | System.Windows.Forms.AnchorStyles.Right)));
      this.zoomBar.LargeChange = 1;
      this.zoomBar.Location = new System.Drawing.Point(210, 3);
      this.zoomBar.Maximum = 40;
      this.zoomBar.Minimum = -8;
      this.zoomBar.Name = "trackBar1";
      this.zoomBar.Size = new System.Drawing.Size(198, 45);
      this.zoomBar.TabIndex = 7;
      this.zoomBar.TickFrequency = 4;
      this.zoomBar.Scroll += new System.EventHandler(this.zoomBar_Scroll);
      // 
      // hScrollBar1
      // 
      this.hScrollBar1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
      | System.Windows.Forms.AnchorStyles.Right)));
      this.hScrollBar1.Location = new System.Drawing.Point(0, 225);
      this.hScrollBar1.Maximum = 1000;
      this.hScrollBar1.Name = "hScrollBar1";
      this.hScrollBar1.Size = new System.Drawing.Size(408, 30);
      this.hScrollBar1.TabIndex = 6;
      this.hScrollBar1.Scroll += new System.Windows.Forms.ScrollEventHandler(this.seekBar_Scroll);
      // 
      // comboBox1
      // 
      this.difficultySelector.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.difficultySelector.FormattingEnabled = true;
      this.difficultySelector.Items.AddRange(new object[] {
            "Easy",
            "Medium",
            "Hard",
            "Expert"});
      this.difficultySelector.Location = new System.Drawing.Point(3, 3);
      this.difficultySelector.Name = "comboBox1";
      this.difficultySelector.Size = new System.Drawing.Size(180, 21);
      this.difficultySelector.TabIndex = 5;
      this.difficultySelector.SelectedIndexChanged += new System.EventHandler(this.difficultySelector_SelectedIndexChanged);
      // 
      // GemTrackInspector
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.gemDisplay);
      this.Controls.Add(this.zoomBar);
      this.Controls.Add(this.hScrollBar1);
      this.Controls.Add(this.difficultySelector);
      this.Name = "GemTrackInspector";
      this.Size = new System.Drawing.Size(411, 272);
      ((System.ComponentModel.ISupportInitialize)(this.gemDisplay)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.zoomBar)).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.PictureBox gemDisplay;
    private System.Windows.Forms.TrackBar zoomBar;
    private System.Windows.Forms.HScrollBar hScrollBar1;
    private System.Windows.Forms.ComboBox difficultySelector;
  }
}
