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
    const int kMeshFileVersion = 0xF; // 0xE in blu-ray version
    public override HxMesh Read()
    {
      // Note: the string doesn't have a null terminator
      Check(Encoding.UTF8.GetString(FixedArr(Byte, 8)), "HXMESH\r\n");
      // This appears to be used to verify endianness
      Check(Int(), 1);
      var version = Int();
      if (version > kMeshFileVersion)
        throw new Exception($"Unknown mesh version 0x{version:X}");
      var vertexType = Int();
      var numVerts = UInt();
      var numTris = UInt();
      if(version >= 0xC)
      {
        var unkFlag1 = Bool();
        var unkFlag2 = Bool();
        if (version >= 0xD)
        {
          var unkFlag3 = Bool();
          var unkFlag4 = Bool();
        }
      }
      if(version >= 0x3)
      {
        var keepMeshData = Bool();
      }
      var vertexUsageFlags = UInt();
      var faceUsageFlags = UInt();
      if (version > 0xA)
      {
        var unk = UInt();
      }
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
          Z = Float().Then(Skip(32)), // TODO: what are these bytes
          U1 = Half(),
          V1 = Half(),
          U2 = Half(),
          V2 = Half().Then(Skip(vertexType == 7 ? 12 : 0)) // TODO: what are these bytes
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
