namespace ConwayLib
{
    public sealed class Game
    {
        private readonly IEvolution mEvolution;
        private IMutableBoard curBoard, nextBoard;

        public Game(IMutableBoard initialBoard, IEvolution evolution)
        {
            curBoard = initialBoard;
            nextBoard = new Board(initialBoard.Width, initialBoard.Height);

            mEvolution = evolution;
        }

        public IReadableBoard Turn()
        {
            for (int x = 0; x < curBoard.Width; ++x)
            {
                for (int y = 0; y < curBoard.Height; ++y)
                {
                    nextBoard.Cell(x, y) = mEvolution.GetNextState(curBoard.Cell(x, y), curBoard.Neighbours(x, y));
                }
            }

            (nextBoard, curBoard) = (curBoard, nextBoard);
            return curBoard;
        }
    }
}