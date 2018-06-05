using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LibForge.Milo
{
  public enum BlockStructure : uint
  {
    /// <summary>
    /// Structured as milo (No compression)
    /// </summary>
    MILO_A = 0xCABEDEAF,
    /// <summary>
    /// Structured as milo (Compressed with ZLib)
    /// </summary>
    MILO_B = 0xCBBEDEAF,
    /// <summary>
    /// Structured as milo (Compressed with GZip)
    /// </summary>
    MILO_C = 0xCCBEDEAF,
    /// <summary>
    /// Structured as milo (Compressed with ZLib)
    /// </summary>
    MILO_D = 0xCDBEDEAF
  }
}
