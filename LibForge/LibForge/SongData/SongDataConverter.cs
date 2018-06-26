using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DtxCS.DataTypes;

namespace LibForge.SongData
{
  public class SongDataConverter
  {
    public static SongData ToSongData(DataArray songDta)
    {
      return new SongData
      {
        AlbumArt = songDta.Array("album_art").Int(1) != 0,
        AlbumName = songDta.Array("album_name").String(1),
        AlbumTrackNumber = (short)songDta.Array("album_track_number").Int(1),
        AlbumYear = songDta.Array("year_released").Int(1),
        Artist = songDta.Array("artist").String(1),
        BandRank = songDta.Array("rank").Array("band").Int(1),
        BassRank = songDta.Array("rank").Array("bass").Int(1),
        DrumRank = songDta.Array("rank").Array("drum").Int(1),
        GuitarRank = songDta.Array("rank").Array("guitar").Int(1),
        KeysRank = songDta.Array("rank").Array("keys").Int(1),
        RealKeysRank = songDta.Array("rank").Array("real_keys").Int(1),
        VocalsRank = songDta.Array("rank").Array("vocals").Int(1),
        Cover = false,
        Fake = false,
        Flags = 0,
        GameOrigin = songDta.Array("game_origin")?.Any(1) ?? "ugc_plus",
        Genre = songDta.Array("genre").Symbol(1).ToString(),
        HasFreestyleVocals = false,
        Medium = "",
        Name = songDta.Array("name").String(1),
        OriginalYear = songDta.Array("year_released").Int(1),
        Tutorial = false,
        Type = 11,
        Version = -1,
        VocalGender = (byte)(songDta.Array("vocal_gender").Any(1) == "male" ? 1 : 2),
        VocalParts = songDta.Array("song").Array("vocal_parts").Int(1),
        Shortname = songDta.Symbol(0).ToString().ToLower(),
        SongId = (uint)songDta.Array("song_id").Int(1),
        SongLength = songDta.Array("song_length").Int(1),
        PreviewStart = songDta.Array("preview").Int(1),
        PreviewEnd = songDta.Array("preview").Int(2),
      };
    }
  }
}
