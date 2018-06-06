using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LibForge.Lipsync
{
  public struct KeyFrame
  {
    public List<VisemeEvent> Events { get; set; }
    public override string ToString() => string.Join(", ", Events.Select(x => x.ToString()));
  }
}
