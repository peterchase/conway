using System;
using CommandLine;

namespace ConwayConsole
{
    public class CommandLineOptions
    {
        [Option('w', "width", Required = false, HelpText = "The width of the simulation")]
        public int Width {get; set;} = 20;

        [Option('h', "height", Required = false, HelpText = "The height of the simulation")]
        public int Height {get; set;} = 10;

        [Option('d', "delay", Required = false, HelpText = "The pause time between each generation")]
        public int Delay {get; set;} = 1000;
    }
}