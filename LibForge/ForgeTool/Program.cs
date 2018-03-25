using System;
using System.IO;
using LibForge.Midi;

namespace ForgeTool
{
  class Program
  {
    static void Main(string[] args)
    {
      if(args.Length < 1)
      {
        Usage();
        return;
      }
      switch (args[0])
      {
        case "rbmid2mid":
          {
            var input = args[1];
            var output = args[2];
            using (var fi = File.OpenRead(input))
            using (var fo = File.OpenWrite(output))
            {
              var rbmid = RBMidReader.ReadStream(fi);
              var midi = rbmid.ToMidiFile();
              MidiCS.MidiFileWriter.WriteSMF(midi, fo);
            }
          }
          break;
        case "reprocess":
          {
            var input = args[1];
            var output = args[2];
            using (var fi = File.OpenRead(input))
            using (var fo = File.OpenWrite(output))
            {
              var rbmid = RBMidReader.ReadStream(fi);
              RBMidWriter.WriteStream(rbmid, fo);
            }
          }
          break;
        default:
          Usage();
          break;
      }
    }

    static void Usage()
    {
      Console.WriteLine("Usage: ForgeTool.exe <verb> [options]");
      Console.WriteLine("Verbs: ");
      Console.WriteLine("  rbmid2mid <input.rbmid> <output.mid>");
      Console.WriteLine("   - converts a Forge midi to a Standard Midi File");
      Console.WriteLine("  reprocess <input.rbmid> <output.rbmid>");
      Console.WriteLine("   - converts a Forge midi to a Forge midi");
    }
  }
}
