using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ConwayLib
{
  /// <summary>
  /// A simple in-memory implementation of <see cref="IMutableBoard"/>. It supports C# collection
  /// initialiser syntax by virtue of implementing <see cref="IEnumerable{T}"/> and having the
  /// <see cref="Add"/> method.
  /// </summary>
  public sealed class Board : IMutableBoard, IEnumerable<bool>
  {
    private readonly int?[][] mCells;

    public Board(int width, int height)
    {
      mCells = Enumerable.Range(0, height)
        .Select(_ => new int?[width])
        .ToArray();
    }

    public byte[] GetUniqueHash()
    {
      using(var hasher = new BoolArrayHasher())
      {
        return hasher.GetUniqueHash(mCells.SelectMany(row => row).Select(i => i.HasValue));
      }
    } 

    public int Width => mCells.FirstOrDefault()?.Length ?? 0;

    public int Height => mCells.Length;

    public bool Cell(int x, int y) => mCells[y][x].HasValue;
    public int? CellAge(int x, int y) => mCells[y][x];
    public void SetCell(int x, int y, bool value)
    {
      if (value)
      {
        if (Cell(x, y))
        {
          mCells[y][x] += 1;
        }
        else
        {
          mCells[y][x] = 0;
        }
      }
      else
      {
        mCells[y][x] = null;
      }
    }

    #region CollectionInitialiser

    public void Add(int x, int y, bool value) => SetCell(x, y, value);

    public IEnumerator<bool> GetEnumerator() => mCells.SelectMany(row => row).Select(i => i.HasValue).GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    #endregion
  }
}