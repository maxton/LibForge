namespace ForgeToolGUI
{
  partial class ArkFileBrowser
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

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      this.fileTreeView = new System.Windows.Forms.TreeView();
      this.menuStrip1 = new System.Windows.Forms.MenuStrip();
      this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
      this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.tabPage4 = new System.Windows.Forms.TabPage();
      this.meshTextBox = new System.Windows.Forms.TextBox();
      this.tabPage3 = new System.Windows.Forms.TabPage();
      this.songDataInspector1 = new ForgeToolGUI.SongDataInspector();
      this.tabPage2 = new System.Windows.Forms.TabPage();
      this.dataTextBox = new System.Windows.Forms.TextBox();
      this.tabPage1 = new System.Windows.Forms.TabPage();
      this.pictureBox1 = new System.Windows.Forms.PictureBox();
      this.tabControl1 = new System.Windows.Forms.TabControl();
      this.tabPage5 = new System.Windows.Forms.TabPage();
      this.treeView1 = new System.Windows.Forms.TreeView();
      this.menuStrip1.SuspendLayout();
      this.tabPage4.SuspendLayout();
      this.tabPage3.SuspendLayout();
      this.tabPage2.SuspendLayout();
      this.tabPage1.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
      this.tabControl1.SuspendLayout();
      this.tabPage5.SuspendLayout();
      this.SuspendLayout();
      // 
      // fileTreeView
      // 
      this.fileTreeView.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
      this.fileTreeView.Location = new System.Drawing.Point(12, 27);
      this.fileTreeView.Name = "fileTreeView";
      this.fileTreeView.Size = new System.Drawing.Size(378, 464);
      this.fileTreeView.TabIndex = 0;
      this.fileTreeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
      // 
      // menuStrip1
      // 
      this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
      this.menuStrip1.Location = new System.Drawing.Point(0, 0);
      this.menuStrip1.Name = "menuStrip1";
      this.menuStrip1.Size = new System.Drawing.Size(812, 24);
      this.menuStrip1.TabIndex = 1;
      this.menuStrip1.Text = "menuStrip1";
      // 
      // fileToolStripMenuItem
      // 
      this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.toolStripMenuItem1,
            this.closeToolStripMenuItem,
            this.exitToolStripMenuItem});
      this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
      this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
      this.fileToolStripMenuItem.Text = "&File";
      // 
      // openToolStripMenuItem
      // 
      this.openToolStripMenuItem.Name = "openToolStripMenuItem";
      this.openToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
      this.openToolStripMenuItem.Text = "&Open...";
      this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
      // 
      // toolStripMenuItem1
      // 
      this.toolStripMenuItem1.Name = "toolStripMenuItem1";
      this.toolStripMenuItem1.Size = new System.Drawing.Size(148, 22);
      this.toolStripMenuItem1.Text = "Open &Folder...";
      this.toolStripMenuItem1.Click += new System.EventHandler(this.toolStripMenuItem1_Click);
      // 
      // closeToolStripMenuItem
      // 
      this.closeToolStripMenuItem.Enabled = false;
      this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
      this.closeToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
      this.closeToolStripMenuItem.Text = "&Close";
      this.closeToolStripMenuItem.Click += new System.EventHandler(this.closeToolStripMenuItem_Click);
      // 
      // exitToolStripMenuItem
      // 
      this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
      this.exitToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
      this.exitToolStripMenuItem.Text = "E&xit";
      this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
      // 
      // tabPage4
      // 
      this.tabPage4.Controls.Add(this.meshTextBox);
      this.tabPage4.Location = new System.Drawing.Point(4, 22);
      this.tabPage4.Name = "tabPage4";
      this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
      this.tabPage4.Size = new System.Drawing.Size(396, 438);
      this.tabPage4.TabIndex = 3;
      this.tabPage4.Text = "Mesh";
      this.tabPage4.UseVisualStyleBackColor = true;
      // 
      // meshTextBox
      // 
      this.meshTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.meshTextBox.Location = new System.Drawing.Point(6, 6);
      this.meshTextBox.Multiline = true;
      this.meshTextBox.Name = "meshTextBox";
      this.meshTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
      this.meshTextBox.Size = new System.Drawing.Size(384, 426);
      this.meshTextBox.TabIndex = 0;
      // 
      // tabPage3
      // 
      this.tabPage3.Controls.Add(this.songDataInspector1);
      this.tabPage3.Location = new System.Drawing.Point(4, 22);
      this.tabPage3.Name = "tabPage3";
      this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
      this.tabPage3.Size = new System.Drawing.Size(396, 438);
      this.tabPage3.TabIndex = 2;
      this.tabPage3.Text = "SongDTA";
      this.tabPage3.UseVisualStyleBackColor = true;
      // 
      // songDataInspector1
      // 
      this.songDataInspector1.Location = new System.Drawing.Point(6, 6);
      this.songDataInspector1.Name = "songDataInspector1";
      this.songDataInspector1.Size = new System.Drawing.Size(288, 426);
      this.songDataInspector1.TabIndex = 0;
      // 
      // tabPage2
      // 
      this.tabPage2.Controls.Add(this.dataTextBox);
      this.tabPage2.Location = new System.Drawing.Point(4, 22);
      this.tabPage2.Name = "tabPage2";
      this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
      this.tabPage2.Size = new System.Drawing.Size(396, 438);
      this.tabPage2.TabIndex = 1;
      this.tabPage2.Text = "Data";
      this.tabPage2.UseVisualStyleBackColor = true;
      // 
      // dataTextBox
      // 
      this.dataTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.dataTextBox.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.dataTextBox.Location = new System.Drawing.Point(6, 4);
      this.dataTextBox.Multiline = true;
      this.dataTextBox.Name = "dataTextBox";
      this.dataTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
      this.dataTextBox.Size = new System.Drawing.Size(384, 428);
      this.dataTextBox.TabIndex = 0;
      // 
      // tabPage1
      // 
      this.tabPage1.AutoScroll = true;
      this.tabPage1.AutoScrollMinSize = new System.Drawing.Size(100, 100);
      this.tabPage1.Controls.Add(this.pictureBox1);
      this.tabPage1.Location = new System.Drawing.Point(4, 22);
      this.tabPage1.Name = "tabPage1";
      this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
      this.tabPage1.Size = new System.Drawing.Size(396, 438);
      this.tabPage1.TabIndex = 0;
      this.tabPage1.Text = "Texture";
      this.tabPage1.UseVisualStyleBackColor = true;
      // 
      // pictureBox1
      // 
      this.pictureBox1.Location = new System.Drawing.Point(6, 6);
      this.pictureBox1.Name = "pictureBox1";
      this.pictureBox1.Size = new System.Drawing.Size(18, 17);
      this.pictureBox1.TabIndex = 2;
      this.pictureBox1.TabStop = false;
      // 
      // tabControl1
      // 
      this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.tabControl1.Controls.Add(this.tabPage1);
      this.tabControl1.Controls.Add(this.tabPage2);
      this.tabControl1.Controls.Add(this.tabPage3);
      this.tabControl1.Controls.Add(this.tabPage4);
      this.tabControl1.Controls.Add(this.tabPage5);
      this.tabControl1.Location = new System.Drawing.Point(396, 27);
      this.tabControl1.Name = "tabControl1";
      this.tabControl1.SelectedIndex = 0;
      this.tabControl1.Size = new System.Drawing.Size(404, 464);
      this.tabControl1.TabIndex = 3;
      // 
      // tabPage5
      // 
      this.tabPage5.Controls.Add(this.treeView1);
      this.tabPage5.Location = new System.Drawing.Point(4, 22);
      this.tabPage5.Name = "tabPage5";
      this.tabPage5.Padding = new System.Windows.Forms.Padding(3);
      this.tabPage5.Size = new System.Drawing.Size(396, 438);
      this.tabPage5.TabIndex = 4;
      this.tabPage5.Text = "RBMid";
      this.tabPage5.UseVisualStyleBackColor = true;
      // 
      // treeView1
      // 
      this.treeView1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.treeView1.Location = new System.Drawing.Point(3, 3);
      this.treeView1.Name = "treeView1";
      this.treeView1.Size = new System.Drawing.Size(390, 432);
      this.treeView1.TabIndex = 0;
      // 
      // ArkFileBrowser
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(812, 503);
      this.Controls.Add(this.tabControl1);
      this.Controls.Add(this.fileTreeView);
      this.Controls.Add(this.menuStrip1);
      this.MainMenuStrip = this.menuStrip1;
      this.Name = "ArkFileBrowser";
      this.ShowIcon = false;
      this.Text = "Ark File Browser";
      this.menuStrip1.ResumeLayout(false);
      this.menuStrip1.PerformLayout();
      this.tabPage4.ResumeLayout(false);
      this.tabPage4.PerformLayout();
      this.tabPage3.ResumeLayout(false);
      this.tabPage2.ResumeLayout(false);
      this.tabPage2.PerformLayout();
      this.tabPage1.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
      this.tabControl1.ResumeLayout(false);
      this.tabPage5.ResumeLayout(false);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.TreeView fileTreeView;
    private System.Windows.Forms.MenuStrip menuStrip1;
    private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem closeToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
    private System.Windows.Forms.TabPage tabPage4;
    private System.Windows.Forms.TextBox meshTextBox;
    private System.Windows.Forms.TabPage tabPage3;
    private SongDataInspector songDataInspector1;
    private System.Windows.Forms.TabPage tabPage2;
    private System.Windows.Forms.TextBox dataTextBox;
    private System.Windows.Forms.TabPage tabPage1;
    private System.Windows.Forms.PictureBox pictureBox1;
    private System.Windows.Forms.TabControl tabControl1;
    private System.Windows.Forms.TabPage tabPage5;
    private System.Windows.Forms.TreeView treeView1;
  }
}

