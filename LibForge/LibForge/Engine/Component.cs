using LibForge.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace LibForge.Engine
{
  public class Component
  {
    public string Name1;
    public string Name2;
    public int Rev;
    public long Unknown2;
    public Property[] Props;

    public void Load(Stream s, int objRev, int layerVersion)
    {
      var r = new BinReader(s);
      Name1 = r.String();
      if (objRev >= 2)
        Name2 = r.String();
      Rev = r.Int();
      Unknown2 = r.Long();
      if (layerVersion >= 0xE)
      {
        Props = new Property[r.Int()];
        for (int i = 0; i < Props.Length; i++)
        {
          Props[i] = Property.Read(s);
        }
        foreach (var prop in Props)
        {
          prop.Value = Value.Read(s, prop.Type);
        }
      }
    }
  }
}
