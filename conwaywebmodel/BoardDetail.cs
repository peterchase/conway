using System;
using System.Linq;

namespace ConwayWebModel
{
    public sealed class BoardDetail
    {
        public BoardInfo Info { get; set; }

        public CellCoord[] LiveCells { get; set;}
        public BoardDetail() {}

        public BoardDetail(BoardInfo info, CellCoord[] liveCells)
        {
            Info = info;
            LiveCells = liveCells;
        }
    }
}