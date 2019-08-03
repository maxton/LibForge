using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MidiCS;
using LibForge.Midi;

namespace LibForgeTests
{
  [TestClass]
  public class MidiConverterTests
  {
    static System.IO.Stream GetFile(string name) => System.IO.File.OpenRead("TestData\\"+name);
    static RBMid GetMid(string name) => RBMidConverter.ToRBMid(MidiFileReader.FromStream(GetFile(name)));
    [TestMethod]
    public void TestHOPOs()
    {
      var rbmid = GetMid("hopos.mid");
      var expected =
        "nnnnnynynynnnynnnynynnnynnnynynnynyyyynn" +
        "nnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnn" +
        "nnnnnnnnnnnnnnnnnnnnnnnnnnnynnynnynnynnn" +
        "nnnnnnnnnnnnnnyynynnnnnnynynynnynn";
      var actual = new string(rbmid.GemTracks[1].Gems[3].Select(g => g.IsHopo ? 'y' : 'n').ToArray());
      Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void TestProMarkers()
    {
      var rbmid = GetMid("pro_markers.mid");
      var expected =
        "TTCCC" +
        "TTTCC" +
        "TTCTC" +
        "TTCCT" +
        "TTTTC" +
        "TTCTT" +
        "TTTCT" +
        "TTTTT" +
        "TTCTTCCTC";
      var actual = new string(rbmid.GemTracks[0].Gems[3].Select(g => g.ProCymbal == 1 ? 'C' : 'T').ToArray());
      Assert.AreEqual(expected, actual);

      var expected_2 = "012436571313260";
      var actual_2 = rbmid.ProMarkers[0].Markers.Select(m => ((int)m.Flags / 4).ToString()).Aggregate(string.Concat);
      Assert.AreEqual(expected_2, actual_2);
    }
    
    [TestMethod]
    public void TestProMarkersGuitar()
    {
      // These are probably useless on guitar, but let's just test it for completion's sake
      var rbmid = GetMid("pro_markers.mid");
      var expected =
        "TTCCC" +
        "TTCC" +
        "TTC" +
        "TTC" +
        "TT";
      var actual = new string(rbmid.GemTracks[1].Gems[3].Select(g => g.ProCymbal == 1 ? 'C' : 'T').ToArray());
      Assert.AreEqual(expected, actual);
    }
  }
}
