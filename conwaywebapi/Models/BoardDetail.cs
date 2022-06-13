namespace ConwayWebApi.Models
{
    public sealed class BoardDetail
    {
        public BoardInfo Info { get; }

        public bool[][] Cells { get; }

        public BoardDetail(BoardInfo info, bool[][] cells)
        {
            Info = info;
            Cells = cells;
        }
    }
}
