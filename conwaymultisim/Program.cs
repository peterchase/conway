using System;
using ConwayLib;
using CommandLine;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Timers;
using System.Threading;
using System.IO;

namespace ConwayMultiSim;

public class Program
{
    private static int mSimsCompleted;
    private static int mSimsExpected;
    private static int mStartSeed;
    private static int mEndSeed;
    private static EventHandler SimulationComplete;
    public static async Task Main(string[] args)
    {
        CommandLineOptions options = Parser.Default.ParseArguments<CommandLineOptions>(args).Value;
        if (options == null)
            return;

        Stopwatch sw = new Stopwatch();

        sw.Start();

        mSimsCompleted = 0;
        mSimsExpected = options.Number;
        mStartSeed = options.StartSeed;
        mEndSeed = options.StartSeed + options.Number;

        await Console.Out.WriteLineAsync($"Testing seeds {mStartSeed}(inc) to {mEndSeed}(excl) with density {options.Density}.");

        List<Simulation> simulations = GenerateGames(options, mStartSeed, mEndSeed).ToList();

        try
        {
            SimulationComplete += UIUpdate;

            Parallel.ForEach(simulations, sim =>
            {
                bool finished = false;
                while (!finished)
                {
                    finished = sim.Turn();
                }
                Interlocked.Increment(ref mSimsCompleted);
                if (!options.HideUI)
                {
                    SimulationComplete?.Invoke(null, EventArgs.Empty);
                }
                sim.Dispose();
            });
            var time = sw.Elapsed;

            if (!options.HideUI)
            {
                await Console.Out.WriteLineAsync($"\r{simulations.Count} simulations completed in {Math.Round(time.TotalMilliseconds)}ms.");

                await Console.Out.WriteLineAsync($"\nTop {Math.Min(5, mSimsExpected)}:");
                var longest5 = simulations.OrderByDescending(x => x.Generation).Take(5);
                await Console.Out.WriteLineAsync($"{"Seed",-8}  {"Gens",-8}");
                await Console.Out.WriteLineAsync($"--------  --------");

                foreach (var sim in longest5)
                {
                    await Console.Out.WriteLineAsync($"{sim.Seed,-8}  {sim.Generation,8}");
                }
                if (await YesNo($"Save initial state of simulation with seed {longest5.First().Seed} to file [Y/N]: "))
                {
                    await SaveToFile(longest5.First().InitialBoard);
                }
            }
        }
        finally
        {
            SimulationComplete -= UIUpdate;
        }
    }

    private static async Task<bool> YesNo(string prompt)
    {
        await Console.Out.WriteAsync(prompt);
        string response = (await Console.In.ReadLineAsync()).ToLower();
        return response == "y" || response == "yes";
    }

    
    private static async Task SaveToFile(IReadableBoard board)
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
                    await Console.Out.WriteAsync("Cancelling.");
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

        await GameStateSerializer.SerializeJson(board.GetCurrentState(DensityOption.Sparse), path);
        await Console.Out.WriteLineAsync($"Board written to: {Path.GetFullPath(path)}");
    }
    private static void UIUpdate(Object source, EventArgs e)
    {
        Console.Out.Write($"\r{Volatile.Read(ref mSimsCompleted)}/{mSimsExpected} completed.");
    }

    static IEnumerable<Simulation> GenerateGames(CommandLineOptions options, int start, int end)
    {
        for (int i = start; i < end; i++)
        {
            yield return new Simulation(options.Width, options.Height, options.Density, StandardEvolution.Instance, i);
        }
    }
}
