namespace ForgeToolGUI
{
  partial class MeshInspector
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
      this.glControl1 = new OpenTK.GLControl();
      this.checkBox1 = new System.Windows.Forms.CheckBox();
      this.label1 = new System.Windows.Forms.Label();
      this.trisLabel = new System.Windows.Forms.Label();
      this.vertsLabel = new System.Windows.Forms.Label();
      this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
      this.flowLayoutPanel1.SuspendLayout();
      this.SuspendLayout();
      // 
      // glControl1
      // 
      this.glControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.glControl1.BackColor = System.Drawing.Color.Black;
      this.glControl1.Location = new System.Drawing.Point(0, 26);
      this.glControl1.Name = "glControl1";
      this.glControl1.Size = new System.Drawing.Size(721, 652);
      this.glControl1.TabIndex = 0;
      this.glControl1.VSync = false;
      this.glControl1.Load += new System.EventHandler(this.glControl1_Load);
      this.glControl1.Paint += new System.Windows.Forms.PaintEventHandler(this.glControl1_Paint);
      this.glControl1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.glControl1_MouseDown);
      this.glControl1.MouseLeave += new System.EventHandler(this.glControl1_MouseLeave);
      this.glControl1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.glControl1_MouseMove);
      this.glControl1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.glControl1_MouseUp);
      this.glControl1.Resize += new System.EventHandler(this.glControl1_Resize);
      // 
      // checkBox1
      // 
      this.checkBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.checkBox1.AutoSize = true;
      this.checkBox1.Location = new System.Drawing.Point(644, 3);
      this.checkBox1.Name = "checkBox1";
      this.checkBox1.Size = new System.Drawing.Size(74, 17);
      this.checkBox1.TabIndex = 1;
      this.checkBox1.Text = "Wireframe";
      this.checkBox1.UseVisualStyleBackColor = true;
      this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(166, 0);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(71, 13);
      this.label1.TabIndex = 2;
      this.label1.Text = "Position: x,y,z";
      // 
      // trisLabel
      // 
      this.trisLabel.AutoSize = true;
      this.trisLabel.Location = new System.Drawing.Point(3, 0);
      this.trisLabel.Name = "trisLabel";
      this.trisLabel.Size = new System.Drawing.Size(72, 13);
      this.trisLabel.TabIndex = 3;
      this.trisLabel.Text = "Tris: 0000000";
      // 
      // vertsLabel
      // 
      this.vertsLabel.AutoSize = true;
      this.vertsLabel.Location = new System.Drawing.Point(81, 0);
      this.vertsLabel.Name = "vertsLabel";
      this.vertsLabel.Size = new System.Drawing.Size(79, 13);
      this.vertsLabel.TabIndex = 4;
      this.vertsLabel.Text = "Verts: 0000000";
      // 
      // flowLayoutPanel1
      // 
      this.flowLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.flowLayoutPanel1.Controls.Add(this.trisLabel);
      this.flowLayoutPanel1.Controls.Add(this.vertsLabel);
      this.flowLayoutPanel1.Controls.Add(this.label1);
      this.flowLayoutPanel1.Location = new System.Drawing.Point(3, 4);
      this.flowLayoutPanel1.Name = "flowLayoutPanel1";
      this.flowLayoutPanel1.Size = new System.Drawing.Size(635, 16);
      this.flowLayoutPanel1.TabIndex = 5;
      // 
      // MeshInspector
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.flowLayoutPanel1);
      this.Controls.Add(this.checkBox1);
      this.Controls.Add(this.glControl1);
      this.Name = "MeshInspector";
      this.Size = new System.Drawing.Size(721, 678);
      this.flowLayoutPanel1.ResumeLayout(false);
      this.flowLayoutPanel1.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private OpenTK.GLControl glControl1;
    private System.Windows.Forms.CheckBox checkBox1;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Label trisLabel;
    private System.Windows.Forms.Label vertsLabel;
    private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
  }
}
