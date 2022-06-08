using System;
using CommandLine;
using System.Drawing;

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
        public int? WindowWidth {get; set;} = null;

        [Option("windowHeight", Required = false, HelpText = "The height of the display")]
        public int? WindowHeight {get; set;} = null;

        [Option("windowX", Required = false, HelpText = "starting x position of the display")]
        public int? WindowX {get; set;} = null;

        [Option("windowY", Required = false, HelpText = "starting y position of the display")]
        public int? WindowY {get; set;} = null;

        public Rectangle Window => new Rectangle(
            WindowX ?? 0,
            WindowY ?? 0,
            WindowWidth ?? Width ?? Console.WindowWidth - 1,
            WindowHeight ?? Height ?? Console.WindowHeight - 2);
    }
}