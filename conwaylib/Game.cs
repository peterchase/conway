using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;

namespace ConwayLib;

/// <summary>
/// Represents one run of Conway's Game of Life.
/// </summary>
public sealed class Game
{
  private HashSet<byte[]> mHistory;

  private readonly IEvolution mEvolution;
  private IMutableBoard mCurBoard, mNextBoard;

  /// <summary>
  /// Constructs a game with starting state <paramref name="initialBoard"/> and using evolution
  /// rules <paramref name="evolution"/>.
  /// </summary>
  public Game(IReadableBoard initialBoard, IEvolution evolution)
  {
    mHistory = new HashSet<byte[]>(new HashComparer());

    mCurBoard = initialBoard.MutableCopy();
    mNextBoard = new Board(initialBoard.Width, initialBoard.Height);
      
    var hash = mCurBoard.GetUniqueHash();
    mHistory.Add(hash);

    mEvolution = evolution;
  }

  public int Generation => mHistory.Count;
    
  /// <summary>
  /// Performs one turn of the game, returning the new state of the board. The returned board
  /// may be modified by subsequent turns.
  /// </summary>
  public IReadableBoard Turn(out bool previousExists)
  {
    Parallel.For(0, mCurBoard.Width, x =>
    {
      for (int y = 0; y < mCurBoard.Height; ++y)
      {
        mNextBoard.SetCell(x, y, mEvolution.GetNextState(mCurBoard.Cell(x, y), mCurBoard.Neighbours(x, y)));
      }
    });

    (mNextBoard, mCurBoard) = (mCurBoard, mNextBoard);
    var hash = mCurBoard.GetUniqueHash();
    previousExists = mHistory.Contains(hash);
    mHistory.Add(hash);
    return mCurBoard;
  }
}