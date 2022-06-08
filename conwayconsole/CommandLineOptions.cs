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

        [Option('s', "seed", Required = false, HelpText = "enables you to select the pattern within the random generation")]
        public int? Seed {get; set;} = null;

        [Option("windowWidth", Required = false, HelpText = "The width of the display")]
        public int? windowWidth {get; set;} = null;

        [Option("windowHeight", Required = false, HelpText = "The height of the display")]
        public int? windowHeight {get; set;} = null;

        [Option("windowX", Required = false, HelpText = "starting x position of the display")]
        public int? windowX {get; set;} = null;

        [Option("windowY", Required = false, HelpText = "starting y position of the display")]
        public int? windowY {get; set;} = null;
    }
}