namespace ConwayLib
{
    public interface IReadableBoard
    {
        int Width { get; }

        int Height { get; }

        bool Cell(int x, int y);
    }
}

