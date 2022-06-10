using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ConwayLib
{
  public interface IAgeArrayBoard
  {
    int?[][] GetAgeBoard();
  }

  /// <summary>
  /// A simple in-memory implementation of <see cref="IMutableBoard"/>. It supports C# collection
  /// initialiser syntax by virtue of implementing <see cref="IEnumerable{T}"/> and having the
  /// <see cref="Add"/> method.
  /// </summary>
  public sealed class Board : IMutableBoard, IEnumerable<bool>, IAgeArrayBoard
  {
    private readonly int?[][] mCells;
    private static int?[][] NewArray(int width, int height)
    {
      return Enumerable.Range(0, height)
        .Select(_ => new int?[width])
        .ToArray();
    }
    public Board(int width, int height)
    {
      mCells = NewArray(width, height);
    }
    public Board(GameState state)
    {
      mCells = NewArray(state.Width, state.Height);
        switch (state.Format)
        {
          case (DensityOption.Sparse):
            Array.ForEach(state.SparseData, (p)=> SetCell(p.X, p.Y, true));
            break;
          case (DensityOption.Dense):
            for (int x = 0; x < Width; x++)
            {
              for (int y = 0; y < Height; y++)
              {
                SetCell(x, y, state.DenseData[y][x]);
              }
            }
            break;
      }
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

    int?[][] IAgeArrayBoard.GetAgeBoard()
    {
      return mCells;
    }

    #region CollectionInitialiser

    public void Add(int x, int y, bool value) => SetCell(x, y, value);

    public IEnumerator<bool> GetEnumerator() => mCells.SelectMany(row => row).Select(i => i.HasValue).GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    #endregion
    }
}