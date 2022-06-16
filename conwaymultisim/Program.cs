using System;
using ConwayLib;
using CommandLine;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Timers;
using System.Threading;

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
                SimulationComplete?.Invoke(null, EventArgs.Empty);
            });
            var time = sw.Elapsed;

            await Console.Out.WriteLineAsync($"\r{simulations.Count} simulations completed in {Math.Round(time.TotalMilliseconds)}ms.");

            await Console.Out.WriteLineAsync($"\nTop {Math.Min(5, mSimsExpected)}:");
            var longest5 = simulations.OrderByDescending(x => x.Generation).Take(5);
            await Console.Out.WriteLineAsync($"{"Seed",-8}  {"Gens",-8}");
            await Console.Out.WriteLineAsync($"--------  --------");

            foreach (var sim in longest5)
            {
                await Console.Out.WriteLineAsync($"{sim.Seed,-8}  {sim.Generation,8}");
            }
        }
        finally
        {
            SimulationComplete -= UIUpdate;
        }
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
