using System;
using System.Text;
using System.Threading.Tasks;
using ConwayLib;
using System.Runtime.InteropServices;

namespace ConwayConsole
{
  /// <summary>
  /// Entry point for a run of Conway's Game of Life with console-based display of the board.
  /// </summary>
  public static class Program
  {
    [DllImport("Kernel32")]
    private static extern bool SetConsoleCtrlHandler(EventHandler handler, bool add);

    private delegate bool EventHandler(CtrlType sig);
    static EventHandler _handler;

    enum CtrlType
    {
      CTRL_C_EVENT = 0,
      CTRL_BREAK_EVENT = 1,
      CTRL_CLOSE_EVENT = 2,
      CTRL_LOGOFF_EVENT = 5,
      CTRL_SHUTDOWN_EVENT = 6
    }

    private static bool Handler(CtrlType sig)
    {
      Console.CursorVisible = true;
      return false;
    }

    public static async Task Main(params string[] args)
    {
      int width = args.Length > 0 ? int.Parse(args[0]) : 40;
      int height = args.Length > 1 ? int.Parse(args[1]) : 20;
      int delay = args.Length > 2 ? int.Parse(args[2]) : 500;
      
      //Handle restoring cursor on exit
      _handler += new EventHandler(Handler);
      SetConsoleCtrlHandler(_handler, true);

      //Handle show cursor on exit
      Console.CancelKeyPress += new ConsoleCancelEventHandler(HandleCancel);
      AppDomain.CurrentDomain.ProcessExit += new EventHandler(HandleExit);

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

    public static void HandleCancel(object sender, ConsoleCancelEventArgs args)
    {
      Console.CursorVisible = true;
    }

    public static void HandleExit(object sender,  EventArgs e)
    {
      Console.CursorVisible = true;
    }
  }
}