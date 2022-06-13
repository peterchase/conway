using System;

namespace ConwayLib
{
    public class CellCoord : IEquatable<CellCoord>
    {
        public int X { get; set; }
        public int Y { get; set;  }
        public CellCoord(int x, int y)
        {
            X = x;
            Y = y;
        }

        public bool Equals(CellCoord other)
        {
            return X==other.X && Y == other.Y;
        }

        public static bool operator ==(CellCoord a, CellCoord b) => a.Equals(b);
        public static bool operator !=(CellCoord a, CellCoord b) => !a.Equals(b);

        public override string ToString()
        {
            return $"({X}, {Y})";
        }

        public override bool Equals(object obj)  { return base.Equals(obj); }

        public override int GetHashCode() { return base.GetHashCode(); }
    }
}