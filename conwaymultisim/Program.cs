using System;
using ConwayLib;
using CommandLine;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Threading.Tasks;

namespace ConwayMultiSim;

public class Program
{
    public static async Task Main(string[] args)
    {
        CommandLineOptions options = Parser.Default.ParseArguments<CommandLineOptions>(args).Value;
        if (options == null)
            return;
        
        Stopwatch sw = new Stopwatch();

        IEnumerable<Simulation> simulations = GenerateGames(options);
        sw.Start();
        foreach (var sim in simulations)
        {
            bool finished = false;
            while (!finished)
                finished = sim.Turn();
        }
        var time = sw.Elapsed;

        await Console.Out.WriteLineAsync($"{simulations.Count()} simulations completed in {time.Duration}.");
        await Console.Out.WriteLineAsync($"Seeds with longest time: {simulations.Count()} simulations completed in {time.Duration}.");
    }

    static IEnumerable<Simulation> GenerateGames(CommandLineOptions options)
    {
        for (int i = 0; i < options.Number; i++)
        {
            yield return new Simulation(options.Width, options.Height, options.Density, StandardEvolution.Instance);
        }         
    }
}
