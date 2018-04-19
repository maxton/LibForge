using LibForge.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace LibForge.Mesh
{
  public class HxMeshReader : ReaderBase<HxMesh>
  {
    public static HxMesh ReadStream(Stream s)
    {
      return new HxMeshReader(s).Read();
    }
    public HxMeshReader(Stream s) : base(s)
    {
    }

    public override HxMesh Read()
    {
      // Note: the string doesn't have a null terminator
      Check(String(9), "HXMESH\r\n");
      Check(Int(), 1);
      var unk1 = Int();
      var unk2 = Int();
      var numVerts = UInt();
      var numTris = UInt();
      s.Position = 0x2D;
      return new HxMesh
      {
        Unknown1 = Float(),
        Unknown2 = Float(),
        Unknown3 = Float(),
        Unknown4 = Float(),
        Points = FixedArr(() => new HxMesh.Point
        {
          X = Float(),
          Y = Float(),
          Z = Float().Then(Skip(unk2 == 7 ? 52 : 40))
        }, numVerts),
        Triangles = FixedArr(() => new HxMesh.Triangle
        {
          V1 = Int(),
          V2 = Int(),
          V3 = Int()
        }, numTris)
      };
    }
  }
}
