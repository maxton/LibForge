using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Linq;
using System.Text;

namespace LibForge
{
  public static class Meta
  {
    public static string BuildString => Assembly.GetAssembly(typeof(Meta)).GetName().Version.ToString();
  }
}
