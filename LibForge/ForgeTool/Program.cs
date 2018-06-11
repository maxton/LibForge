using System;
using System.Diagnostics;
using System.IO;
using LibForge.Lipsync;
using LibForge.Mesh;
using LibForge.Midi;
using LibForge.Milo;
using LibForge.Texture;

namespace ForgeTool
{
  class Program
  {
    static void Main(string[] args)
    {
      if (args.Length < 1)
      {
        Usage();
        return;
      }
      void WithIO(Action<Stream, Stream> action)
      {
        using (var fi = File.OpenRead(args[1]))
        using (var fo = File.OpenWrite(args[2]))
          action(fi, fo);
      }
      switch (args[0])
      {
        case "rbmid2mid":
          WithIO((fi, fo) =>
          {
            var rbmid = RBMidReader.ReadStream(fi);
            var midi = RBMidConverter.ToMid(rbmid);
            MidiCS.MidiFileWriter.WriteSMF(midi, fo);
          });
          break;
        case "mid2rbmid":
          WithIO((fi, fo) =>
          {
            var mid = MidiCS.MidiFileReader.FromStream(fi);
            var rbmid = RBMidConverter.ToRBMid(mid);
            RBMidWriter.WriteStream(rbmid, fo);
          });
          break;
        case "reprocess":
          WithIO((fi, fo) =>
          {
            var rbmid = RBMidReader.ReadStream(fi);
            var processed = RBMidConverter.ToRBMid(RBMidConverter.ToMid(rbmid));
            RBMidWriter.WriteStream(processed, fo);
          });
          break;
        case "tex2png":
          WithIO((fi, fo) =>
          {
            var tex = TextureReader.ReadStream(fi);
            var bitmap = TextureConverter.ToBitmap(tex, 0);
            bitmap.Save(fo, System.Drawing.Imaging.ImageFormat.Png);
          });
          break;
        case "mesh2obj":
          {
            var input = args[1];
            var output = args[2];
            using (var fi = File.OpenRead(input))
            {
              var mesh = HxMeshReader.ReadStream(fi);
              var obj = HxMeshConverter.ToObj(mesh);
              File.WriteAllText(output, obj);
            }
            break;
          }
        case "milo2lip":
        case "milo2lips":
        case "milo2lipsync":
          WithIO((fi, fo) =>
          {
            var milo = MiloFile.ReadFromStream(fi);
            var lipsync = LipsyncConverter.FromMilo(milo);
            new LipsyncWriter(fo).WriteStream(lipsync);
          });
          break;
        case "version":
          var assembly = System.Reflection.Assembly.GetExecutingAssembly();
          var version = FileVersionInfo.GetVersionInfo(assembly.Location).FileVersion;
          var libAssembly = System.Reflection.Assembly.GetAssembly(typeof(RBMid));
          var libVersion = FileVersionInfo.GetVersionInfo(libAssembly.Location).FileVersion;
          Console.WriteLine($"ForgeTool v{version}");
          Console.WriteLine($"LibForge v{libVersion}");
          break;
        case "test":
          {
            var dir = args[1];
            int succ = 0, warn = 0, fail = 0;
            var files = dir.EndsWith(".rbmid_ps4") || dir.EndsWith(".rbmid_pc") ?
              new[] { dir } : Directory.EnumerateFiles(dir, "*.rbmid_*");
            foreach (var f in files)
            {
              var info = new FileInfo(f);
              var name = info.Name;
              using (var fi = File.OpenRead(f))
              {
                try
                {
                  var rbmid = RBMidReader.ReadStream(fi);
                  var midi = RBMidConverter.ToMid(rbmid);
                  var rbmid2 = RBMidConverter.ToRBMid(midi);
                  using (var ms = new MemoryStream((int)fi.Length))
                  {
                    // TODO: Switch this to rbmid2 once we are generating all events
                    //       (regardless of whether the event data are all correct)
                    RBMidWriter.WriteStream(rbmid, ms);
                    ms.Position = 0;
                    if (ms.Length == fi.Length)
                    {
                      var comparison = rbmid.Compare(rbmid2);
                      if (comparison != null)
                      {
                        throw new CompareException("File comparison failed at field: " + comparison);
                      }
                      Console.WriteLine($"[OK] {name}");
                      succ++;
                    }
                    else
                    {
                      Console.Write($"[WARN] {name}:");
                      Console.WriteLine($" Processed file had different length ({fi.Length} orig, {ms.Length} processed)");
                      warn++;
                    }
                  }
                } catch (CompareException e)
                {
                  Console.WriteLine($"[WARN] {name}: {e.Message}");
                  warn++;
                }
                catch (Exception e)
                {
                  Console.WriteLine($"[ERROR] {name}: {e.Message}");
                  Console.WriteLine(e.StackTrace);
                  fail++;
                }
              }
            }
            Console.WriteLine($"Summary: {succ} OK, {warn} WARN, {fail} ERROR");
            if (fail > 0)
            {
              Console.WriteLine("(a = converted file, b = original)");
            }
          }
          break;
        case "simpletest":
          {
            var dir = args[1];
            int succ = 0, warn = 0, fail = 0;
            foreach (var f in Directory.EnumerateFiles(dir, "*.rbmid_*"))
            {
              var info = new FileInfo(f);
              var name = info.Name;
              using (var fi = File.OpenRead(f))
              {
                try
                {
                  var rbmid = RBMidReader.ReadStream(fi);
                  using (var ms = new MemoryStream((int)fi.Length))
                  {
                    RBMidWriter.WriteStream(rbmid, ms);
                    ms.Position = 0;
                    if (ms.Length == fi.Length)
                    {
                      succ++;
                    }
                    else
                    {
                      Console.WriteLine($"[WARN] {name}:");
                      Console.WriteLine($"  Processed file had different length ({fi.Length} orig, {ms.Length} processed)");
                      warn++;
                    }
                  }
                }
                catch (Exception e)
                {
                  Console.WriteLine($"[ERROR] {name}:");
                  Console.WriteLine("  " + e.Message);
                  fail++;
                }
              }
            }
            Console.WriteLine($"Summary: {succ} OK, {warn} WARN, {fail} ERROR");
          }
          break;
        case "con2gp4":
          LibForge.PkgCreator.ConToGp4(args[1], args[2]);
          break;
        case "con2pkg":
          var conFilename = Path.GetFileName(args[2]);
          var tmpDir = Path.Combine(Path.GetTempPath(), conFilename + "_tmp_build");
          LibForge.PkgCreator.ConToGp4(args[2], tmpDir);
          LibForge.PkgCreator.BuildPkg(args[1], Path.Combine(tmpDir, "project.gp4"), args[3]);
          Directory.Delete(tmpDir, true);
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
      Console.WriteLine("  version");
      Console.WriteLine("    - Prints the version number and exits");
      Console.WriteLine("  rbmid2mid <input.rbmid> <output.mid>");
      Console.WriteLine("   - converts a Forge midi to a Standard Midi File");
      Console.WriteLine("  reprocess <input.rbmid> <output.rbmid>");
      Console.WriteLine("   - converts a Forge midi to a Forge midi");
      Console.WriteLine("  mid2rbmid <input.mid> <output.rbmid>");
      Console.WriteLine("   - converts a Standard Midi File to a Forge midi");
      Console.WriteLine("  tex2png <input.png/bmp_pc/ps4> <output.png>");
      Console.WriteLine("   - converts a Forge texture to PNG");
      Console.WriteLine("  mesh2obj <input.fbx...> <output.obj>");
      Console.WriteLine("   - converts a Forge mesh to OBJ");
      Console.WriteLine("  con2gp4 <input_con> <output_dir>");
      Console.WriteLine("   - converts a CON custom to a .gp4 project in the given output directory");
      Console.WriteLine("  con2pkg <path_to_pub_cmd.exe> <input_con> <output_dir>");
      Console.WriteLine("   - converts a CON custom to a PS4 PKG custom in the given output directory");
      Console.WriteLine("  milo2lipsync <input.milo_xbox> <output.lipsync>");
      Console.WriteLine("   - converts an uncompressed milo archive to forge lipsync file");
    }
  }
  class CompareException : Exception {
    public CompareException(string msg) : base(msg) { }
  }
}
