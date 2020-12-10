# FUSER Song Formats

DLC Songs are stored in the path /Game/DLC/Songs

Each song has its own directory with the song's shortname.
This has the metadata and textures.
In each subfolder, the instrument data is stored.

```
`-- shortname
    |-- Meta_shortname.[uasset|uexp]
    |-- T_ShortName_Large.[uasset|uexp]
    |-- T_ShortName_Small.[uasset|uexp]
    `-- shortname[bt|bs|ld|lp]
        `-- [...]
```

Most songs have 4 instruments: bt (beat), bs (bass), ld (lead), lp (loop).
Beat is generally drums, bass is generally bass, lead is generally vocals,
and loop is generally guitars/synths.

## Instrument

Let's look at the directory structure for an instrument in detail.
Each instrument has a "normal" variation and a "trans" variation (for transitioning between songs?)

```
`-- shortnamebt
    |-- Meta_shortnamebt.[uasset|uexp]
    |-- Meta_shortnamebt_trans.[uasset|uexp]
    |-- shortnamebt_midisong.[uasset|uexp]
    |-- shortnamebt_trans_midisong.[uasset|uexp]
    |-- midi
    |   |-- ShortName_INST_Key_Tempo_mid.[uasset|uexp]
    |   |-- ShortName_INST_Key2_Tempo_mid.[uasset|uexp]
    |   |-- trans_ShortName_INST_Key_Tempo_mid.[uasset|uexp]
    |   `-- trans_ShortName_INST_Key2_Tempo_mid.[uasset|uexp]
    `-- patches
        |-- ShortName_INST_Key_Tempo_fusion.[uasset|uexp]
        `-- trans_ShortName_INST_Key_Tempo_fusion.[uasset|uexp]
```

It looks like a lot but really it's kind of simple, and similar to
other games with Harmonix's audio engine, just wrapped in Unreal formats
rather than Ark and DTA files.

## Unreal Assets
Aside from the meta assets, there's really only 3 kinds of asset, and
they are pretty much just containers for Resource files.

### HmxMidiSongAsset
Links Midi files and tracks with their corresponding Fusion patches.
#### MidiMusicResource

### HmxMidiFileAsset
Contains MIDI sequence data.
#### MidiFileResource

### HmxFusionAsset
Contains audio samples and Fusion patches (which contain key-maps for the samples,
as well as optional effects for the audio engine to apply).
#### MoggSampleResource
#### FusionPatchResource
