using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MidiCS;
using LibForge.Midi;

namespace LibForgeTests
{
  [TestClass]
  public class GuitarTests
  {
    static System.IO.Stream GetFile(string name) => System.IO.File.OpenRead("TestData\\"+name);

    [TestMethod]
    public void TestHOPOs()
    {
      var rbmid = RBMidConverter.ToRBMid(MidiFileReader.FromStream(GetFile("hopos.mid")));
      var expected =
        "nnnnnynynynnnynnnynynnnynnnynynnynnnnnnn" +
        "nnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnn" +
        "nnnnnnnnnnnnnnnnnnnnnnnnnnnynnynnynnynnn" +
        "nnnnnnnnnnnnnnyynyn";
      var actual = new string(rbmid.GemTracks[1].Gems[3].Select(g => g.IsHopo ? 'y' : 'n').ToArray());
      Assert.AreEqual(expected, actual);
    }
  }
}
