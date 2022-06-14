using System.ComponentModel.DataAnnotations.Schema;

namespace ConwayWebApi.Database
{
    [Table("BoardCells")]
    public class BoardCell
    {
        public int ID {get; set; }
        public int BoardID { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
    }
}