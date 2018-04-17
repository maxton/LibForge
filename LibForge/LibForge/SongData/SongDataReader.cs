using System.IO;
using LibForge.Util;

namespace LibForge.SongData
{
  public class SongDataReader : ReaderBase<SongData>
  {
    public static SongData ReadStream(Stream s)
    {
      return new SongDataReader(s).Read();
    }
    public SongDataReader(Stream s) : base(s) { }

    public override SongData Read()
    {
      return new SongData
      {
        Type = UInt(),
        SongId = UInt(),
        Version = Short(),
        GameOrigin = String(18),
        PreviewStart = Float(),
        PreviewEnd = Float(),
        Name = String(256),
        Artist = String(256),
        AlbumName = String(256),
        AlbumTrackNumber = Short().Then(Skip(2)),
        AlbumYear = Int(),
        OriginalYear = Int(),
        Genre = String(64),
        SongLength = Float(),
        GuitarRank = Float(),
        BassRank = Float(),
        VocalsRank = Float(),
        DrumRank = Float(),
        BandRank = Float(),
        KeysRank = Float(),
        RealKeysRank = Float(),
        Tutorial = Bool(),
        AlbumArt = Bool(),
        Cover = Bool(),
        VocalGender = Byte(),
        Medium = String(16),
        HasFreestyleVocals = Bool().Then(Skip(3)),
        VocalParts = Int(),
        Flags = Int(),
        Fake = Bool(),
        Shortname = String(256)
      };
    }
  }
}
