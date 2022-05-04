namespace ConwayLib
{
  /// <summary>
  /// Allows changes to the instantaneous state of a game of Conway's Game of Life on a rectangular grid.
  /// </summary>
  public interface IMutableBoard : IReadableBoard
  {
    new ref bool Cell(int x, int y);
  }
}