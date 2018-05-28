using System.IO;
using LibForge.Util;

namespace LibForge.SongData
{
  public class SongDataWriter : WriterBase<SongData>
  {
    public static void WriteStream(SongData r, Stream s)
    {
      new SongDataWriter(s).WriteStream(r);
    }
    private SongDataWriter(Stream s) : base(s) { }
    public override void WriteStream(SongData r)
    {
      Write(r.Type);
      Write(r.SongId);
      Write(r.Version);
      Write(r.GameOrigin, 18);
      Write(r.PreviewStart);
      Write(r.PreviewEnd);
      Write(r.Name, 256);
      Write(r.Artist, 256);
      Write(r.AlbumName, 256);
      Write(r.AlbumTrackNumber);
      s.Position += 2;
      Write(r.AlbumYear);
      Write(r.OriginalYear);
      Write(r.Genre, 64);
      Write(r.SongLength);
      Write(r.GuitarRank);
      Write(r.BassRank);
      Write(r.VocalsRank);
      Write(r.DrumRank);
      Write(r.BandRank);
      Write(r.KeysRank);
      Write(r.RealKeysRank);
      Write(r.Tutorial);
      Write(r.AlbumArt);
      Write(r.Cover);
      Write(r.VocalGender);
      Write(r.Medium, 16);
      Write(r.HasFreestyleVocals);
      s.Position += 3;
      Write(r.VocalParts);
      Write(r.Flags);
      Write(r.Fake);
      Write(r.Shortname, 256);
      s.WriteByte(0);
    }
  }
}
