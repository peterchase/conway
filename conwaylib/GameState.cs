using System;
using System.Text.Json;
using System.Drawing;

namespace ConwayLib
{
    public enum DensityOption {Dense, Sparse}
    public class GameState
    {
        public string FormatText {get; set;}
        public DensityOption Format {
            get
            {
                return Enum.Parse<DensityOption>(FormatText);
            }
        }
        public Point[] SparseData {get; set;}
        public bool[][] DenseData { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
    } 
}