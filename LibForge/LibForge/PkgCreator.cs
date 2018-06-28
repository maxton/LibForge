using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using DtxCS;
using DtxCS.DataTypes;
using GameArchives.STFS;
using LibForge.Lipsync;
using LibForge.Midi;
using LibForge.Milo;
using LibForge.RBSong;
using LibForge.SongData;

namespace LibForge.Util
{
  public static class PkgCreator
  {
    static byte[] paramSFO = {
      0x00, 0x50, 0x53, 0x46, 0x01, 0x01, 0x00, 0x00, 0x84, 0x00, 0x00, 0x00, 0xC0, 0x00, 0x00, 0x00,
      0x07, 0x00, 0x00, 0x00, 0x00, 0x00, 0x04, 0x04, 0x04, 0x00, 0x00, 0x00, 0x04, 0x00, 0x00, 0x00,
      0x00, 0x00, 0x00, 0x00, 0x0A, 0x00, 0x04, 0x02, 0x03, 0x00, 0x00, 0x00, 0x04, 0x00, 0x00, 0x00,
      0x04, 0x00, 0x00, 0x00, 0x13, 0x00, 0x04, 0x02, 0x25, 0x00, 0x00, 0x00, 0x30, 0x00, 0x00, 0x00,
      0x08, 0x00, 0x00, 0x00, 0x1E, 0x00, 0x04, 0x02, 0x04, 0x00, 0x00, 0x00, 0x04, 0x00, 0x00, 0x00,
      0x38, 0x00, 0x00, 0x00, 0x25, 0x00, 0x04, 0x02, 0x1D, 0x00, 0x00, 0x00, 0x80, 0x00, 0x00, 0x00,
      0x3C, 0x00, 0x00, 0x00, 0x2B, 0x00, 0x04, 0x02, 0x0A, 0x00, 0x00, 0x00, 0x0C, 0x00, 0x00, 0x00,
      0xBC, 0x00, 0x00, 0x00, 0x34, 0x00, 0x04, 0x02, 0x06, 0x00, 0x00, 0x00, 0x08, 0x00, 0x00, 0x00,
      0xC8, 0x00, 0x00, 0x00, 0x41, 0x54, 0x54, 0x52, 0x49, 0x42, 0x55, 0x54, 0x45, 0x00, 0x43, 0x41,
      0x54, 0x45, 0x47, 0x4F, 0x52, 0x59, 0x00, 0x43, 0x4F, 0x4E, 0x54, 0x45, 0x4E, 0x54, 0x5F, 0x49,
      0x44, 0x00, 0x46, 0x4F, 0x52, 0x4D, 0x41, 0x54, 0x00, 0x54, 0x49, 0x54, 0x4C, 0x45, 0x00, 0x54,
      0x49, 0x54, 0x4C, 0x45, 0x5F, 0x49, 0x44, 0x00, 0x56, 0x45, 0x52, 0x53, 0x49, 0x4F, 0x4E, 0x00,
      0x00, 0x00, 0x00, 0x00, 0x61, 0x63, 0x00, 0x00, 0x55, 0x50, 0x38, 0x38, 0x30, 0x32, 0x2D, 0x43,
      0x55, 0x53, 0x41, 0x30, 0x32, 0x30, 0x38, 0x34, 0x5F, 0x30, 0x30, 0x2D, 0x52, 0x42, 0x43, 0x55,
      0x53, 0x54, 0x4F, 0x4D, 0x58, 0x58, 0x58, 0x58, 0x35, 0x30, 0x30, 0x30, 0x00, 0x00, 0x00, 0x00,
      0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x6F, 0x62, 0x73, 0x00, 0x00, 0x00, 0x00, 0x00,
      0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
      0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x04, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
      0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
      0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
      0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
      0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
      0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
      0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x43, 0x55, 0x53, 0x41,
      0x30, 0x32, 0x30, 0x38, 0x34, 0x00, 0x00, 0x00, 0x30, 0x31, 0x2E, 0x30, 0x30, 0x00, 0x00, 0x00
    };
    static string gp4 = @"<?xml version=""1.0"" encoding=""utf-8"" standalone=""yes""?>
<psproject fmt=""gp4"" version=""1000"">
  <volume>
    <volume_type>pkg_ps4_ac_data</volume_type>
    <volume_id></volume_id>
    <volume_ts>2018-01-01 00:00:00</volume_ts>
    <package content_id=""UP8802-CUSA02084_00-RBCUSTOMXXXX5000"" passcode=""00000000000000000000000000000000""/>
  </volume>
  <files img_no=""0"">
    <file targ_path=""sce_sys/param.sfo"" orig_path=""param.sfo""/>
FILES  </files>
  <rootdir>
    <dir targ_name=""sce_sys""/>
    <dir targ_name=""songs"">
      <dir targ_name=""custom00001""/>
    </dir>
  </rootdir>
</psproject>";

    public static byte[] MakeParamSfo(string pkgId, string description)
    {
      var idBytes = Encoding.UTF8.GetBytes(pkgId);
      if (idBytes.Length != 36) throw new Exception("Content ID is not formatted correctly. It should be 36 characters");
      var descBytes = Encoding.UTF8.GetBytes(description);
      var param = paramSFO.ToArray();
      Array.Copy(idBytes, 0, param, 200, 36);
      Array.Copy(descBytes, 0, param, 252, Math.Min(descBytes.Length, 128));
      return param;
    }

    public static byte[] MakeGp4(string pkgId, string shortname, List<string> files)
    {
      var fileSb = new StringBuilder();
      foreach(var f in files)
      {
        fileSb.AppendLine($"    <file targ_path=\"{f}\" orig_path=\"{f.Replace('/', '\\')}\"/>");
      }
      var project = gp4.Replace("UP8802-CUSA02084_00-RBCUSTOMXXXX5000", pkgId)
                       .Replace("custom00001", shortname)
                       .Replace("FILES", fileSb.ToString())
                       .Replace("2018-01-01 00:00:00", DateTime.UtcNow.ToString());
      return Encoding.UTF8.GetBytes(project);
    }

    public static string MakeMoggDta(DataArray array)
    {
      var moggDta = new DataArray();
      var trackArray = new DataArray();
      trackArray.AddNode(DataSymbol.Symbol("tracks"));
      var trackSubArray = trackArray.AddNode(new DataArray());
      foreach (var child in array.Array("song").Array("tracks").Array(1).Children)
      {
        trackSubArray.AddNode(child);
      }
      var totalTracks = array.Array("song").Array("pans").Array(1).Children.Count;
      var lastTrack = ((trackSubArray.Children.Last() as DataArray)
        .Array(1).Children.Last() as DataAtom).Int;
      var crowdChannel = array.Array("song").Array("crowd_channels")?.Int(1);
      if (crowdChannel != null)
      {
        if (crowdChannel == lastTrack + 2)
          trackSubArray.AddNode(DTX.FromDtaString($"fake ({lastTrack + 1})"));
        else if (crowdChannel == lastTrack + 3)
          trackSubArray.AddNode(DTX.FromDtaString($"fake ({lastTrack + 1} {lastTrack + 2})"));
        trackSubArray.AddNode(DTX.FromDtaString($"crowd ({crowdChannel} {crowdChannel + 1})"));
      }
      else
      {
        if (totalTracks == lastTrack + 2)
          trackSubArray.AddNode(DTX.FromDtaString($"fake ({lastTrack + 1})"));
        else if (totalTracks == lastTrack + 3)
          trackSubArray.AddNode(DTX.FromDtaString($"fake ({lastTrack + 1} {lastTrack + 2})"));
      }
      var moggDtaStr = new StringBuilder();
      moggDtaStr.AppendLine(trackArray.ToString());
      moggDtaStr.AppendLine(array.Array("song").Array("pans").ToString());
      moggDtaStr.AppendLine(array.Array("song").Array("vols").ToString());
      return moggDtaStr.ToString();
    }
    
    // TODO: RBSONG
    public static RBSong.RBSong MakeRBSong(DataArray array)
    {
      var drumBank = array.Array("drum_bank")?.Any(1)
        .Replace("sfx", "fusion/patches")
        .Replace("_bank.milo", ".fusion") 
        ?? "fusion/patches/kit01.fusion";
      var editorComponent = new Component
      {
        ClassName = "Editor",
        Name = "Editor",
        Unknown1 = 3,
        Unknown2 = 2,
        Props = new[]
        {
          new Property("capabilities", new FlagValue(50))
        }
      };
      var vec3 = StructType.FromData(DTX.FromDtaString("define vec3 (props (x float) (y float) (z float))"));
      var xfm_type = StructType.FromData(DTX.FromDtaString(
        @"define xfm
            (props
              (basis_x vec3)
              (basis_y vec3)
              (basis_z vec3)
              (translate vec3))"));
      var entityHeaderComponent = new Component
      {
        ClassName = "EntityHeader",
        Name = "EntityHeader",
        Unknown1 = 3,
        Unknown2 = 1,
        Props = new[]
        {
          new Property("copy_on_instance", new BoolValue(true)),
          new Property("drives_parent", new BoolValue(false)),
          new Property("static", new BoolValue(false)),
          new Property("instance_polling_mode", new IntValue(0)),
          new Property("num_instances", new IntValue(0)),
          new Property("num_meshes", new IntValue(0)),
          new Property("num_particles", new IntValue(0)),
          new Property("num_propanims", new IntValue(0)),
          new Property("num_lights", new IntValue(0)),
          new Property("num_verts", new IntValue(0)),
          new Property("num_faces", new IntValue(0)),
          new Property("changelist", new IntValue(0)),
          new Property("icon_cam_initialized", new BoolValue(false)),
          new Property("icon_cam_near", new FloatValue(0.1f)),
          new Property("icon_cam_far", new FloatValue(1000f)),
          new Property("icon_cam_xfm", StructValue.FromData(xfm_type, DTX.FromDtaString(
            @"(basis_x ((x 1.0) (y 0.0) (z 0.0)))
              (basis_y ((x 0.0) (y 1.0) (z 0.0)))
              (basis_z ((x 0.0) (y 0.0) (z 1.0)))
              (translate ((x 0.0) (y 0.0) (z 0.0)))"))),
          new Property("icon_data", 
            new ArrayValue(new ArrayType{ElementType = PrimitiveType.Byte, InternalType = RBSong.DataType.Uint8 | RBSong.DataType.Array }, new Value[] { }))
        }
      };
      return new RBSong.RBSong
      {
        Version = 0xE,
        Object1 = new ObjectContainer
        {
          Unknown1 = 20,
          Unknown2 = 1,
          Unknown3 = 20,
          Unknown4 = 0,
          Unknown5 = 1,
          Entities = new[] {
            new Entity
            {
              Index0 = 0,
              Index1 = 0,
              Name = "root",
              Coms = new Component[] {
                editorComponent,
                entityHeaderComponent,
                new Component
                {
                  ClassName = "RBSongMetadata",
                  Name = "RBSongMetadata",
                  Unknown1 = 3,
                  Unknown2 = 4,
                  Props = new[]
                  {
                    new Property("tempo", new SymbolValue("medium")),
                    new Property("vocal_tonic_note", new LongValue(array.Array("vocal_tonic_note")?.Int(1) ?? 0)),
                    new Property("vocal_track_scroll_duration_ms", new LongValue(array.Array("song_scroll_speed")?.Int(1) ?? 2300)),
                    new Property("global_tuning_offset", new FloatValue(array.Array("tuning_offset_cents")?.Int(1) ?? 0)),
                    new Property("band_fail_sound_event", new SymbolValue("", true)),
                    new Property("vocal_percussion_patch", new ResourcePathValue("fusion/patches/vox_perc_tambourine.fusion", true)),
                    new Property("drum_kit_patch", new ResourcePathValue(drumBank)),
                    new Property("improv_solo_patch", new SymbolValue("gtrsolo_amer_03")),
                    new Property("dynamic_drum_fill_override", new IntValue(10)),
                    new Property("improv_solo_volume_db", new FloatValue(-9))
                  }
                },
                new Component
                {
                  ClassName = "RBVenueAuthoring",
                  Name = "RBVenueAuthoring",
                  Unknown1 = 3,
                  Unknown2 = 0,
                  Props = new[]
                  {
                    new Property("part2_instrument", new IntValue(2)),
                    new Property("part3_instrument", new IntValue(0)),
                    new Property("part4_instrument", new IntValue(1))
                  }
                }
              }
            }
          }
        },
        KV = new KeyValue
        {
          Str1 = "PropAnimResource",
          Str2 = "venue_authoring_data"
        },
        Object2 = new ObjectContainer
        {
          Unknown1 = 20,
          Unknown2 = 1,
          Unknown3 = 20,
          Unknown4 = 0,
          Unknown5 = 1,
          Entities = new[] {
            new Entity
            {
              Index0 = 0,
              Index1 = 0,
              Name = "root",
              Coms = new[]
              {
                editorComponent,
                entityHeaderComponent,
                new Component
                {
                  ClassName = "PropAnim",
                  Name = "PropAnim",
                  Unknown1 = 3,
                  Unknown2 = 0,
                  Props = StructValue.FromData(
                            StructType.FromData(DTX.FromDtaString(
                              @"(props 
                                  (frame_range_start float)
                                  (frame_range_end float)
                                  (time_units int)
                                  (is_looping bool))")),
                            DTX.FromDtaString(
                              @"(frame_range_start 3.402823E+38)
                                (frame_range_end -3.402823E+38)
                                (time_units 0)
                                (is_looping 0)")).Props

                }
              }
            }
          }
        }
      };
    }

    public static void ConToGp4(string conPath, string buildDir)
    {
      // Phase 1: Reading from CON
      var con = STFSPackage.OpenFile(GameArchives.Util.LocalFile(conPath));
      if(con.Type != STFSType.CON)
      {
        Console.WriteLine("Error: given file was not a CON file");
        return;
      }
      var dta = DTX.FromPlainTextBytes(con.RootDirectory.GetFileAtPath("songs/songs.dta").GetBytes());
      if(dta.Count > 1)
      {
        Console.WriteLine("Error: only 1-song CONs are supported at this time");
        return;
      }
      var array = dta.Array(0);
      var path = array.Array("song").Array("name").String(1);
      var hopoThreshold = array.Array("song").Array("hopo_threshold")?.Int(1) ?? 170;
      var midPath = path + ".mid";
      var moggPath = path + ".mogg";
      var shortname = path.Split('/').Last();
      var artPath = $"songs/{shortname}/gen/{shortname}_keep.png_xbox";
      var miloPath = $"songs/{shortname}/gen/{shortname}.milo_xbox";
      var pkgName = shortname.ToUpper().Substring(0, Math.Min(shortname.Length, 10)).PadRight(10, 'X');
      var songId = array.Array("song_id").Node(1);
      string pkgNum = ((songId.Type == DtxCS.DataTypes.DataType.INT 
        ? (songId as DataAtom).Int 
        : (songId as DataAtom).String.GetHashCode()
        ) % 10000).ToString().PadLeft(4, '0');
      var pkgId = $"UP8802-CUSA02084_00-RB{pkgName}{pkgNum}";
      var name = array.Array("name").String(1);
      var artist = array.Array("artist").String(1);
      var pkgDesc = $"Custom: \"{name} - {artist}\"";
      var mid = MidiCS.MidiFileReader.FromBytes(con.RootDirectory.GetFileAtPath(midPath).GetBytes());
      var paramSfo = MakeParamSfo(pkgId, pkgDesc);

      // TODO: Catch possible conversion exceptions? i.e. Unsupported milo version
      var milo = MiloFile.ReadFromStream(con.RootDirectory.GetFileAtPath(miloPath).GetStream());
      var lipsync = LipsyncConverter.FromMilo(milo);
      var moggDtaStr = MakeMoggDta(array);
      var rbsong = MakeRBSong(array);
      var songDta = SongDataConverter.ToSongData(array);

      // Phase 2: Writing files
      var songPath = Path.Combine(buildDir, "songs", shortname);
      Directory.CreateDirectory(songPath);
      File.WriteAllBytes(Path.Combine(buildDir, "param.sfo"), paramSfo);
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
        RBMidWriter.WriteStream(RBMidConverter.ToRBMid(mid, hopoThreshold), rbmid);
      using (var rbsongFile = File.OpenWrite(Path.Combine(songPath, $"{shortname}.rbsong")))
        new RBSongWriter(rbsongFile).WriteStream(rbsong);
      using (var songdtaFile = File.OpenWrite(Path.Combine(songPath, $"{shortname}.songdta_ps4")))
        SongDataWriter.WriteStream(songDta, songdtaFile);
      if (songDta.AlbumArt)
      {
        var albumArt = Texture.TextureConverter.MiloPngToTexture(con.RootDirectory.GetFileAtPath(artPath).GetStream());
        using (var artFile = File.OpenWrite(Path.Combine(songPath, $"{shortname}.png_ps4")))
          Texture.TextureWriter.WriteStream(albumArt, artFile);
      }

      // Phase 3: Create project file
      var files = new List<string> {
        $"songs/{shortname}/{shortname}.lipsync_ps4",
        $"songs/{shortname}/{shortname}.mogg",
        $"songs/{shortname}/{shortname}.mogg.dta",
        $"songs/{shortname}/{shortname}.moggsong",
        $"songs/{shortname}/{shortname}.rbmid_ps4",
        $"songs/{shortname}/{shortname}.rbsong",
        $"songs/{shortname}/{shortname}.songdta_ps4",
      };
      if (songDta.AlbumArt)
      {
        files.Add($"songs/{shortname}/{shortname}.png_ps4");
      }
      File.WriteAllBytes(Path.Combine(buildDir,"project.gp4"), MakeGp4(pkgId, shortname, files));
    }

    public static void BuildPkg(string cmdExe, string proj, string outPath)
    {
      var p = new Process
      {
        StartInfo = new ProcessStartInfo
        {
          UseShellExecute = false,
          FileName = cmdExe,
          Arguments = $"img_create --oformat pkg \"{proj}\" \"{outPath}\"",
          RedirectStandardOutput = true
        },
      };
      p.Start();
      p.WaitForExit();
      Console.Write(p.StandardOutput.ReadToEnd());
    }
  }
}
