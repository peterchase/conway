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
          int width = options.Width ?? Console.WindowWidth - 1;
          int height = options.Height ?? Console.WindowHeight - 2;
          var random = options.Seed.HasValue ? new Random(options.Seed.Value) : new Random();
          double density = 1- Math.Clamp(options.Density,0,1);

          var initialBoard = new Board(width, height).Randomise(random, density);

          var game = new Game(initialBoard, StandardEvolution.Instance);
          var builder = new StringBuilder();

          Console.Clear();

          Console.CursorVisible = false;
          bool stop = false;
              
          DateTime lastLoopTime = DateTime.UtcNow;
          for (IReadableBoard board = initialBoard; !(stop || cts.IsCancellationRequested); board = game.Turn(out stop))
          {
            await Console.Out.WriteLineAsync(board.ToConsoleString(builder));
            DateTime now = DateTime.UtcNow;
            TimeSpan elapsed = now.Subtract(lastLoopTime);
            TimeSpan delay = TimeSpan.FromMilliseconds(options.Delay).Subtract(elapsed);
            if (delay > TimeSpan.Zero)
            {
              await Task.Delay(delay, cts.Token);
            }

            lastLoopTime = DateTime.UtcNow;
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