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

        public int Width { get; }

        public int Height { get; }

        public string Description { get; }

        public int? Id { get; }
    }
}
