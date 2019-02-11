using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LibForge.Mesh
{
  public class HxMesh
  {
    public struct Point
    {
      public float X, Y, Z;

      // Texture coordinates
      public float U1, V1;
      public float U2, V2;
      // other stuff TBD
    }
    public class Triangle
    {
      public int V1, V2, V3;
    }
    public Point[] Points;
    public Triangle[] Triangles;
    public float Unknown1;
    public float Unknown2;
    public float Unknown3;
    public float Unknown4;
  }
}
