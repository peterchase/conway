namespace ConwayWebApi.Database
{
    public class BoardCell
    {
        public long ID {get; set; }
        public long BoardID { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
    }
}