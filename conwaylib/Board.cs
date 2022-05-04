using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ConwayLib;

/// <summary>
/// A simple in-memory implementation of <see cref="IMutableBoard"/>. It supports C# collection
/// initialiser syntax by virtue of implementing <see cref="IEnumerable{T}"/> and having the
/// <see cref="Add"/> method.
/// </summary>
public sealed class Board : IMutableBoard, IEnumerable<bool>
{
  private readonly bool[][] mCells;

  public Board(int width, int height)
  {
    mCells = Enumerable.Range(0, height)
      .Select(_ => new bool[width])
      .ToArray();
  }

  public int Width => mCells.FirstOrDefault()?.Length ?? 0;

  public int Height => mCells.Length;

  bool IReadableBoard.Cell(int x, int y) => Cell(x, y);

  public ref bool Cell(int x, int y) => ref mCells[y][x];

  #region CollectionInitialiser
  public void Add(int x, int y, bool value) => Cell(x, y) = value;

  public IEnumerator<bool> GetEnumerator() => mCells.SelectMany(row => row).GetEnumerator();

  IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
  #endregion
}