using GameArchives;
using LibForge.Extensions;
using LibForge.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace LibForge.Ark
{
  public class ArkBuilder
  {
    private string name;
    private string[] arkNames;
    private uint[] arkSizes;
    private int[] hashTable;
    private List<List<Tuple<string, IFile>>> layout;
    private FileEntry[] entries;

    /// <summary>
    /// Constructs a new Ark Builder.
    /// </summary>
    /// <param name="name">Name of the archive files. Example: "main_pc"</param>
    /// <param name="ArkFiles">Ordered list of ark files; each of which is a list of ark path / file pairs.</param>
    public ArkBuilder(string name, List<List<Tuple<string, IFile>>> ArkFiles)
    {
      this.name = name;
      layout = ArkFiles;
      arkNames = new string[layout.Count];
      arkSizes = new uint[layout.Count];
      entries = new FileEntry[layout.Sum(x => x.Count)];
      int i = 0;
      int entryIndex = 0;
      long arkOffset = 0;
      foreach (var ark in layout)
      {
        foreach(var (path,file) in layout[i])
        {
          entries[entryIndex++] = new FileEntry()
          {
            Flags = -1,
            Size = (uint)file.Size,
            Offset = arkOffset,
            Path = path
          };
          arkOffset += file.Size;
          arkSizes[i] += (uint)file.Size;
        }
        arkNames[i] = $"{name}_{i++}.ark";
      }
      Array.Sort(entries, (x, y) => x.Path.CompareTo(y.Path));
      hashTable = new int[entries.Length];
      for (i = 0; i < hashTable.Length; i++) hashTable[i] = -1;
      for(i = 0; i < entries.Length; i++)
      {
        long bin = ((uint)Hash.Compute(Encoding.UTF8.GetBytes(entries[i].Path))) % hashTable.Length;
        hashTable[bin] = i;
      }
    }

    public void Save(string outPath)
    {
      var hdrPath = Path.Combine(outPath, name + ".hdr");
      using (var s = File.Create(hdrPath))
      {
        WriteHdr(s);
      }

      for(int i = 0; i < arkNames.Length; i++)
      {
        using (var s = File.Create(Path.Combine(outPath, arkNames[i])))
        {
          WriteArk(i, s);
        }
      }
    }

    public void WriteHdr(Stream output)
    {
      var cryptStream = new EncryptedWriteStream(output, new Random().Next(), 0xFF);
      var w = new BinWriter(cryptStream);
      w.Write(10); // version
      w.Write(1); // unknown
      var guid = Guid.NewGuid().ToByteArray();
      cryptStream.Write(guid, 0, guid.Length);
      w.Write(arkSizes.Length);
      w.Write(arkSizes, w.Write);
      w.Write(arkNames, w.Write);
      //TODO: What are these strings
      w.Write(1);
      w.Write(0);
      w.Write(entries, x => x.Save(cryptStream));
      w.Write(hashTable, w.Write);
    }

    public void WriteArk(int arkIndex, Stream output)
    {
      if (arkIndex < 0 || arkIndex > layout.Count) throw new ArgumentOutOfRangeException(nameof(arkIndex));
      foreach(var (_,file) in layout[arkIndex])
      {
        using (var s = file.GetStream())
        {
          s.CopyTo(output);
        }
      }
    }
  }
}
