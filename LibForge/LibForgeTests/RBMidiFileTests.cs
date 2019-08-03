using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibForge.Midi;
using MidiCS;
using System.IO;

namespace LibForgeTests
{
  [TestClass]
  public class RBMidiFileTests
  {
    static System.IO.Stream GetFile(string name) => System.IO.File.OpenRead("TestData\\" + name);
    static RBMid GetMid(string name) { using (var s = GetFile(name)) return RBMidConverter.ToRBMid(MidiFileReader.FromStream(GetFile(name))); }
    [TestMethod]
    public void TestRBMidiFileRoundTrip()
    {
      RBMid rbmid = GetMid("hopos.mid");
      try
      {
        using (var ms = new MemoryStream())
        {
          RBMidWriter.WriteStream(rbmid, ms);
          ms.Position = 0;
          var rbmid2 = new RBMidReader(ms).Read();
          var diff = rbmid.Compare(rbmid2);
          Assert.AreEqual(null, diff, "RBMid differed after round-trip at element " + diff);
        }
      }
      catch(Exception e)
      {
        Assert.Fail("Exception occurred writing or reading an RBMid: " + e.Message);
      }
    }
  }
}
