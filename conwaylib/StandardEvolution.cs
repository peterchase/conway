namespace ConwayLib;

/// <summary>
/// Represents the standard evolution rules of Conway's Game of Life.
/// </summary>
public sealed class StandardEvolution : IEvolution
{
  public static IEvolution Instance { get; } = new StandardEvolution();

  private StandardEvolution() { }

  public bool GetNextState(bool curState, int neighbours)
  {
    switch (neighbours)
    {
      case 2:
        return curState;
      case 3:
        return true;
      default:
        return false;
    }
  }
}