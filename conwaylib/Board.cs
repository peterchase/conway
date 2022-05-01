using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ConwayLib
{

    public sealed class Board : IBoard, IEnumerable<bool>
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

        IEnumerator<bool> IEnumerable<bool>.GetEnumerator() => mCells.SelectMany(row => row).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable<bool>)this).GetEnumerator();
    }
}

