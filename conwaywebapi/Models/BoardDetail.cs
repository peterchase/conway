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
    }
}