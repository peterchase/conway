namespace ConwayLib;

/// <summary>
/// Represents the rules for turn-by-turn evolution of the state of a single cell in a
///  game of Conway's Game of Life.
/// </summary>
public interface IEvolution
{
  /// <summary>
  /// For a cell with current alive/dead state represented by <paramref name="curState"/> and
  /// <paramref name="neighbours"/> alive neighbours, this method returns the next alive/dead state.
  /// </summary>
  bool GetNextState(bool curState, int neighbours);
}