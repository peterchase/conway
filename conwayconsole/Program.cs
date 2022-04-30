using System;
using System.Threading;
using ConwayLib;

namespace ConwayConsole
{
    public static class Program
    {
        public static void Main(params string[] args)
        {
            var random = new Random();
            var board = new Board(100, 25);
            for (int x = 0; x < board.Width; ++x)
            {
                for (int y = 0; y < board.Height; ++y)
                {
                    board.Cell(x, y) = random.NextDouble() > 0.6;
                }
            }

            var game = new Game(board);

            while (true)
            {
                Console.Clear();
                for (int y = 0; y < board.Height; ++y)
                {
                    for (int x = 0; x < board.Width; ++x)
                    {
                        switch (board.Neighbours(x, y))
                        {
                            case 0:
                                Console.ForegroundColor = ConsoleColor.Blue;
                                break;
                            case 1:
                                Console.ForegroundColor = ConsoleColor.Green;
                                break;
                            case 2:
                                Console.ForegroundColor = ConsoleColor.Yellow;
                                break;
                            case 3:
                                Console.ForegroundColor = ConsoleColor.Magenta;
                                break;
                            case 4:
                                Console.ForegroundColor = ConsoleColor.Cyan;
                                break;
                            case 5:
                                Console.ForegroundColor = ConsoleColor.DarkMagenta;
                                break;
                            case 6:
                                Console.ForegroundColor = ConsoleColor.DarkGreen;
                                break;
                            case 7:
                                Console.ForegroundColor = ConsoleColor.DarkCyan;
                                break;
                            default:
                                Console.ForegroundColor = ConsoleColor.DarkBlue;
                                break;
                        }

                        Console.Write(board.Cell(x, y) ? "O" : " ");
                    }

                    Console.WriteLine();
                }

                Thread.Sleep(600);
                board = game.Turn();
            }
        }
    }
}