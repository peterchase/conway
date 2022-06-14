namespace ConwayWebApi.Database
{
    public class Board 
    {
        public long ID { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public string Description { get; set; }
    }
}