using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ConwayLib;

namespace ConwayConsole
{

    public static class Program
    {

        public static async Task Main(params string[] args)
        {
            int width = args.Length > 0 ? int.Parse(args[0]) : 40;
            int height = args.Length > 1 ? int.Parse(args[1]) : 20;
            int delay = args.Length > 2 ? int.Parse(args[2]) : 500;

            IBoard board = new Board(width, height);
            board.Randomise(new Random(), 0.6);

            var game = new Game(board, StandardEvolution.Instance);
            var builder = new StringBuilder();
            
            Console.Clear();

            while (true)
            {
                await Console.Out.WriteLineAsync(board.ToConsoleString(builder));
                await Task.Delay(delay);
                board = game.Turn();
            }
        }
    }
}