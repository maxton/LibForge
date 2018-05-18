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
      foreach (var p in mesh.Points)
      {
        sb.AppendLine($"vt {p.U1} {1 - p.V1}");
      }
      foreach (var t in mesh.Triangles)
      {
        // Writes vert/uv
        sb.AppendLine($"f {t.V1 + 1}/{t.V1 + 1} {t.V2 + 1}/{t.V2 + 1} {t.V3 + 1}/{t.V3 + 1}");
      }
      return sb.ToString();
    }
  }
}
