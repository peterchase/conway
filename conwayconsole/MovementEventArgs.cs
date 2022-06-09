using System;

namespace ConwayConsole
{
    internal sealed class MovementEventArgs : EventArgs
    {
        public MovementEventArgs(int vertical, int horizontal)
        {
            Vertical = vertical;
            Horizontal = horizontal;
        }

        public int Vertical { get; }
        public int Horizontal { get; }
    }
}