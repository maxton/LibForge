using LibForge.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LibForge.Fuser
{
  public class FusionPatch : IDataSerializable
  {
    public int Version { get; set; }
    public enum DecompressOnLoadSetting
    {
      Yes, No, AsPerGame
    };
    public DecompressOnLoadSetting DecompressOnLoad { get; set; }
    public List<Keyzone> Keymap { get; set; }
    public Dictionary<string, Preset> Presets { get; set; }
  }
  public class PanConfig : IDataSerializable
  {
    public string Mode { get; set; }
    public float Position { get; set; }
  }
  public class Keyzone : IDataSerializable
  {
    public string SamplePath { get; set; }
    public int StartOffsetFrame { get; set; }
    public int EndOffsetFrame { get; set; }
    public int RootNote { get; set; }
    public int MinNote { get; set; }
    public int MaxNote { get; set; }
    public int Unpitched { get; set; }
    public int VelocityToVolume { get; set; }
    public class TimestretchSettingsConfig : IDataSerializable
    {
      public int MaintainTime { get; set; }
      public string Algorithm { get; set; }
      public int MaintainFormant { get; set; }
      public float SyncTempo { get; set; }
      public float OrigTempo { get; set; }
      public int EnvelopeOrder { get; set; }
    }
    public TimestretchSettingsConfig TimestretchSettings { get; set; }
    public float RandomWeight { get; set; }
    public int MinVelocity { get; set; }
    public int MaxVelocity { get; set; }
    public float Volume { get; set; }
    public PanConfig Pan { get; set; }
    public float FineTune { get; set; }
    public int IsNoteOffZone { get; set; }
    public int Priority { get; set; }
    public int Singleton { get; set; }
  }

  public class Preset : IDataSerializable
  {

  }
}
