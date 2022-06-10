using System;
using System.Drawing;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ConwayLib;
using CommandLine;
using System.IO;

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
      MoveKeyMonitor.Start();
      Func<IReadableBoard, int, int, int> getValueForColour;
      switch (options.ColourBy)
      {
        case ColourByType.Age:
          getValueForColour = (b, x, y) => b.CellAge(x, y).Value;
          break;
        case ColourByType.Neighbours:
        default:
          getValueForColour = (b, x, y) => b.Neighbours(x, y);
          break;
      }

      using (var cts = new CancellationTokenSource())
      {
        Console.CancelKeyPress += HandleCancel;
        try
        { 
          if (!options.TryGetWindow(out Rectangle window))
          {
            await Console.Error.WriteLineAsync("Bad window specification");
            return;
          }

          Board initialBoard;
          if (options.FilePath != null)
          {
            //Create board from a file
            try
            {
              var gameState = await GameStateSerializer.DeserializeJson(options.FilePath);
              initialBoard = new Board(gameState);
              var fileWindow = new Rectangle(0,0,gameState.Width, gameState.Height);
              window.Intersect(fileWindow);
            }
            catch (IOException)
            {
              Console.WriteLine("Could not read from file");
              return;
            }            
          }
          else
          {
            // create board based on settings
            var random = options.Seed.HasValue ? new Random(options.Seed.Value) : new Random();
            double density = 1- Math.Clamp(options.Density,0,1);
            initialBoard=new Board(options.BoardWidth, options.BoardHeight);
            initialBoard.Randomise(random, density);
          }

          MoveKeyMonitor.Movement += (_, args) => 
          {
             
              window.X = Math.Min(Math.Max(0, args.Horizontal + window.X), initialBoard.Width - window.Width);
              window.Y = Math.Min(Math.Max(0, args.Vertical + window.Y), initialBoard.Height - window.Height );

          };

          var game = new Game(initialBoard, StandardEvolution.Instance);
          var builder = new StringBuilder();

          Console.Clear();

          Console.CursorVisible = false;
          bool stop = false;
              
          DateTime lastLoopTime = DateTime.UtcNow;
          for (IReadableBoard board = initialBoard; !(stop || cts.IsCancellationRequested); board = game.Turn(out stop))
          {
            await Console.Out.WriteLineAsync(board.ToConsoleString(window, builder, getValueForColour));
            DateTime now = DateTime.UtcNow;
            TimeSpan elapsed = now.Subtract(lastLoopTime);
            TimeSpan delay = TimeSpan.FromMilliseconds(options.Delay).Subtract(elapsed);
            
            Console.WriteLine($"{BoardConsoleExtensions.cHome}({board.Width}x{board.Height}) ({window.Width}x{window.Height}) ({window.X}, {window.Y})");
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
          Console.ResetColor();
          Console.CancelKeyPress -= HandleCancel;
        }

        void HandleCancel(object sender, ConsoleCancelEventArgs args) => cts.Cancel();
      }
    }
  }
}