using System;
using System.Text;
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

      //Handle show cursor on exit
      Console.CancelKeyPress += HandleCancel;
      
      try
      { 
        var initialBoard = new Board(width, height).Randomise(new Random(), 0.8);
        var game = new Game(initialBoard, StandardEvolution.Instance);
        var builder = new StringBuilder();

        Console.Clear();

        Console.CursorVisible = false;
             
        for (IReadableBoard board = initialBoard; ; board = game.Turn())
        {
          await Console.Out.WriteLineAsync(board.ToConsoleString(builder));
          await Task.Delay(delay);
        }
      }
      finally
      {        
        Console.CursorVisible = true;
        Console.CancelKeyPress -= HandleCancel;
      }
    }

    public static void HandleCancel(object sender, ConsoleCancelEventArgs args)
    {
      Console.CursorVisible = true;
    }
  }
}