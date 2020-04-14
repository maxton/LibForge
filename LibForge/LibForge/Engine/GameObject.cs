using LibForge.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace LibForge.Engine
{
  public struct GameObjectId
  {
    public short Layer;
    public int Index;
  }
  public class GameObject
  {
    public const int MaxVersion = 0x4;
    public GameObjectId Id;
    public int Rev;
    public string Name;
    public Component[] Components;

    public void Load(Stream s, int layerVersion)
    {
      var r = new BinReader(s);
      var id = r.Int();
      if (id == -1) return;
      Id = new GameObjectId
      {
        Index = id & 0xFFF,
        Layer = (short)(id >> 16), // Some places say this should be >> 12, but that doesn't make sense with any files
      };
      Rev = r.Int();
      if (Rev < 0)
      {
        Name = new string((char)r.Byte(), 1);
      }
      else
      {
        Name = r.String();
      }

      if (Rev >= 3 && Name.Length == 0)
      {
        // TODO: Newer GameObject unknown stuff
        r.Int();
        r.Int();
        if (Rev >= 4)
        {
          r.Int();
        }
        Components = new Component[] { };
        return;
      }
      var numChildren = r.Int();
      Components = new Component[numChildren];
      for (int i = 0; i < numChildren; i++)
      {
        Components[i] = new Component();
        Components[i].Load(s, Rev, layerVersion);
      }
    }
  }
}
