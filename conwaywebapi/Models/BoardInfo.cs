using ConwayWebApi.Database;

namespace ConwayWebApi.Models
{
    public sealed class BoardInfo
    {
        public BoardInfo(int width, int height, string description, int? id = null)
        {
            Width = width;
            Height = height;
            Description = description;
            Id = id;
        }
        public BoardInfo(Board board, int? id = null)
        {
            
            Width = board.Width;
            Height = board.Height;
            Description = board.Description;
            Id = id;
        }
        public BoardInfo(BoardInfo oldInfo, int? id = null)
        {
            Width = oldInfo.Width;
            Height = oldInfo.Height;
            Description = oldInfo.Description;
            Id = id;
        }
        public BoardInfo() {}

        public int Width { get; set; }

        public int Height { get; set; }

        public string Description { get; set; }

        public int? Id { get; set; }
    }
}
