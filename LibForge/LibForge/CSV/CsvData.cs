using LibForge.Extensions;
using LibForge.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace LibForge.CSV
{
  public class CsvData
  {
    public string[] headerRow;
    public string[][] rows;

    public static CsvData LoadFile(Stream s)
    {
      CsvData c = new CsvData();
      c.Read(s);
      return c;
    }

    public override string ToString()
    {
      var sb = new StringBuilder();
      if (headerRow.Length > 0)
      {
        sb.Append(headerRow[0]);
        for (int i = 1; i < headerRow.Length; i++)
        {
          sb.Append(',').Append(headerRow[i]);
        }
        sb.AppendLine();
      }
      if (rows.Length > 0)
      {
        for(int i = 0; i < rows.Length; i++)
        {
          if (rows[i].Length == 0) continue;
          sb.Append(rows[i][0]);
          for (int j = 1; j < rows[i].Length; j++)
          {
            sb.Append(',').Append(rows[i][j]);
          }
          sb.AppendLine();
        }
      }
      return sb.ToString();
    }

    public void Read(Stream s)
    {
      var r = new BinReader(s);
      r.Check(r.Int(), 1);
      r.Check(r.Int(), 2);
      r.Check(r.Byte(), 0x2C);
      var tableSize = r.Int();
      if (tableSize + s.Position >= s.Length)
      {
        throw new InvalidDataException("String table exceeded the length of the file.");
      }
      var stringTable = s.ReadBytes(tableSize);
      var stringcache = new Dictionary<int, string>();
      string GetString(int index)
      {
        if (stringcache.ContainsKey(index)) return stringcache[index];
        int length = 0;
        while (stringTable[index + length] != 0 && ++length < stringTable.Length) ;
        return stringcache[index] = Encoding.UTF8.GetString(stringTable, index, length);
      }
      var headerColumns = r.Int();
      headerRow = new string[headerColumns];
      for(int i = 0; i < headerColumns; i++)
      {
        headerRow[i] = GetString(r.Int());
      }
      var numRows = r.Int();
      rows = new string[numRows][];
      for(int i = 0; i < numRows; i++)
      {
        var numColumns = r.Int();
        rows[i] = new string[numColumns];
        for (int j = 0; j < numColumns; j++)
        {
          rows[i][j] = GetString(r.Int());
        }
      }
    }
  }
}
