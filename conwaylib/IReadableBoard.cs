using System;

namespace ConwayLib;

/// <summary>
/// Represents the instantaneous state of a game of Conway's Game of Life on a rectangular grid.
/// </summary>
public interface IReadableBoard : IDisposable
{
  int Width { get; }

  int Height { get; }

  bool Cell(int x, int y);
  int? CellAge(int x, int y);
  byte[] GetUniqueHash();
}