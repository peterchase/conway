using System;
using System.Collections.Generic;
using System.Linq;
using ConwayWebApi.Models;

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
    }
}