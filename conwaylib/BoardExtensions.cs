using System;
using static System.Math;

namespace ConwayLib
{
  public static class BoardExtensions
  {
    /// <summary>
    /// For the cell of <paramref name="board"/> at position [<paramref name="x"/>, <paramref name="y"/>], this
    /// returns the number of alive neighbours, not including the cell itself. Cells beyond the edges of
    /// <paramref name="board"/> are considered always-dead.
    /// </summary>
    public static int Neighbours(this IReadableBoard board, int x, int y)
    {
      int neighbours = 0;
      for (int xx = Max(0, x - 1);
           xx <= Min(board.Width - 1, x + 1);
           ++xx)
      {
        for (int yy = Max(0, y - 1);
             yy <= Min(board.Height - 1, y + 1);
             ++yy)
        {
          if ((xx == x) && (yy == y))
          {
            continue;
          }

          if (board.Cell(xx, yy))
          {
            ++neighbours;
          }
        }
      }

      return neighbours;
    }

    /// <summary>
    /// Randomises the state of all cells in <paramref name="board"/>. The proportion of cells that
    /// are dead is given by <paramref name="deadFraction"/>, which must be between 0 and 1.
    /// </summary>
    public static IMutableBoard Randomise(this IMutableBoard board, Random random, double deadFraction)
    {
      for (int x = 0; x < board.Width; ++x)
      {
        for (int y = 0; y < board.Height; ++y)
        {
          board.SetCell(x, y, random.NextDouble() > deadFraction);
        }
      }

      return board;
    }

    public static void Apply(this Board board, GameState toApply)
    {
      Array.ForEach(toApply.SparseData, (p)=>
      {
        board.SetCell(p.X, p.Y, true);
      }):
    }

    public static IMutableBoard Randomise(this IMutableBoard board, double deadFraction)
      => board.Randomise(new Random(), deadFraction);
      
    /// <summary>
    /// Returns a new mutable copy of <paramref name="board"/>.
    /// </summary>
    public static IMutableBoard MutableCopy(this IReadableBoard board)
    {
      var newBoard = new Board(board.Width, board.Height);
      for (int x = 0; x < board.Width; ++x)
      {
        for (int y = 0; y < board.Height; ++y)
        {
          newBoard.SetCell(x, y, board.Cell(x, y));
        }
      }

      return newBoard;
    }
  }
}