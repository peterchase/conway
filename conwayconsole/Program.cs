using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ConwayLib;

namespace ConwayConsole
{
  /// <summary>
  /// Entry point for a run of Conway's Game of Life with console-based display of the board.
  /// </summary>
  public static class Program
  {
    public static async Task Main(params string[] args)
    {
      int width = args.Length > 0 ? int.Parse(args[0]) : 40;
      int height = args.Length > 1 ? int.Parse(args[1]) : 20;
      int delay = args.Length > 2 ? int.Parse(args[2]) : 500;

      using (var cts = new CancellationTokenSource())
      {
        Console.CancelKeyPress += HandleCancel;
        try
        { 
          var initialBoard = new Board(width, height).Randomise(new Random(), 0.8);
          var game = new Game(initialBoard, StandardEvolution.Instance);
          var builder = new StringBuilder();

          Console.Clear();

          Console.CursorVisible = false;
              
          for (IReadableBoard board = initialBoard; !cts.IsCancellationRequested; board = game.Turn())
          {
            await Console.Out.WriteLineAsync(board.ToConsoleString(builder));
            await Task.Delay(delay, cts.Token);
          }
        }
        finally
        {        
          Console.CursorVisible = true;
          Console.CancelKeyPress -= HandleCancel;
        }

        void HandleCancel(object sender, ConsoleCancelEventArgs args) => cts.Cancel();
      }
    }
  }
}