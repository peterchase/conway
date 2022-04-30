using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ConwayLib
{
    public sealed class Board : IEnumerable<bool>
    {
        private readonly bool[][] mCells;

        public Board(int width, int height)
        {
            mCells = Enumerable.Range(0, height)
                .Select(row => new bool[width])
                .ToArray();
        }

        public int Width => mCells.FirstOrDefault()?.Length ?? 0;

        public int Height => mCells.Length;

        public ref bool Cell(int x, int y) => ref mCells[y][x];

        // This method is mostly here to allow the collection initialiser syntax
        public void Add(int x, int y, bool value) => Cell(x, y) = value;

        public int Neighbours(int x, int y)
        {
            int neighbours = 0;
            for (int xx = Math.Max(0, x - 1);
                xx <= Math.Min(Width - 1, x + 1);
                ++xx)
                {
                    for (int yy = Math.Max(0, y - 1);
                        yy <= Math.Min(Height - 1, y + 1);
                        ++yy)
                        {
                            if ((xx == x) && (yy == y))
                            {
                                continue;
                            }

                            if (mCells[yy][xx])
                            {
                                ++neighbours;
                            }
                        }
                }

            return neighbours;
        }

        public IEnumerator<bool> GetEnumerator() => mCells.SelectMany(row => row).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}

