using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using ConwayLib;

namespace ConwayConsole
{
    public static class Program
    {
        private static readonly Dictionary<int, int> sColours = new Dictionary<int, int>
        {
            { 0, 31 },
            { 1, 32 },
            { 2, 33 },
            { 3, 34 },
            { 4, 35 },
            { 5, 36 },
            { 6, 37 },
            { 7, 91 },
            { 8, 92 },
        };

        public static void Main(params string[] args)
        {
            int width = args.Length > 0 ? int.Parse(args[0]) : 40;
            int height = args.Length > 1 ? int.Parse(args[1]) : 20;
            int delay = args.Length > 2 ? int.Parse(args[2]) : 500;

            var random = new Random();
            var board = new Board(width, height);
            for (int x = 0; x < board.Width; ++x)
            {
                for (int y = 0; y < board.Height; ++y)
                {
                    board.Cell(x, y) = random.NextDouble() > 0.6;
                }
            }

            var game = new Game(board);
            var builder = new StringBuilder();
            
            while (true)
            {
                Console.Clear();
                for (int y = 0; y < board.Height; ++y)
                {
                    builder.Clear();
                    for (int x = 0; x < board.Width; ++x)
                    {
                        if (board.Cell(x,y))
                        {
                            // Set the colour
                            builder.Append("\u001b[");
                            builder.Append(sColours[board.Neighbours(x, y)].ToString());
                            builder.Append('m');

                            // Show the cell
                            builder.Append('O');
                        }
                        else
                        {
                            builder.Append(' ');
                        }
                    }

                    Console.WriteLine(builder.ToString());
                }

                Console.WriteLine("\u001b[0m");

                Thread.Sleep(delay);
                board = game.Turn();
            }
        }
    }
}