using System;
using System.Collections.Generic;
using System.Text;
using ConwayLib;

namespace ConwayConsole
{
    /// <summary>
    /// Assists with displaying a board of Conway's Game of Life on a Windows Console that supports
    /// escape-codes to move the cursor and change the text colour.
    /// </summary>
    internal static class BoardConsoleExtensions
    {
        private static readonly Dictionary<int, int> sColours = new Dictionary<int, int>
        {
            { 0, 34 },
            { 1, 32 },
            { 2, 33 },
            { 3, 31 },
            { 4, 35 },
            { 5, 36 },
            { 6, 37 },
            { 7, 91 },
            { 8, 92 },
        };

        private const string cHome = "\u001b[0;0H";
        private const string cDefaultColour = "\u001b[0m";

        private static string ColourForNeighbours(int n)
          => $"\u001b[{sColours[n].ToString()}m";

        public static string ToConsoleString(this IReadableBoard board, StringBuilder builder = null)
        {
            if (builder is null)
            {
                builder = new StringBuilder();
            }
            else
            {
                builder.Clear();
            }

            builder.Append(cHome);

            for (int y = 0; y < board.Height; ++y)
            {
                for (int x = 0; x < board.Width; ++x)
                {
                    if (board.Cell(x, y))
                    {
                        builder.Append(ColourForNeighbours(board.Neighbours(x, y)));
                        builder.Append('O');
                    }
                    else
                    {
                        builder.Append(' ');
                    }
                }

                builder.Append(Environment.NewLine);
            }

            builder.Append(cDefaultColour);

            return builder.ToString();
        }
    }
}