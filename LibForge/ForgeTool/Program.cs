using System;
using System.Linq;
using System.Diagnostics;
using System.IO;
using DtxCS;
using DtxCS.DataTypes;
using LibForge.Lipsync;
using LibForge.Mesh;
using LibForge.Midi;
using LibForge.Milo;
using LibForge.SongData;
using LibForge.Texture;
using GameArchives.STFS;

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
        case "con2pkg":
          {
            // Phase 1: Reading from CON
            var con = STFSPackage.OpenFile(GameArchives.Util.LocalFile(args[1]));
            if(con.Type != STFSType.CON)
            {
              Console.WriteLine("Error: given file was not a CON file");
              break;
            }
            var out_dir = args[2];
            var dta = DtxCS.DTX.FromPlainTextBytes(con.RootDirectory.GetFileAtPath("songs/songs.dta").GetBytes());
            if(dta.Count > 1)
            {
              Console.WriteLine("Error: only 1-song CONs are supported at this time");
              break;
            }
            var array = dta.Array(0);
            var path = array.Array("song").Array("name").String(1);
            var midPath = path + ".mid";
            var moggPath = path + ".mogg";
            var shortname = path.Split('/').Last();
            var artPath = $"songs/{shortname}/gen/{shortname}_keep.png_xbox";
            var miloPath = $"songs/{shortname}/gen/{shortname}.milo_xbox";
            var pkgName = shortname.ToUpper().Substring(0, Math.Min(shortname.Length, 10)).PadRight(10, 'X');
            var pkgNum = (array.Array("song_id").Int(1) % 10000).ToString().PadLeft(4, '0');
            var pkgId = $"UP8802-CUSA02084_00-RB{pkgName}{pkgNum}";
            var name = array.Array("name").String(1);
            var artist = array.Array("artist").String(1);
            var pkgDesc = $"Custom: \"{name} - {artist}\"";
            var mid = MidiCS.MidiFileReader.FromBytes(con.RootDirectory.GetFileAtPath(midPath).GetBytes());
            var paramSfo = PkgCreator.MakeParamSfo(pkgId, pkgDesc);

            var milo = Milo.ReadFromStream(con.RootDirectory.GetFileAtPath(miloPath).GetStream());

            // TODO: Lipsync
            var lipsync = new Lipsync
            {
              Version = 0,
              Subtype = 0,
              FrameRate = 30,
              Visemes = new string[0],
              Players = new string[0],
              FrameIndices = new uint[2] { 0, 0 },
              FrameData = new byte[0]
            };

            var moggDta = new DataArray();
            var trackArray = new DataArray();
            trackArray.AddNode(DataSymbol.Symbol("tracks"));
            var trackSubArray = trackArray.AddNode(new DataArray());
            foreach(var child in array.Array("song").Array("tracks").Array(1).Children)
            {
              trackSubArray.AddNode(child);
            }
            var totalTracks = array.Array("song").Array("pans").Array(1).Children.Count;
            var lastTrack = ((trackSubArray.Children.Last() as DataArray)
              .Array(1).Children.Last() as DataAtom).Int;
            var crowdChannel = array.Array("song").Array("crowd_channels")?.Int(1);
            if(crowdChannel != null)
            {
              if(crowdChannel == lastTrack + 2)
                trackSubArray.AddNode(DTX.FromDtaString($"fake ({lastTrack + 1})"));
              else if(crowdChannel == lastTrack + 3)
                trackSubArray.AddNode(DTX.FromDtaString($"fake ({lastTrack + 1} {lastTrack + 2})"));
              trackSubArray.AddNode(DTX.FromDtaString($"crowd ({crowdChannel} {crowdChannel + 1})"));
            } else
            {
              if (totalTracks == lastTrack + 2)
                trackSubArray.AddNode(DTX.FromDtaString($"fake ({lastTrack + 1})"));
              else if (totalTracks == lastTrack + 3)
                trackSubArray.AddNode(DTX.FromDtaString($"fake ({lastTrack + 1} {lastTrack + 2})"));
            }
            moggDta.AddNode(trackArray);
            moggDta.AddNode(array.Array("song").Array("pans"));
            moggDta.AddNode(array.Array("song").Array("vols"));
            var moggDtaStr = "";
            foreach (var arr in moggDta.Children)
              moggDtaStr += arr.ToString() + "\r\n";

            // TODO: RBSONG
            var rbsong = new LibForge.RBSong.RBSong
            {
              Structs = new LibForge.RBSong.RBSong.IUnknown[0]
            };

            // Phase 2: Writing files
            var songPath = Path.Combine(out_dir, "songs", shortname);
            Directory.CreateDirectory(songPath);
            File.WriteAllBytes(Path.Combine(out_dir, "param.sfo"), paramSfo);
            using (var lipsyncFile = File.OpenWrite(Path.Combine(songPath, $"{shortname}.lipsync_ps4")))
              new LipsyncWriter(lipsyncFile).WriteStream(lipsync);
            using (var mogg = File.OpenWrite(Path.Combine(songPath, $"{shortname}.mogg")))
            using (var conMogg = con.RootDirectory.GetFileAtPath(moggPath).GetStream())
            {
              conMogg.CopyTo(mogg);
            }

            File.WriteAllText(Path.Combine(songPath, $"{shortname}.mogg.dta"), moggDtaStr);
            File.WriteAllText(Path.Combine(songPath, shortname + ".moggsong"),
              $"(mogg_path \"{shortname}.mogg\")\r\n(midi_path \"{shortname}.rbmid\")\r\n");
            using (var rbmid = File.OpenWrite(Path.Combine(songPath, $"{shortname}.rbmid_ps4")))
              RBMidWriter.WriteStream(RBMidConverter.ToRBMid(mid), rbmid);
            using (var rbsongFile = File.OpenWrite(Path.Combine(songPath, $"{shortname}.rbsong")))
              new LibForge.RBSong.RBSongWriter(rbsongFile).WriteStream(rbsong);
            using (var songdtaFile = File.OpenWrite(Path.Combine(songPath, $"{shortname}.songdta_ps4")))
              SongDataWriter.WriteStream(SongDataConverter.ToSongData(array), songdtaFile);

            // Phase 3: Create project file
            string[] files = {
              $"songs/{shortname}/{shortname}.lipsync_ps4",
              $"songs/{shortname}/{shortname}.mogg",
              $"songs/{shortname}/{shortname}.mogg.dta",
              $"songs/{shortname}/{shortname}.moggsong",
              //$"songs/{shortname}/{shortname}.png_ps4",
              $"songs/{shortname}/{shortname}.rbmid_ps4",
              $"songs/{shortname}/{shortname}.rbsong",
              $"songs/{shortname}/{shortname}.songdta_ps4",
            };
            File.WriteAllBytes(Path.Combine(out_dir,"project.gp4"), PkgCreator.MakeGp4(pkgId, shortname, files));

            // Phase 4: Make pkg (TODO if/when open source PKG tool is released)
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
      Console.WriteLine("  con2pkg <input_con> <output_dir>");
      Console.WriteLine("   - converts a CON custom to a .gp4 project in the given output directory");
    }
  }
  class CompareException : Exception {
    public CompareException(string msg) : base(msg) { }
  }
}
