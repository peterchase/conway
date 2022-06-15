using System;
using System.Drawing;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ConwayLib;
using CommandLine;
using System.IO;
using ConwayWebClient;

namespace ConwayConsole
{
    /// <summary>
    /// Entry point for a run of Conway's Game of Life with console-based display of the board.
    /// </summary>
    public static class Program
    {
        static ConwayClient mClient;
        public static async Task Main(params string[] args)
        {
            CommandLineOptions options = Parser.Default.ParseArguments<CommandLineOptions>(args).Value;
            mClient = new ConwayClient(options.WebURL);
            if (options == null)
                return;

            // Colour function for coloring cells
            Func<IReadableBoard, int, int, int> getValueForColour = options.ColourBy switch
            {
                ColourByType.Age => (b, x, y) => b.CellAge(x, y).Value,
                _ => (b, x, y) => b.Neighbours(x, y),
            };
            
            using (var cts = new CancellationTokenSource())
            {

                if (!options.HideDisplay)
                {
                    Console.CancelKeyPress += HandleCancel;
                    KeyMonitor.Start();
                }

                IReadableBoard boardToSave = null;
                try
                {

                    // Game Loading
                    if (!options.TryGetWindow(out Rectangle window))
                    {
                        await Console.Error.WriteLineAsync("Bad window specification");
                        return;
                    }
                    var loadResult = await Load(options, window);
                    Board initialBoard = loadResult.Item1;
                    window = loadResult.Item2;

                    //Keypress event handling
                    bool save = false;
                    KeyMonitor.Movement += (_, args) =>
                    {

                        window.X = Math.Min(Math.Max(0, args.Horizontal + window.X), initialBoard.Width - window.Width);
                        window.Y = Math.Min(Math.Max(0, args.Vertical + window.Y), initialBoard.Height - window.Height);

                    };
                    KeyMonitor.Save += (_, args) => save = true;

                    // create game
                    var game = new Game(initialBoard, StandardEvolution.Instance);
                    var builder = new StringBuilder();

                    // Setup console
                    if (!options.HideDisplay)
                    {
                        Console.Clear();
                        Console.CursorVisible = false;
                    }

                    // Run simulation
                    bool stop = false;
                    DateTime lastLoopTime = DateTime.UtcNow;
                    for (IReadableBoard board = initialBoard;
                      !(stop || cts.IsCancellationRequested) && (options.MaxGenerations >= game.Generation);
                      board = game.Turn(out stop))
                    {
                        if (!options.HideDisplay)
                        {
                            // Render
                            await Console.Out.WriteLineAsync(board.ToConsoleString(window, builder, getValueForColour));
                            await Console.Out.WriteLineAsync($"{BoardConsoleExtensions.cHome}({board.Width}x{board.Height}) ({window.Width}x{window.Height}) ({window.X}, {window.Y})");

                            // Make sure delay between boards is equal to the delay specified in cmd if possible
                            DateTime now = DateTime.UtcNow;
                            TimeSpan elapsed = now.Subtract(lastLoopTime);
                            TimeSpan delay = TimeSpan.FromMilliseconds(options.Delay).Subtract(elapsed);
                            if (delay > TimeSpan.Zero)
                            {
                                if (delay > TimeSpan.Zero)
                                {
                                    await Task.Delay(delay, cts.Token);
                                }

                                lastLoopTime = DateTime.UtcNow;
                            }
                        }

                        if (save)
                        {
                            boardToSave = board;
                            break;
                        }
                    }
                }
                catch (OperationCanceledException)
                {
                }
                finally
                {
                    // Reset Console
                    if (!options.HideDisplay)
                    {
                        Console.CursorVisible = true;
                        Console.ResetColor();
                        Console.CancelKeyPress -= HandleCancel;
                    }
                }

                if (boardToSave != null)
                {
                    await Save(boardToSave);
                }

                void HandleCancel(object sender, ConsoleCancelEventArgs args) => cts.Cancel();
            }
        }

        private static async Task<bool> YesNo(string prompt)
        {
            await Console.Out.WriteAsync(prompt);
            string response = (await Console.In.ReadLineAsync()).ToLower();
            return response == "y" || response == "yes";
        }


        private static async Task<(Board, Rectangle)> Load(CommandLineOptions options, Rectangle window)
        {
            Board initialBoard;
            if (options.FilePath != null)
            {
                //Create board from a file
                try
                {
                    var gameState = await GameStateSerializer.DeserializeJson(options.FilePath);
                    initialBoard = new Board(gameState);
                    var fileWindow = new Rectangle(0, 0, gameState.Width, gameState.Height);
                    window.Intersect(fileWindow);
                }
                catch (IOException)
                {
                    Console.WriteLine("Could not read from file");
                    return (null, window);
                }
            }
            else if (options.LoadID.HasValue)
            {
                // create board from database
                initialBoard = new Board((await mClient.GetBoardDetailAsync(options.LoadID.Value)).ToGameState());
                var fileWindow = new Rectangle(0, 0, initialBoard.Width, initialBoard.Height);
                window.Intersect(fileWindow);
            }
            else
            {
                // create board based on settings
                var random = options.Seed.HasValue ? new Random(options.Seed.Value) : new Random();
                double density = 1 - Math.Clamp(options.Density, 0, 1);
                initialBoard = new Board(options.BoardWidth, options.BoardHeight);
                initialBoard.Randomise(random, density);
            }
            return (initialBoard, window);
        }

        private static async Task Save(IReadableBoard boardToSave)
        {
            await Console.Out.WriteAsync("Enter file path (leave blank for default): ");
            string path = await Console.In.ReadLineAsync();

            if (string.IsNullOrWhiteSpace(path))
            {
                path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), $"{Guid.NewGuid():N}.json");
            }
            else
            {
                path = Path.ChangeExtension(path, ".json");
                string dir = Path.GetDirectoryName(path);
                if (dir != null && !Directory.Exists(dir))
                {
                    if (await YesNo("Do you want to make a new directory [Y/N]: "))
                    {
                        Directory.CreateDirectory(dir);
                    }
                    else
                    {
                        return;
                    }
                }
            }

            if (File.Exists(path))
            {
                if (!await YesNo("An existing file with that name already exists. Do you want to overwrite it [Y/N]: "))
                {
                    path = Path.Combine(Path.GetDirectoryName(path), $"{Path.GetFileNameWithoutExtension(path)}-{Guid.NewGuid():N}.json");
                }
            }

            await GameStateSerializer.SerializeJson(boardToSave.GetCurrentState(DensityOption.Sparse), path);
            await Console.Out.WriteLineAsync($"Board written to: {Path.GetFullPath(path)}");
        }
    }
}