using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using LibForge.RBSong;
using LibForge.Extensions;
using LibForge.Util;

namespace LibForge.Engine
{
  public abstract class Resource
  {
    public string Type;
    public string Path;
    public static Resource Create(string type)
    {
      switch (type)
      {
        case "PropAnimResource":
          return new PropAnimResource();
        case "RBSongResource":
          return new RBSongResource();
        case "EntityResource":
          return new EntityResource();
        default:
          throw new NotImplementedException("Unimplemented resource type: " + type);
      }
    }

    public virtual void Load(Stream s)
    {
    }
  }
}
