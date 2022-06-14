using System.Collections.Generic;

namespace ConwayWebApi.Database
{
    public class Board 
    {
        public int ID { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public string Description { get; set; }

        public ICollection<BoardCell> BoardCells { get; set; }
    }
}