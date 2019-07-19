using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DtxCS.DataTypes;

namespace LibForge
{
  public class DLCSong
  {
    public SongData.SongData SongData;
    public Lipsync.Lipsync Lipsync;
    public Midi.RBMid RBMidi;
    public Texture.Texture Artwork;
    public GameArchives.IFile Mogg;
    public DataArray MoggSong;
    public DataArray MoggDta;
    public RBSong.RBSongResource RBSong;
  }
}
