using System;

namespace ConwayLib
{
    public static class BoardExtensions
    {
        public static int Neighbours(this IBoard board, int x, int y)
        {
            int neighbours = 0;
            for (int xx = Math.Max(0, x - 1);
                xx <= Math.Min(board.Width - 1, x + 1);
                ++xx)
            {
                for (int yy = Math.Max(0, y - 1);
                    yy <= Math.Min(board.Height - 1, y + 1);
                    ++yy)
                {
                    if ((xx == x) && (yy == y))
                    {
                        continue;
                    }

                    if (board.Cell(xx, yy))
                    {
                        ++neighbours;
                    }
                }
            }

            return neighbours;
        }

        public static void Randomise(this IBoard board, Random random, double threshold)
        {
            for (int x = 0; x < board.Width; ++x)
            {
                for (int y = 0; y < board.Height; ++y)
                {
                    board.Cell(x, y) = random.NextDouble() > 0.6;
                }
            }
        }
    }
}

