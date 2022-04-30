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
            { 0, 34 },
            { 1, 32 },
            { 2, 33 },
            { 3, 31 },
            { 4, 35 },
            { 5, 36 },
            { 6, 37 },
            { 7, 91 },
            { 8, 92 },
        };

        private const string cHome = "\u001b[0;0H";
        private const string cDefaultColour = "\u001b[0m";

        private static string ColourForNeighbours(int n)
          => $"\u001b[{sColours[n].ToString()}m";

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
            
            Console.Clear();

            while (true)
            {
                builder.Clear().Append(cHome);

                for (int y = 0; y < board.Height; ++y)
                {
                    for (int x = 0; x < board.Width; ++x)
                    {
                        if (board.Cell(x,y))
                        {
                            builder.Append(ColourForNeighbours(board.Neighbours(x, y)));
                            builder.Append('O');
                        }
                        else
                        {
                            builder.Append(' ');
                        }
                    }

                    builder.Append(Environment.NewLine);
                }

                builder.Append(cDefaultColour);

                Console.WriteLine(builder.ToString());

                Thread.Sleep(delay);
                board = game.Turn();
            }
        }
    }
}