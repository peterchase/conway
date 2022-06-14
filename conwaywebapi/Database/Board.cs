using System;
using System.Collections.Generic;
using System.Linq;
using ConwayWebModel;

namespace ConwayWebApi.Database
{
    public class Board 
    {
        public int ID { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public string Description { get; set; }
        

        public ICollection<BoardCell> BoardCells { get; set; }
        public Board(){}
        public Board(BoardDetail board)
        {
            if (board.Info.Id.HasValue)
                throw new ArgumentException("ID should be null");

            Width = board.Info.Width;
            Height = board.Info.Height;
            Description = board.Info.Description;

            BoardCells = board.LiveCells.Select(x => new BoardCell{X = x.X, Y = x.Y}).ToArray();
        }
        public BoardInfo ToBoardInfo()
        {
            return new BoardInfo{
                Width = Width,
                Height = Height,
                Description = Description,
                Id = ID
            };
        }
        public BoardDetail ToBoardDetail()
        {
            return new BoardDetail{
                Info = ToBoardInfo(),
                LiveCells = BoardCells.Select(x => new CellCoord(x.X, x.Y)).ToArray()
            };
        }
    }
}