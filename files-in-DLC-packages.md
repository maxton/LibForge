DLC Packages on the PS4 have a `songs` directory, within which is a directory
for each song in the package. There is no per-package songs.dta to describe
every song in the package, they are just enumerated based on the folders in the
`songs` folder.

Within each song's folder, there are 8 files.

## shortname.lipsync_ps4
This file seems to contain the facial animation data. Its structure is unknown.

## shortname.mogg
This file has the multitrack audio data for the song.

## shortname.mogg.dta
This file has the audio mixing data for the song (this used to be in songs.dta)
It is a plaintext-encoded DTA file.

## shortname.moggsong
This file seems to tell the game what the .mogg and .rbmid files are named. It
is a plaintext-encoded DTA file.

## shortname.png_ps4
This file has the album artwork for the song.

## shortname.rbmid_ps4
This file has the instrument authoring data (tempo, notes, lyrics, markup).

## shortname.rbsong
This file seems to contain the venue authoring data.

## shortname.songdta_ps4
This file has the song metadata (song id, name, artist, year, length, etc).
This information also used to be in the songs.dta file.
See `010/songdta.bt` for an 010 Editor template describing the known data structures in this file.