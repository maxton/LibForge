using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LibForge.Mesh
{
  public class HxMesh
  {
    public class Point
    {
      public float X, Y, Z;
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
