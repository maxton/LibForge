# About This Repository

I am keeping track of my research into RB4 customs here.

I am working out the structure of the files in Rock Band 4. These are being documented in the form of 010 Editor Template files, which are in the `010` directory.

# LibForge

This is a library I'm working on that handles reading, writing, and converting for formats in the Forge engine used by Rock Band 4 and Rock Band VR.

It is licensed under the GNU LGPLv3 and includes two frontends at the moment:

## ForgeTool

This is a command line tool that currently does file conversions from rbmid -> mid, rbmid -> rbmid, mid -> rbmid, and {png|bmp}_{ps4|pc} -> png

## ForgeToolGUI

This has an ark file browser with texture preview, songdta preview, and DTA preview.