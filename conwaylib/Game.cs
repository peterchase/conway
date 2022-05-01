namespace ConwayLib
{
    public sealed class Game
    {
        private readonly IEvolution mEvolution;
        private IBoard curBoard, nextBoard;

        public Game(IBoard initialBoard, IEvolution evolution)
        {
            curBoard = initialBoard;
            nextBoard = new Board(initialBoard.Width, initialBoard.Height);
            for (int x = 0; x < initialBoard.Width; ++x)
            {
                for (int y = 0; y < initialBoard.Height; ++y)
                {
                    curBoard.Cell(x, y) = initialBoard.Cell(x, y);
                }
            }

            mEvolution = evolution;
        }

        public IBoard Turn()
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