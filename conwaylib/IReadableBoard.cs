namespace ConwayLib
{
  /// <summary>
  /// Represents the instantaneous state of a game of Conway's Game of Life on a rectangular grid.
  /// </summary>
  public interface IReadableBoard
  {
    int Width { get; }

    int Height { get; }

    bool Cell(int x, int y);
  }
}