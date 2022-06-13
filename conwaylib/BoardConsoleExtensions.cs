using System;
using System.Collections.Generic;
using System.Text;
using ConwayLib;
using System.Drawing;
using System.Linq;

namespace ConwayLib
{
    /// <summary>
    /// Assists with displaying a board of Conway's Game of Life on a Windows Console that supports
    /// escape-codes to move the cursor and change the text colour.
    /// </summary>
    public static class BoardConsoleExtensions
    {
        private static readonly Dictionary<int, int> sColours = new()
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
      { 9, 93 },
      { 10, 94 },
      { 11, 95 },
      { 12, 96 },
      { 13, 97 },

    };

        public const string cHome = "\u001b[0;0H";
        private const string cDefaultColour = "\u001b[0m";

        internal static string ColourCode(int n) => $"\u001b[{sColours[n % sColours.Count]}m";

        public static string ToConsoleString(this IReadableBoard board, Rectangle? window = null, StringBuilder builder = null, Func<IReadableBoard, int, int, int> getValueForColour = null)
        {
            getValueForColour ??= (b, x, y) => b.Neighbours(x, y);

            if (builder is null)
            {
                builder = new StringBuilder();
            }
            else
            {
                builder.Clear();
            }

            builder.Append(cHome);

            int xOffset = window?.X ?? 0;
            int yOffset = window?.Y ?? 0;

            for (int y = 0; y < (window?.Height ?? board.Height); ++y)
            {
                for (int x = 0; x < (window?.Width ?? board.Width); ++x)
                {
                    if (board.Cell(x + xOffset, y + yOffset))
                    {
                        builder.Append(ColourCode(getValueForColour(board, x + xOffset, y + yOffset)));
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