namespace ConwayLib
{
    public sealed class Game
    {
        private Board curBoard, nextBoard;

        public Game(Board initialBoard)
        {
            curBoard = new Board(initialBoard.Width, initialBoard.Height);
            nextBoard = new Board(initialBoard.Width, initialBoard.Height);
            for (int x = 0; x < initialBoard.Width; ++x)
            {
                for (int y = 0; y < initialBoard.Height; ++y)
                {
                    curBoard.Cell(x, y) = initialBoard.Cell(x, y);
                }
            }
        }

        public Board Turn()
        {
            for (int x = 0; x < curBoard.Width; ++x)
            {
                for (int y = 0; y < curBoard.Height; ++y)
                {
                    switch (curBoard.Neighbours(x, y))
                    {
                        case 2:
                            nextBoard.Cell(x, y) = curBoard.Cell(x, y);
                            break;
                        case 3:
                            nextBoard.Cell(x, y) = true;
                            break;
                        default:
                            nextBoard.Cell(x, y) = false;
                            break;
                    }
                }
            }

            (nextBoard, curBoard) = (curBoard, nextBoard);
            return curBoard;
        }
    }
}