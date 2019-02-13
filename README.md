# About This Repository

I am keeping track of my research into RB4 customs here.

I am working out the structure of the files in Rock Band 4. These are being documented in the form of 010 Editor Template files, which are in the `010` directory.

# Latest Build

Can be downloaded at [Appveyor](https://ci.appveyor.com/project/maxton/libforge/branch/master/artifacts)

# LibForge

This is a library I'm working on that handles reading, writing, and converting for formats in the Forge engine used by Rock Band 4 and Rock Band VR.

It is licensed under the GNU LGPLv3 and includes two frontends at the moment:

## ForgeTool

This is a command line tool that does file conversions.

```
Usage: ForgeTool.exe <verb> [options]
Verbs:
  version
    - Prints the version number and exits
  rbmid2mid <input.rbmid> <output.mid>
   - converts a Forge midi to a Standard Midi File
  reprocess <input.rbmid> <output.rbmid>
   - converts a Forge midi to a Forge midi
  mid2rbmid <input.mid> <output.rbmid>
   - converts a Standard Midi File to a Forge midi
  tex2png <input.png/bmp_pc/ps4> <output.png>
   - converts a Forge texture to PNG
  mesh2obj <input.fbx...> <output.obj>
   - converts a Forge mesh to OBJ
  con2gp4 [--scee] [--id 16CHARIDENTIFIER] [--desc "Package Description"] <input_con> <output_dir>
   - converts a CON custom to a .gp4 project in the given output directory
       --scee : make an EU package
       --id <16CHARIDENTIFIER> : set the customizable part of the Package ID/Filename
       --desc "Package Description" : set the description of the package
  con2pkg [--scee] [--id 16CHARIDENTIFIER] [--desc "Package Description"] <input_con> <output_dir>
   - converts a CON custom to a PS4 PKG custom in the given output directory
       --scee : make an EU package
       --id <16CHARIDENTIFIER> : set the customizable part of the Package ID/Filename
       --desc "Package Description" : set the description of the package
  milo2lipsync <input.milo_xbox> <output.lipsync>
   - converts an uncompressed milo archive to forge lipsync file
```

## ForgeToolGUI

This has an ark/PFS/folder browser with support for previewing the following:
  - Textures (.png_pc, .png_ps4, .bmp_pc, .bmp_ps4, etc)
  - Models (a 3d preview with optional wireframe)
  - .songdta files
  - Data (.dta/.dtb/.\*\_dta\_\*/.moggsong) files
  - RBmid files
  - RBsong files