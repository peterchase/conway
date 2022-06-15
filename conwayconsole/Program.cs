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
        public static async Task Main(params string[] args)
        {
            CommandLineOptions options = Parser.Default.ParseArguments<CommandLineOptions>(args).Value;
            ConwayClient.SetupClient("http://localhost:5000/");

            if (options == null)
                return;
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
                    if (!options.TryGetWindow(out Rectangle window))
                    {
                        await Console.Error.WriteLineAsync("Bad window specification");
                        return;
                    }

                    // Load options
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
                            return;
                        }
                    }
                    else if (options.LoadID.HasValue)
                    {
                        initialBoard = new Board(await ConwayClient.GetGameStateAsync(options.LoadID.Value));
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

                    KeyMonitor.Movement += (_, args) =>
                    {

                        window.X = Math.Min(Math.Max(0, args.Horizontal + window.X), initialBoard.Width - window.Width);
                        window.Y = Math.Min(Math.Max(0, args.Vertical + window.Y), initialBoard.Height - window.Height);

                    };

                    bool save = false;
                    KeyMonitor.Save += (_, args) => save = true;

                    var game = new Game(initialBoard, StandardEvolution.Instance);
                    var builder = new StringBuilder();

                    if (!options.HideDisplay)
                    {
                        Console.Clear();
                        Console.CursorVisible = false;
                    }

                    bool stop = false;
                    DateTime lastLoopTime = DateTime.UtcNow;
                    for (IReadableBoard board = initialBoard;
                      !(stop || cts.IsCancellationRequested) && (options.MaxGenerations >= game.Generation);
                      board = game.Turn(out stop))
                    {
                        if (!options.HideDisplay)
                        {
                            await Console.Out.WriteLineAsync(board.ToConsoleString(window, builder, getValueForColour));
                            DateTime now = DateTime.UtcNow;
                            TimeSpan elapsed = now.Subtract(lastLoopTime);
                            TimeSpan delay = TimeSpan.FromMilliseconds(options.Delay).Subtract(elapsed);

                            await Console.Out.WriteLineAsync($"{BoardConsoleExtensions.cHome}({board.Width}x{board.Height}) ({window.Width}x{window.Height}) ({window.X}, {window.Y})");
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