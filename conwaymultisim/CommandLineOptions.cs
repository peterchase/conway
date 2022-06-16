using System;
using CommandLine;
using System.Drawing;
using static System.Math;

namespace ConwayMultiSim
{
    public class CommandLineOptions
    {
        [Option('w', "width", Required = true, HelpText = "The width of the simulation")]
        public int Width {get; set;}

        [Option('h', "height", Required = true, HelpText = "The height of the simulation")]
        public int Height {get; set;}

        [Option('d', "density", Required = true, HelpText = "Starting density of the simulation")]
        public double Density {get; set;}

        [Option('n', "number", Required = true, HelpText = "The number of different simulations to perform.")]
        public int Number {get; set;}

        [Option('g',"maxGenerations", Required = false, HelpText = "Maximimum number of generations performed before the simulation ends.")]
        public int MaxGenerations {get; set;} = int.MaxValue;        

        [Option('s',"startSeed", Required = false, HelpText = "Starting seed the games generate from.")]
        public int StartSeed {get; set;} = 0;

        [Option('i',"hideUI", Required = false, HelpText = "Hide UI (Silent mode).")]
        public bool HideUI {get; set;} = false;
    }
}