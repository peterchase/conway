using System;
using ConwayLib;
using CommandLine;
using System.Collections.Generic;
using System.Linq;

namespace ConwayMultiSim
{
    partial class Program
    {
        static void Main(string[] args)
        {
            CommandLineOptions options = Parser.Default.ParseArguments<CommandLineOptions>(args).Value;
            if (options == null)
                return;
            
            List<Simulation> simulations = GenerateGames(options).ToList();
            while(simulations.Any(x => !x.Finished))
            {                
                foreach (var sim in simulations)
                {
                    sim.Turn();
                }
            }
        }

        static IEnumerable<Simulation> GenerateGames(CommandLineOptions options)
        {
            for (int i = 0; i < options.Number; i++)
            {
                yield return new Simulation(options.Width, options.Height, options.Density, StandardEvolution.Instance);
            }         
        }
    }
}
