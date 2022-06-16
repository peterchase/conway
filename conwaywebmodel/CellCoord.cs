using System;

namespace ConwayWebModel;

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
    return other != null && X==other.X && Y == other.Y;
  }

  public static bool operator ==(CellCoord a, CellCoord b) => !ReferenceEquals(a, null) && a.Equals(b);
  public static bool operator !=(CellCoord a, CellCoord b) => !ReferenceEquals(a, null) && !a.Equals(b);

  public override string ToString()
  {
    return $"({X}, {Y})";
  }

  public override int GetHashCode()
  {
    unchecked{
      return X+Y;
    }
  }

  public override bool Equals(object obj)
  {
    return obj is CellCoord coord && Equals(coord);
  }
}