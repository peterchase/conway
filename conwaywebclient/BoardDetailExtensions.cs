using System.Collections.Generic;
using System.Linq;
using ConwayLib;
using ConwayWebModel;

namespace ConwayWebClient;

public static class BoardDetailExtensions
{
  public static GameState ToGameState(this BoardDetail details)
  {
    return new GameState
    {
      Width = details.Info.Width,
      Height = details.Info.Height,
      SparseData = details.LiveCells.Select(x => new ConwayLib.CellCoord(x.X, x.Y)).ToArray(),
      Format = DensityOption.Sparse
    };
  }

  public static BoardDetail ToBoardDetail(this IReadableBoard board, string description = "")
  {
    return new BoardDetail
    {
      Info = new BoardInfo(board.Width, board.Height, description),
      LiveCells = board.GetSparseWebDataFromBoard().ToArray()
    };
  }

  public static IEnumerable<ConwayWebModel.CellCoord> GetSparseWebDataFromBoard(this IReadableBoard board)
  {
    for (int y = 0; y < board.Height; y++)
    {
      for (int x = 0; x < board.Width; x++)
      {
        if (board.Cell(x,y))
          yield return new ConwayWebModel.CellCoord(x,y);
      }
    }
  }
}