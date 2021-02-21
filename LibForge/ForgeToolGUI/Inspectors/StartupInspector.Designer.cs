
namespace ForgeToolGUI.Inspectors
{
  partial class StartupInspector
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
      this.convertButton = new System.Windows.Forms.Button();
      this.label1 = new System.Windows.Forms.Label();
      this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
      this.openFileButton = new System.Windows.Forms.Button();
      this.openPackageButton = new System.Windows.Forms.Button();
      this.openFolderButton = new System.Windows.Forms.Button();
      this.tableLayoutPanel1.SuspendLayout();
      this.SuspendLayout();
      // 
      // convertButton
      // 
      this.convertButton.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.convertButton.Location = new System.Drawing.Point(321, 3);
      this.convertButton.Name = "convertButton";
      this.convertButton.Size = new System.Drawing.Size(312, 165);
      this.convertButton.TabIndex = 0;
      this.convertButton.Text = "Convert RB3 Customs to RB4";
      this.convertButton.UseVisualStyleBackColor = true;
      this.convertButton.Click += new System.EventHandler(this.button1_Click);
      // 
      // label1
      // 
      this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.label1.Location = new System.Drawing.Point(-3, 0);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(645, 42);
      this.label1.TabIndex = 1;
      this.label1.Text = "Welcome to ForgeToolGUI!\r\n\r\nSelect an action below.";
      this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
      // 
      // tableLayoutPanel1
      // 
      this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.tableLayoutPanel1.ColumnCount = 2;
      this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
      this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
      this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
      this.tableLayoutPanel1.Controls.Add(this.convertButton, 1, 0);
      this.tableLayoutPanel1.Controls.Add(this.openFileButton, 0, 0);
      this.tableLayoutPanel1.Controls.Add(this.openPackageButton, 0, 1);
      this.tableLayoutPanel1.Controls.Add(this.openFolderButton, 1, 1);
      this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 45);
      this.tableLayoutPanel1.Name = "tableLayoutPanel1";
      this.tableLayoutPanel1.RowCount = 2;
      this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
      this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
      this.tableLayoutPanel1.Size = new System.Drawing.Size(636, 342);
      this.tableLayoutPanel1.TabIndex = 3;
      // 
      // openFileButton
      // 
      this.openFileButton.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.openFileButton.Location = new System.Drawing.Point(3, 3);
      this.openFileButton.Name = "openFileButton";
      this.openFileButton.Size = new System.Drawing.Size(312, 165);
      this.openFileButton.TabIndex = 1;
      this.openFileButton.Text = "Open a file...";
      this.openFileButton.UseVisualStyleBackColor = true;
      this.openFileButton.Click += new System.EventHandler(this.openFileButton_Click);
      // 
      // openPackageButton
      // 
      this.openPackageButton.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.openPackageButton.Location = new System.Drawing.Point(3, 174);
      this.openPackageButton.Name = "openPackageButton";
      this.openPackageButton.Size = new System.Drawing.Size(312, 165);
      this.openPackageButton.TabIndex = 1;
      this.openPackageButton.Text = "Open a package or archive...";
      this.openPackageButton.UseVisualStyleBackColor = true;
      this.openPackageButton.Click += new System.EventHandler(this.openPackageButton_Click);
      // 
      // openFolderButton
      // 
      this.openFolderButton.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.openFolderButton.Location = new System.Drawing.Point(321, 174);
      this.openFolderButton.Name = "openFolderButton";
      this.openFolderButton.Size = new System.Drawing.Size(312, 165);
      this.openFolderButton.TabIndex = 1;
      this.openFolderButton.Text = "Open a folder...";
      this.openFolderButton.UseVisualStyleBackColor = true;
      this.openFolderButton.Click += new System.EventHandler(this.openFolderButton_Click);
      // 
      // StartupInspector
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.tableLayoutPanel1);
      this.Controls.Add(this.label1);
      this.Name = "StartupInspector";
      this.Size = new System.Drawing.Size(642, 390);
      this.tableLayoutPanel1.ResumeLayout(false);
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Button convertButton;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    private System.Windows.Forms.Button openFileButton;
    private System.Windows.Forms.Button openPackageButton;
    private System.Windows.Forms.Button openFolderButton;
  }
}
