using System;
using CommandLine;
using System.Drawing;
using static System.Math;

namespace ConwayMultiSim
{
    public class CommandLineOptions
    {
        [Option('w', "width", Required = false, HelpText = "The width of the simulation")]
        public int? Width {get; set;} = null;

        [Option('h', "height", Required = false, HelpText = "The height of the simulation")]
        public int? Height {get; set;} = null;

        [Option('s', "seeds", Seperator=", ", Required = false, HelpText = "Enables you to select the pattern within the random generation")]
        public IENumberable<int>? Seeds {get; set;} = null;

        [Option('p', "density", Seperator=", ", Required = false, HelpText = "How many cells spawn in on initialisation. Ranges from 0 (no cells) to 1 (full cells)")]
        public IENumberable<double>? Densities {get; set;} = 0.2;

        [Option('f',"files", Seperator=", ", Required = false, HelpText = "Load the game's initial state from a json file.")]
        public IENumberable<int>? FilePaths {get; set;} = null;

        [Option('l',"loads", Required = false, HelpText = "Load the game's initial state the database via webservice. Requires the board's ID.")]
        public IENumberable<int>? LoadIDs {get; set;} = null;

        [Option('g',"maxGenerations", Required = false, HelpText = "Maximimum number of generations performed before the simulation ends.")]
        public int MaxGenerations {get; set;} = int.MaxValue;
    }
}