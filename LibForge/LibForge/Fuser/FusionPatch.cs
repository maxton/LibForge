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
    public enum EDecompressOnLoad
    {
      Yes, No, AsPerGame
    };
    public EDecompressOnLoad DecompressOnLoad { get; set; }
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

  public class Adsr : IDataSerializable
  {
    public int Enabled { get; set; }
    public string Target { get; set; }
    public float Attack { get; set; }
    public float AttackCurve { get; set; }
    public float Decay { get; set; }
    public float DecayCurve { get; set; }
    public float Sustain { get; set; }
    public float Release { get; set; }
    public float ReleaseCurve { get; set; }
    public float Depth { get; set; }
  }
  public class Lfo : IDataSerializable
  {
    public int Enabled { get; set; }
    public string Target { get; set; }
    public float Depth { get; set; }
    public float Frequency { get; set; }
    public enum EShape
    {
      Sine, Square, SawUp, SawDown, Triangle
    }
    public EShape Shape { get; set; }
    public int Retrigger { get; set; }
    public float InitialPhase { get; set; }
    public int BeatSync { get; set; }
    public float Tempo { get; set; }
  }
  public class Filter : IDataSerializable
  {
    public int? FilterPre { get; set; }
    public int Enabled { get; set; }
    // low-pass, high-pass, band-pass, peaking, low-shelf, high-shelf
    public string Type { get; set; }
    public float Frequency { get; set; }
    public float Q { get; set; }
    public float GainDb { get; set; }
  }
  public class PitchBend : IDataSerializable
  {
    public Tuple<float, float> Range { get; set; }
  }
  public class Modulator : IDataSerializable
  {
    public string Target { get; set; }
    public float Range { get; set; }
    public float Depth { get; set; }
  }
  public class DelayEffect : IDataSerializable
  {
    public int Enabled { get; set; }
    public int BeatSync { get; set; }
    public float Time { get; set; }
    public float WetGain { get; set; }
    public float DryGain { get; set; }
    public float Feedback { get; set; }
    public int DelayEqEnabled { get; set; }
    public int DelayEqType { get; set; }
    public float DelayEqFrequency { get; set; }
    public float DelayEqQ { get; set; }
    public int DelayLfoEnabled { get; set; }
    public int DelayLfoBeatsync { get; set; }
    public float DelayLfoRate { get; set; }
    public float DelayLfoDepth { get; set; }
    public int DelayStereoType { get; set; }
    public float DelayStereoLeft { get; set; }
    public float DelayStereoRight { get; set; }
  }
  public class BitcrushEffect : IDataSerializable
  {
    public int Enabled { get; set; }
    public float WetPercent { get; set; }
    public float CrushLevel { get; set; }
    public int SampleHold { get; set; }
  }
  public class VocoderEffect : IDataSerializable
  {
    public int Enabled { get; set; }
    public int SlaveIndex { get; set; }
    public int BandConfigIndex { get; set; }
    public float CarrierGain { get; set; }
    public float CarrierThin { get; set; }
    public float ModulatorGain { get; set; }
    public float ModulatorThin { get; set; }
    public float Attack { get; set; }
    public float Release { get; set; }
    public float HighEmphasis { get; set; }
    public float WetGain { get; set; }
    public float OutputGainDb { get; set; }
    public int Solo { get; set; }
    public class VocoderBand : IDataSerializable
    {
      public float GainDb { get; set; }
      public int Solo { get; set; }
    }
    [AnonymousList]
    public List<VocoderBand> Band { get; set; }
  }
  public class DistortionEffect : IDataSerializable
  {
    public int Enabled { get; set; }
    public float InputGainDb { get; set; }
    public float OutputGainDb { get; set; }
    public int Type { get; set; }
    public int Oversample { get; set; }
    public List<Filter> FilterSet { get; set; }
  }
  public class Portamento : IDataSerializable
  {
    public int Enabled { get; set; }
    public string Mode { get; set; }
    public float Time { get; set; }
  }

  public class Preset : IDataSerializable
  {
    public float Volume { get; set; }
    public PanConfig Pan { get; set; }
    public float StartPoint { get; set; }
    public float FineTune { get; set; }
    public int VoiceLimit { get; set; }
    public enum ELayerSelectMode
    {
      Layers, Random, RandomWithRepetition, Cycle
    }
    public ELayerSelectMode LayerSelectMode { get; set; }
    public List<Adsr> Adsrs { get; set; }
    public List<Lfo> Lfos { get; set; }
    public Filter Filter { get; set; }
    public PitchBend PitchBend { get; set; }
    public List<Modulator> Randomizers { get; set; }
    public List<Modulator> VelocityMods { get; set; }
    public DelayEffect DelayEffect { get; set; }
    public BitcrushEffect BitcrushEffect { get; set; }
    public VocoderEffect VocoderEffect { get; set; }
    public DistortionEffect DistortionEffect { get; set; }
    public Portamento Portamento { get; set; }
  }
}
