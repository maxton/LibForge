namespace ForgeToolGUI
{
  partial class ForgeBrowser
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
      this.components = new System.ComponentModel.Container();
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ForgeBrowser));
      this.fileTreeView = new System.Windows.Forms.TreeView();
      this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
      this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.extractToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.menuStrip1 = new System.Windows.Forms.MenuStrip();
      this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.openFileMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.openPackageMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.openFolderMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
      this.closeTabMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.closePackageMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
      this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.convertCONToPKGToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.tabControl1 = new System.Windows.Forms.TabControl();
      this.splitContainer1 = new System.Windows.Forms.SplitContainer();
      this.contextMenuStrip1.SuspendLayout();
      this.menuStrip1.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
      this.splitContainer1.Panel1.SuspendLayout();
      this.splitContainer1.Panel2.SuspendLayout();
      this.splitContainer1.SuspendLayout();
      this.SuspendLayout();
      // 
      // fileTreeView
      // 
      this.fileTreeView.ContextMenuStrip = this.contextMenuStrip1;
      this.fileTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
      this.fileTreeView.Location = new System.Drawing.Point(0, 0);
      this.fileTreeView.Name = "fileTreeView";
      this.fileTreeView.Size = new System.Drawing.Size(200, 479);
      this.fileTreeView.TabIndex = 0;
      this.fileTreeView.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.fileTreeView_NodeMouseClick);
      this.fileTreeView.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.previewSelectedNode);
      this.fileTreeView.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.fileTreeView_KeyPress);
      // 
      // contextMenuStrip1
      // 
      this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.extractToolStripMenuItem});
      this.contextMenuStrip1.Name = "contextMenuStrip1";
      this.contextMenuStrip1.Size = new System.Drawing.Size(111, 48);
      // 
      // openToolStripMenuItem
      // 
      this.openToolStripMenuItem.Name = "openToolStripMenuItem";
      this.openToolStripMenuItem.Size = new System.Drawing.Size(110, 22);
      this.openToolStripMenuItem.Text = "Open";
      this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click_1);
      // 
      // extractToolStripMenuItem
      // 
      this.extractToolStripMenuItem.Name = "extractToolStripMenuItem";
      this.extractToolStripMenuItem.Size = new System.Drawing.Size(110, 22);
      this.extractToolStripMenuItem.Text = "Extract";
      this.extractToolStripMenuItem.Click += new System.EventHandler(this.extractToolStripMenuItem_Click);
      // 
      // menuStrip1
      // 
      this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.toolsToolStripMenuItem});
      this.menuStrip1.Location = new System.Drawing.Point(0, 0);
      this.menuStrip1.Name = "menuStrip1";
      this.menuStrip1.Size = new System.Drawing.Size(812, 24);
      this.menuStrip1.TabIndex = 1;
      this.menuStrip1.Text = "menuStrip1";
      // 
      // fileToolStripMenuItem
      // 
      this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openFileMenuItem,
            this.openPackageMenuItem,
            this.openFolderMenuItem,
            this.toolStripSeparator1,
            this.closeTabMenuItem,
            this.closePackageMenuItem,
            this.toolStripSeparator2,
            this.exitToolStripMenuItem});
      this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
      this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
      this.fileToolStripMenuItem.Text = "&File";
      // 
      // openFileMenuItem
      // 
      this.openFileMenuItem.Name = "openFileMenuItem";
      this.openFileMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
      this.openFileMenuItem.Size = new System.Drawing.Size(234, 22);
      this.openFileMenuItem.Text = "&Open File...";
      this.openFileMenuItem.Click += new System.EventHandler(this.openFile_Click);
      // 
      // openPackageMenuItem
      // 
      this.openPackageMenuItem.Name = "openPackageMenuItem";
      this.openPackageMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.O)));
      this.openPackageMenuItem.Size = new System.Drawing.Size(234, 22);
      this.openPackageMenuItem.Text = "Open &Package...";
      this.openPackageMenuItem.Click += new System.EventHandler(this.openPackage_Click);
      // 
      // openFolderMenuItem
      // 
      this.openFolderMenuItem.Name = "openFolderMenuItem";
      this.openFolderMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F)));
      this.openFolderMenuItem.Size = new System.Drawing.Size(234, 22);
      this.openFolderMenuItem.Text = "Open &Folder...";
      this.openFolderMenuItem.Click += new System.EventHandler(this.openFolder_Click);
      // 
      // toolStripSeparator1
      // 
      this.toolStripSeparator1.Name = "toolStripSeparator1";
      this.toolStripSeparator1.Size = new System.Drawing.Size(231, 6);
      // 
      // closeTabMenuItem
      // 
      this.closeTabMenuItem.Name = "closeTabMenuItem";
      this.closeTabMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.W)));
      this.closeTabMenuItem.Size = new System.Drawing.Size(234, 22);
      this.closeTabMenuItem.Text = "Close &Tab";
      this.closeTabMenuItem.Click += new System.EventHandler(this.toolStripMenuItem3_Click);
      // 
      // closePackageMenuItem
      // 
      this.closePackageMenuItem.Enabled = false;
      this.closePackageMenuItem.Name = "closePackageMenuItem";
      this.closePackageMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.W)));
      this.closePackageMenuItem.Size = new System.Drawing.Size(234, 22);
      this.closePackageMenuItem.Text = "&Close Package";
      this.closePackageMenuItem.Click += new System.EventHandler(this.closeToolStripMenuItem_Click);
      // 
      // toolStripSeparator2
      // 
      this.toolStripSeparator2.Name = "toolStripSeparator2";
      this.toolStripSeparator2.Size = new System.Drawing.Size(231, 6);
      // 
      // exitToolStripMenuItem
      // 
      this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
      this.exitToolStripMenuItem.Size = new System.Drawing.Size(234, 22);
      this.exitToolStripMenuItem.Text = "E&xit";
      this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
      // 
      // toolsToolStripMenuItem
      // 
      this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.convertCONToPKGToolStripMenuItem});
      this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
      this.toolsToolStripMenuItem.Size = new System.Drawing.Size(46, 20);
      this.toolsToolStripMenuItem.Text = "&Tools";
      // 
      // convertCONToPKGToolStripMenuItem
      // 
      this.convertCONToPKGToolStripMenuItem.Name = "convertCONToPKGToolStripMenuItem";
      this.convertCONToPKGToolStripMenuItem.Size = new System.Drawing.Size(207, 22);
      this.convertCONToPKGToolStripMenuItem.Text = "Convert CON files to PKG";
      this.convertCONToPKGToolStripMenuItem.Click += new System.EventHandler(this.convertCONToPKGToolStripMenuItem_Click);
      // 
      // tabControl1
      // 
      this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.tabControl1.Location = new System.Drawing.Point(0, 0);
      this.tabControl1.Multiline = true;
      this.tabControl1.Name = "tabControl1";
      this.tabControl1.SelectedIndex = 0;
      this.tabControl1.Size = new System.Drawing.Size(604, 479);
      this.tabControl1.TabIndex = 3;
      this.tabControl1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.tabControl1_MouseClick);
      // 
      // splitContainer1
      // 
      this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.splitContainer1.Location = new System.Drawing.Point(0, 24);
      this.splitContainer1.Name = "splitContainer1";
      // 
      // splitContainer1.Panel1
      // 
      this.splitContainer1.Panel1.Controls.Add(this.fileTreeView);
      this.splitContainer1.Panel1MinSize = 200;
      // 
      // splitContainer1.Panel2
      // 
      this.splitContainer1.Panel2.Controls.Add(this.tabControl1);
      this.splitContainer1.Size = new System.Drawing.Size(812, 479);
      this.splitContainer1.SplitterDistance = 200;
      this.splitContainer1.SplitterWidth = 8;
      this.splitContainer1.TabIndex = 4;
      // 
      // ForgeBrowser
      // 
      this.AllowDrop = true;
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(812, 503);
      this.Controls.Add(this.splitContainer1);
      this.Controls.Add(this.menuStrip1);
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.KeyPreview = true;
      this.MainMenuStrip = this.menuStrip1;
      this.MinimumSize = new System.Drawing.Size(828, 542);
      this.Name = "ForgeBrowser";
      this.Text = "ForgeToolGUI";
      this.DragDrop += new System.Windows.Forms.DragEventHandler(this.ForgeBrowser_DragDrop);
      this.DragOver += new System.Windows.Forms.DragEventHandler(this.ForgeBrowser_DragOver);
      this.contextMenuStrip1.ResumeLayout(false);
      this.menuStrip1.ResumeLayout(false);
      this.menuStrip1.PerformLayout();
      this.splitContainer1.Panel1.ResumeLayout(false);
      this.splitContainer1.Panel2.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
      this.splitContainer1.ResumeLayout(false);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.TreeView fileTreeView;
    private System.Windows.Forms.MenuStrip menuStrip1;
    private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem openPackageMenuItem;
    private System.Windows.Forms.ToolStripMenuItem closePackageMenuItem;
    private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem openFolderMenuItem;
    private System.Windows.Forms.TabControl tabControl1;
    private System.Windows.Forms.ToolStripMenuItem openFileMenuItem;
    private System.Windows.Forms.ToolStripMenuItem closeTabMenuItem;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
    private System.Windows.Forms.SplitContainer splitContainer1;
    private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem convertCONToPKGToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem extractToolStripMenuItem;
    }
}

