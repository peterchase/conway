namespace ConwayLib
{
    public interface IMutableBoard : IReadableBoard
    {
        new ref bool Cell(int x, int y);
    }
}

