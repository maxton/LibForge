using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using LibForge.Mesh;

namespace ForgeToolGUI
{
  public partial class MeshInspector : Inspector
  {
    private HxMesh mesh;
    private const int vertexSize = 7 * 4; // x,y,z, u1,v1, u2,v2
    private HxMesh.Point[] points;
    float scaleFactor = 0;
    private float xOff = 0, yOff = 0, zOff = 0;
    private DragState drag = DragState.None;
    private Point startDrag;
    public MeshInspector(HxMesh mesh)
    {
      float maxPoint = 0;
      this.mesh = mesh;
      points = new HxMesh.Point[mesh.Triangles.Length * 3];
      for(int i = 0; i < mesh.Triangles.Length; i++)
      {
        points[i * 3 + 0] = mesh.Points[mesh.Triangles[i].V3];
        points[i * 3 + 1] = mesh.Points[mesh.Triangles[i].V2];
        points[i * 3 + 2] = mesh.Points[mesh.Triangles[i].V1];
        if (points[i * 3].X > maxPoint) maxPoint = points[i * 3].X;
        if (points[i * 3].Y > maxPoint) maxPoint = points[i * 3].Y;
        if (points[i * 3].Z > maxPoint) maxPoint = points[i * 3].Z;
      }
      scaleFactor = 1 / maxPoint;
      InitializeComponent();
      trisLabel.Text = $"Tris: {mesh.Triangles.Length}";
      vertsLabel.Text = $"Verts: {mesh.Points.Length}";
      glControl1.MouseWheel += GlControl1_MouseWheel;
    }

    private void glControl1_Load(object sender, EventArgs e)
    {
      vertexBuffer = new VertexBuffer<HxMesh.Point>(vertexSize, points);
      shaderProgram = new ShaderProgram(new Shader(ShaderType.VertexShader, vertexShader),
                                             new Shader(ShaderType.FragmentShader, fragmentShader));
      vertexArray = new VertexArray<HxMesh.Point>(
                this.vertexBuffer, this.shaderProgram,
                new VertexAttribute("vPosition", 3, VertexAttribPointerType.Float, vertexSize, 0),
                new VertexAttribute("vUv1", 2, VertexAttribPointerType.Float, vertexSize, 12),
                new VertexAttribute("vUv2", 2, VertexAttribPointerType.Float, vertexSize, 20));

      // create projection matrix uniform
      projectionMatrix = new Matrix4Uniform("projectionMatrix");
    }

    private void glControl1_Resize(object sender, EventArgs e)
    {
      if (glControl1.ClientSize.Height == 0)
        glControl1.ClientSize = new Size(glControl1.ClientSize.Width, 1);

      GL.Viewport(0, 0, glControl1.ClientSize.Width, glControl1.ClientSize.Height);
    }

    private void glControl1_Paint(object sender, PaintEventArgs e)
    {
      label1.Text = $"Position: {xOff:0.00}, {yOff:0.00}, {-1 + zOff:0.00}   Scale: {scaleFactor:0.000}";
      glControl1.MakeCurrent();
      GL.ClearColor(Color.DimGray);
      GL.Clear(ClearBufferMask.ColorBufferBit);

      GL.PolygonMode(MaterialFace.FrontAndBack, 
        checkBox1.Checked ? PolygonMode.Line : PolygonMode.Fill);

      projectionMatrix.Matrix = Matrix4.CreateScale(scaleFactor);
      projectionMatrix.Matrix *= Matrix4.CreateTranslation(xOff, yOff, -1 + zOff);
      projectionMatrix.Matrix *= Matrix4.CreatePerspectiveFieldOfView(
        MathHelper.PiOver2, 1f * glControl1.ClientSize.Width / glControl1.ClientSize.Height, 0.1f, 100f);

      // activate shader program and set uniforms
      this.shaderProgram.Use();
      this.projectionMatrix.Set(this.shaderProgram);

      // bind vertex buffer and array objects
      this.vertexBuffer.Bind();
      this.vertexArray.Bind();

      // upload vertices to GPU and draw them
      this.vertexBuffer.BufferData();
      this.vertexBuffer.Draw();

      // reset state for potential further draw calls (optional, but good practice)
      GL.BindVertexArray(0);
      GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
      GL.UseProgram(0);

      glControl1.SwapBuffers();
    }

    string vertexShader = @"
#version 130

// a projection transformation to apply to the vertex' position
uniform mat4 projectionMatrix;

// attributes of our vertex
in vec3 vPosition;
in vec2 vUv1;
in vec2 vUv2;

out vec4 fColor; // must match name in fragment shader

void main()
{
    // gl_Position is a special variable of OpenGL that must be set
    gl_Position = projectionMatrix * vec4(vPosition, 1.0);
    fColor = vec4(gl_Position.x, gl_Position.y, gl_Position.z, 1.0);
}";

    string fragmentShader = @"
#version 130

in vec4 fColor; // must match name in vertex shader

out vec4 fragColor; // first out variable is automatically written to the screen

void main()
{
    vec3 lightPos = vec3(0.0, 1.0, 0.0);
    
    fragColor = fColor;
}
";
    private VertexBuffer<HxMesh.Point> vertexBuffer;
    private ShaderProgram shaderProgram;
    private VertexArray<HxMesh.Point> vertexArray;

    private void glControl1_MouseDown(object sender, MouseEventArgs e)
    {
      switch (e.Button)
      {
        case MouseButtons.Left:
          drag = DragState.Pan;
          break;
        case MouseButtons.None:
          drag = DragState.None;
          break;
        case MouseButtons.Right:
          drag = DragState.Zoom;
          break;
        case MouseButtons.Middle:
          drag = DragState.Rotate;
          break;
        default:
          break;
      }
      startDrag = e.Location;
    }

    private void glControl1_MouseLeave(object sender, EventArgs e)
    {
      drag = DragState.None;
    }

    private void glControl1_MouseUp(object sender, MouseEventArgs e)
    {
      drag = DragState.None;
    }

    private void glControl1_MouseMove(object sender, MouseEventArgs e)
    {
      if (drag == DragState.None) return;
      var dragDiffX = e.Location.X - startDrag.X;
      var dragDiffY = e.Location.Y - startDrag.Y;
      switch (drag)
      {
        case DragState.None:
          break;
        case DragState.Pan:
          xOff += dragDiffX * 0.005f;
          yOff -= dragDiffY * 0.005f;
          break;
        case DragState.Zoom:
          scaleFactor += scaleFactor * (dragDiffY * 0.005f);
          break;
        case DragState.Rotate:
          break;
        default:
          break;
      }
      startDrag = e.Location;
      glControl1.Invalidate();
    }

    private void checkBox1_CheckedChanged(object sender, EventArgs e)
    {
      glControl1.Invalidate();
    }

    private Matrix4Uniform projectionMatrix;

    private void GlControl1_MouseWheel(object sender, MouseEventArgs e)
    {
      zOff += e.Delta * 0.005f;
      glControl1.Invalidate();
    }

    enum DragState
    {
      None, Pan, Zoom, Rotate
    }
  }

  sealed class Matrix4Uniform
  {
    private readonly string name;
    private Matrix4 matrix;

    public Matrix4 Matrix { get { return this.matrix; } set { this.matrix = value; } }

    public Matrix4Uniform(string name)
    {
      this.name = name;
    }

    public void Set(ShaderProgram program)
    {
      // get uniform location
      var i = program.GetUniformLocation(this.name);

      // set uniform value
      GL.UniformMatrix4(i, false, ref this.matrix);
    }
  }
  sealed class VertexBuffer<TVertex>
    where TVertex : struct // vertices must be structs so we can copy them to GPU memory easily
  {
    private readonly int vertexSize;
    private TVertex[] vertices;

    private int count;

    private readonly int handle;

    public VertexBuffer(int vertexSize, TVertex[] verts)
    {
      this.vertexSize = vertexSize;
      this.vertices = verts;
      count = verts.Length;
      // generate the actual Vertex Buffer Object
      this.handle = GL.GenBuffer();
    }

    public void Bind()
    {
      // make this the active array buffer
      GL.BindBuffer(BufferTarget.ArrayBuffer, this.handle);
    }

    public void BufferData()
    {
      // copy contained vertices to GPU memory
      GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(this.vertexSize * this.count),
          this.vertices, BufferUsageHint.StreamDraw);
    }

    public void Draw()
    {
      // draw buffered vertices as triangles
      GL.DrawArrays(PrimitiveType.Triangles, 0, this.count);
    }
  }
  sealed class VertexArray<TVertex>
    where TVertex : struct
  {
    private readonly int handle;

    public VertexArray(VertexBuffer<TVertex> vertexBuffer, ShaderProgram program,
        params VertexAttribute[] attributes)
    {
      // create new vertex array object
      GL.GenVertexArrays(1, out this.handle);

      // bind the object so we can modify it
      this.Bind();

      // bind the vertex buffer object
      vertexBuffer.Bind();

      // set all attributes
      foreach (var attribute in attributes)
        attribute.Set(program);

      // unbind objects to reset state
      GL.BindVertexArray(0);
      GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
    }

    public void Bind()
    {
      // bind for usage (modification or rendering)
      GL.BindVertexArray(this.handle);
    }
  }
  sealed class VertexAttribute
  {
    private readonly string name;
    private readonly int size;
    private readonly VertexAttribPointerType type;
    private readonly bool normalize;
    private readonly int stride;
    private readonly int offset;

    public VertexAttribute(string name, int size, VertexAttribPointerType type,
        int stride, int offset, bool normalize = false)
    {
      this.name = name;
      this.size = size;
      this.type = type;
      this.stride = stride;
      this.offset = offset;
      this.normalize = normalize;
    }

    public void Set(ShaderProgram program)
    {
      // get location of attribute from shader program
      int index = program.GetAttributeLocation(this.name);

      // enable and set attribute
      GL.EnableVertexAttribArray(index);
      GL.VertexAttribPointer(index, this.size, this.type,
          this.normalize, this.stride, this.offset);
    }
  }

  class Shader
  {
    private readonly int handle;

    public int Handle { get { return this.handle; } }

    public Shader(ShaderType type, string code)
    {
      // create shader object
      this.handle = GL.CreateShader(type);

      // set source and compile shader
      GL.ShaderSource(this.handle, code);
      GL.CompileShader(this.handle);
    }
  }
  sealed class ShaderProgram
  {
    private readonly int handle;

    public ShaderProgram(params Shader[] shaders)
    {
      // create program object
      this.handle = GL.CreateProgram();

      // assign all shaders
      foreach (var shader in shaders)
        GL.AttachShader(this.handle, shader.Handle);

      // link program (effectively compiles it)
      GL.LinkProgram(this.handle);

      // detach shaders
      foreach (var shader in shaders)
        GL.DetachShader(this.handle, shader.Handle);
    }

    public int GetAttributeLocation(string name)
    {
      // get the location of a vertex attribute
      return GL.GetAttribLocation(this.handle, name);
    }

    public int GetUniformLocation(string name)
    {
      // get the location of a uniform variable
      return GL.GetUniformLocation(this.handle, name);
    }

    public void Use()
    {
      // activate this program to be used
      GL.UseProgram(this.handle);
    }
  }
}
