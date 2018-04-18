using LibForge.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace LibForge.Mesh
{
  public class HxMeshWriter : WriterBase<HxMesh>
  {
    public HxMeshWriter(Stream s) : base(s)
    {
    }

    public override void WriteStream(HxMesh v)
    {
      throw new NotImplementedException();
    }
  }
}
