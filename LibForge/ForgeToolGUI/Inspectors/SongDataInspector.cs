using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ForgeToolGUI
{
  public partial class SongDataInspector : Inspector
  {
    public SongDataInspector(LibForge.SongData.SongData data)
    {
      InitializeComponent();
      if(data != null)
        UpdateValues(data);
    }
    public void UpdateValues(LibForge.SongData.SongData data)
    {
      songIdTextBox.Text = data.SongId.ToString();
      gameOriginTextBox.Text = data.GameOrigin;
      versionTextBox.Text = data.Version.ToString();
      nameTextBox.Text = data.Name;
      artistTextBox.Text = data.Artist;
      albumTextBox.Text = data.AlbumName;
      yearTextBox.Text = data.AlbumYear.ToString();
      origYearTextBox.Text = data.OriginalYear.ToString();
      genreTextBox.Text = data.Genre;
      trackTextBox.Text = data.AlbumTrackNumber.ToString();
      songLengthTextBox.Text = data.SongLength.ToString();
      previewStartTextBox.Text = data.PreviewStart.ToString();
      previewEndTextBox.Text = data.PreviewEnd.ToString();
      guitarRankTextBox.Text= data.GuitarRank.ToString();
      bassRankTextBox.Text = data.BassRank.ToString();
      voxRankTextBox.Text = data.VocalsRank.ToString();
      drumsRankTextBox.Text = data.DrumRank.ToString();
      bandRankTextBox.Text = data.BandRank.ToString();
      tutorialCheckBox.Checked = data.Tutorial;
      artCheckBox.Checked = data.AlbumArt;
      coverCheckBox.Checked = data.Cover;
      vocalPartsTextBox.Text = data.VocalParts.ToString();
      vocalGenderTextBox.Text = data.VocalGender.ToString();
      mediumTextBox.Text = data.Medium;
      freestyleCheckBox.Checked = data.HasFreestyleVocals;
      fakeCheckBox.Checked = data.Fake;
      shortnameTextBox.Text = data.Shortname;
    }
  }
}
