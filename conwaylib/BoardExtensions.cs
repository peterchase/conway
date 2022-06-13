using System;
using System.Linq;
using System.Collections.Generic;
using System.Drawing;
using static System.Math;

namespace ConwayLib
{
    public static class BoardExtensions
    {
        /// <summary>
        /// For the cell of <paramref name="board"/> at position [<paramref name="x"/>, <paramref name="y"/>], this
        /// returns the number of alive neighbours, not including the cell itself. Cells beyond the edges of
        /// <paramref name="board"/> are considered always-dead.
        /// </summary>
        public static int Neighbours(this IReadableBoard board, int x, int y)
        {
            int neighbours = 0;
            int minX = Max(0, x - 1);
            int minY = Max(0, y - 1);
            int maxX = Min(board.Width - 1, x + 1);
            int maxY = Min(board.Height - 1, y + 1);

            //Fast way
            if (board is IAgeArrayBoard ageArrayBoard)
            {
                int?[][] ageArray = ageArrayBoard.GetAgeBoard();
                if (minY < y && maxY > y && minX < x && maxX > x)
                {
                    int?[] minYArray = ageArray[minY];
                    int?[] yArray = ageArray[y];
                    int?[] maxYArray = ageArray[maxY];

                    if (minYArray[minX].HasValue)
                    {
                        ++neighbours;
                    }
                    if (yArray[minX].HasValue)
                    {
                        ++neighbours;
                    }
                    if (maxYArray[minX].HasValue)
                    {
                        ++neighbours;
                    }

                    if (minYArray[x].HasValue)
                    {
                        ++neighbours;
                    }
                    if (maxYArray[x].HasValue)
                    {
                        ++neighbours;
                    }

                    if (minYArray[maxX].HasValue)
                    {
                        ++neighbours;
                    }
                    if (yArray[maxX].HasValue)
                    {
                        ++neighbours;
                    }
                    if (maxYArray[maxX].HasValue)
                    {
                        ++neighbours;
                    }

                    return neighbours;
                }
            }
            
            //Slow way
            for (int xx = minX; xx <= maxX; ++xx)
            {
                for (int yy = minY; yy <= maxY; ++yy)
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

        /// <summary>
        /// Randomises the state of all cells in <paramref name="board"/>. The proportion of cells that
        /// are alive is given by <paramref name="liveFraction"/>, which must be between 0 and 1.
        /// </summary>
        public static IMutableBoard Randomise(this IMutableBoard board, Random random, double liveFraction)
        {
            for (int x = 0; x < board.Width; ++x)
            {
                for (int y = 0; y < board.Height; ++y)
                {
                    board.SetCell(x, y, random.NextDouble() > liveFraction);
                }
            }

            return board;
        }

        public static IMutableBoard Randomise(this IMutableBoard board, double liveFraction)
          => board.Randomise(new Random(), liveFraction);

        /// <summary>
        /// Returns a new mutable copy of <paramref name="board"/>.
        /// </summary>
        public static IMutableBoard MutableCopy(this IReadableBoard board)
        {
            var newBoard = new Board(board.Width, board.Height);
            for (int x = 0; x < board.Width; ++x)
            {
                for (int y = 0; y < board.Height; ++y)
                {
                    newBoard.SetCell(x, y, board.Cell(x, y));
                }
            }

            return newBoard;
        }

        public static GameState GetCurrentState(this IReadableBoard board, DensityOption option)
        {
            var state = new GameState
            {
                Format = option,
                Width = board.Width,
                Height = board.Height
            };
            switch (option)
            {
                case DensityOption.Dense:
                    state.DenseData = Enumerable.Range(0, board.Height)
                        .Select(y => Enumerable.Range(0, board.Width)
                            .Select(x => board.Cell(x, y)).ToArray())
                        .ToArray();
                    break;
                case DensityOption.Sparse:
                    state.SparseData = Enumerable.Range(0, board.Height)
                        .Select(y => Enumerable.Range(0, board.Width)
                            .Select(x => new Point(x, y)))
                        .SelectMany(xy => xy)
                        .Where(xy => board.Cell(xy.X, xy.Y))
                        .ToArray();
                    break;
            }
            return state;
        }
    }
}