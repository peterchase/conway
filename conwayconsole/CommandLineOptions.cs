using System;
using CommandLine;

namespace ConwayConsole
{
    public class CommandLineOptions
    {
        [Option('w', "width", Required = false, HelpText = "The width of the simulation")]
        public int? Width {get; set;} = null;

        [Option('h', "height", Required = false, HelpText = "The height of the simulation")]
        public int? Height {get; set;} = null;
        
        [Option('d', "delay", Required = false, HelpText = "The pause time between each generation in milliseconds")]
        public long Delay {get; set;} = 500;
    }
}