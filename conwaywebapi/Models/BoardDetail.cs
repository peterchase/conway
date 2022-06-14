using System;
using System.Linq;
using ConwayWebApi.Database;

namespace ConwayWebApi.Models
{
    public sealed class BoardDetail
    {
        public BoardInfo Info { get; }

        public CellCoord[] LiveCells { get; }

        public BoardDetail(BoardInfo info, CellCoord[] liveCells)
        {
            Info = info;
            LiveCells = liveCells;
        }
        public BoardDetail(Board board, int? id)
        {
            Info = new BoardInfo(board, id);
            LiveCells = board.BoardCells?.Select(x => new CellCoord(x.X, x.Y)).ToArray() ?? Array.Empty<CellCoord>();
        }
    }
}