using System;
using CommandLine;
using System.Drawing;

using static System.Math;
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

        [Option('s', "seed", Required = false, HelpText = "Enables you to select the pattern within the random generation")]
        public int? Seed {get; set;} = null;

        [Option("windowWidth", Required = false, HelpText = "The width of the display")]
        public int? WindowWidth {get; set;} = null;

        [Option("windowHeight", Required = false, HelpText = "The height of the display")]
        public int? WindowHeight {get; set;} = null;

        [Option("windowX", Required = false, HelpText = "Starting x position of the display")]
        public int? WindowX {get; set;} = null;

        [Option("windowY", Required = false, HelpText = "Starting y position of the display")]
        public int? WindowY {get; set;} = null;

        public int BoardWidth => Width ?? Console.WindowWidth - 1;

        public int BoardHeight => Height ?? Console.WindowHeight - 2;

        public bool TryGetWindow(out Rectangle window)
        {
            int x = WindowX ?? 0;
            int y = WindowY ?? 0;
            int width = WindowWidth ?? Console.WindowWidth - 1;
            int height = WindowHeight ?? Console.WindowHeight - 2;
            
            window = new Rectangle(x, y, Min(width,BoardWidth),Min(height,BoardHeight));
            var board  = new Rectangle(0 , 0, BoardWidth, BoardHeight);
            
            if (!board.Contains(window))
            {
                window = default;
                return false;
            }

            return true;
        }

        [Option('p', "density", Required = false, HelpText = "How many cells spawn in on initialisation. Ranges from 0 (no cells) to 1 (full cells)")]
        public double Density {get; set;} = 0.2;

        [Option("colourBy", Required = false, HelpText = "How to colour the cells")]
        public ColourByType ColourBy {get; set;} = ColourByType.Age;        

        [Option('f',"file", Required = false, HelpText = "Load the game's initial state from a file")]
        public string FilePath {get; set;} = null;

        [Option('i',"hideDisplay", Required = false, HelpText = "Option not to render the simulation. Does not delay between frames either.")]
        public bool HideDisplay {get; set;} = false;

        [Option('g',"maxGenerations", Required = false, HelpText = "Maximimum number of generations performed before the simulation ends.")]
        public int MaxGenerations {get; set;} = int.MaxValue;
    }

    public enum ColourByType { Age, Neighbours }
}