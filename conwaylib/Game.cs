namespace ConwayLib;

/// <summary>
/// Represents one run of Conway's Game of Life.
/// </summary>
public sealed class Game
{
  private readonly IEvolution mEvolution;
  private IMutableBoard mCurBoard, mNextBoard;

  /// <summary>
  /// Constructs a game with starting state <paramref name="initialBoard"/> and using evolution
  /// rules <paramref name="evolution"/>. The board provided in <paramref name="initialBoard"/>
  /// may be modified as the game proceeds.
  /// </summary>
  public Game(IMutableBoard initialBoard, IEvolution evolution)
  {
    mCurBoard = initialBoard;
    mNextBoard = new Board(initialBoard.Width, initialBoard.Height);

    mEvolution = evolution;
  }

  /// <summary>
  /// Performs one turn of the game, returning the new state of the board. The returned board
  /// may be modified by subsequent turns.
  /// </summary>
  public IReadableBoard Turn()
  {
    for (int x = 0; x < mCurBoard.Width; ++x)
    {
      for (int y = 0; y < mCurBoard.Height; ++y)
      {
        mNextBoard.Cell(x, y) = mEvolution.GetNextState(mCurBoard.Cell(x, y), mCurBoard.Neighbours(x, y));
      }
    }

    (mNextBoard, mCurBoard) = (mCurBoard, mNextBoard);
    return mCurBoard;
  }
}