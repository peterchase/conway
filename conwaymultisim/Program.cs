using System;
using ConwayLib;

namespace ConwayMultiSim
{
    partial class Program
    {
        static void Main(string[] args)
        {
            CommandLineOptions options = Parser.Default.ParseArguments<CommandLineOptions>(args).Value;
            if (options == null)
                return;
            
            if (options.LoadIDs.HasValue)
            {
                //load from db
            }
            else if (options.files.HasValue)
            {
                //load from file paths
            }
            else
            {
                //load from seeds and densities
            }
        }

        static IEnumerable<IReadableBoard> GenerateGames(CommandLineOptions options)
        {

        }
    }
}
