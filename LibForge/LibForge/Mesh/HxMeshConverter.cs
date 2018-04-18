using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LibForge.Mesh
{
  public class HxMeshConverter
  {
    public static string ToObj(HxMesh mesh)
    {
      var sb = new StringBuilder();
      foreach (var p in mesh.Points)
      {
        sb.AppendLine($"v {p.X} {p.Y} {p.Z}");
      }
      foreach (var t in mesh.Triangles)
      {
        sb.AppendLine($"f {t.V1 + 1} {t.V2 + 1} {t.V3 + 1}");
      }
      return sb.ToString();
    }
  }
}
