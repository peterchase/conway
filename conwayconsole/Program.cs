using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ConwayLib;
using CommandLine;

namespace ConwayConsole
{
  /// <summary>
  /// Entry point for a run of Conway's Game of Life with console-based display of the board.
  /// </summary>
  public static class Program
  {
    public static async Task Main(params string[] args)
    {
      CommandLineOptions options = Parser.Default.ParseArguments<CommandLineOptions>(args).Value;

      if (options==null)
        return;

      using (var cts = new CancellationTokenSource())
      {
        Console.CancelKeyPress += HandleCancel;
        try
        { 
          var initialBoard = new Board(options.Width, options.Height).Randomise(new Random(), 0.8);
          var game = new Game(initialBoard, StandardEvolution.Instance);
          var builder = new StringBuilder();

          Console.Clear();

          Console.CursorVisible = false;
          bool stop = false;
          for (IReadableBoard board = initialBoard; !(stop || cts.IsCancellationRequested); board = game.Turn(out stop))
          {
            await Console.Out.WriteLineAsync(board.ToConsoleString(builder));
            await Task.Delay(options.Delay, cts.Token);
          }
        }
        catch (OperationCanceledException)
        {
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