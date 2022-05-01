namespace ConwayLib
{
    public interface IBoard
    {
        int Width { get; }
        
        int Height { get; }

        ref bool Cell(int x, int y);
    }
}

