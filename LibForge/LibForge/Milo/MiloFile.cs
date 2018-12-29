using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using LibForge.Extensions;

namespace LibForge.Milo
{
  // Subset of code taken from Mackiloha library
  public class MiloFile
  {
    private const uint ADDE_PADDING = 0xADDEADDE;

    private MiloFile(string name, string type)
    {
      Name = name;
      Type = type;
      Entries = new List<IMiloEntry>();
    }

    public static MiloFile ReadFromStream(Stream stream)
    {
      long startingOffset = stream.Position; // You might not be starting at 0x0
      var structureType = (BlockStructure)stream.ReadUInt32LE();

      if (!(structureType == BlockStructure.MILO_A || structureType == BlockStructure.MILO_D))
        throw new Exception("Unsupported milo compression");

      int offset = stream.ReadInt32LE(); // Start of blocks
      int blockCount = stream.ReadInt32LE();
      stream.Seek(4, SeekOrigin.Current); // Skips max uncompressed size

      // Reads block sizes
      int totalSize;
      if (structureType == BlockStructure.MILO_A)
      {
        totalSize = Enumerable.Range(0, blockCount)
          .Select(x => stream.ReadInt32LE())
          .Sum();
      }
      else // Milo_D
      {
        // TODO: Implement zlib block inflation
        totalSize = Enumerable.Range(0, blockCount)
          .Select(x =>
          {
            var blockSize = stream.ReadInt32LE();
            var compressed = (blockSize & 0x01_00_00_00) == 0;
            
            if (compressed)
              throw new NotImplementedException("Zlib block inflation not implemented yet");
          
            return blockSize & 0xFF_FF_FF;
          })
          .Sum();
      }

      // Jumps to first block offset
      stream.Position = startingOffset + offset;

      using (var ms = new MemoryStream()) 
      {
        // Copies raw milo data
        stream.CopyTo(ms, totalSize);
        ms.Seek(0, SeekOrigin.Begin);
        return ParseDirectory(ms);
      }
    }

    private static MiloFile ParseDirectory(Stream stream)
    {
      int version = stream.ReadInt32BE();

      if (!(version == 25 || version == 28)) // RBN1 and RBN2 milos
        throw new NotSupportedException($"Milo directory version of {version} is not supported");

      string dirType = stream.ReadLengthUTF8(true), dirName = stream.ReadLengthUTF8(true);
      MiloFile milo = new MiloFile(dirType, dirName);

      stream.Position += 8; // Skips string count + total length

      // Reads entry types/names
      int count = stream.ReadInt32BE();
      string[] types = new string[count];
      string[] names = new string[count];

      for (int i = 0; i < count; i++)
      {
        types[i] = stream.ReadLengthUTF8(true);
        names[i] = stream.ReadLengthUTF8(true);
      }

      // Skips unknown data
      var next = FindNext(stream, ADDE_PADDING);
      stream.Seek(next + 4, SeekOrigin.Current);

      // Reads each file
      for (int i = 0; i < names.Length; i++)
      {
        long start = stream.Position;
        int size = FindNext(stream, ADDE_PADDING);
        byte[] data = stream.ReadBytes(size);
        stream.Position += 4;

        //milo.Entries.Add(new MiloEntry(names[i], types[i], stream.ReadBytes(size)));

        if (types[i].Equals("CharLipSync", StringComparison.CurrentCultureIgnoreCase))
        {
          // Parse LipSync data
          using (var ms = new MemoryStream(data))
          {
            CharLipSync lipsync = CharLipSync.FromStream(ms, names[i]);
            milo.Entries.Add(lipsync);
          }
        }
      }

      return milo;
    }

    private static int FindNext(Stream stream, uint magic)
    {
      long start = stream.Position, currentPosition = stream.Position;
      uint currentMagic = 0;

      while (magic != currentMagic)
      {
        if (stream.Position == stream.Length)
        {
          // Couldn't find it
          stream.Seek(start, SeekOrigin.Begin);
          return -1;
        }

        currentMagic = (uint)((currentMagic << 8) | stream.ReadByte());
        currentPosition++;
      }
      
      stream.Seek(start, SeekOrigin.Begin);
      return (int)((currentPosition - 4) - start);
    }

    public string Name { get; }
    public string Type { get; }

    public List<IMiloEntry> Entries { get; }
  }
}
