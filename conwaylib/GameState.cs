using System;
using System.Text.Json;
using System.Drawing;

namespace ConwayLib
{
    [Serializable]
    public class GameState
    {
        public string Format {get; set;}
        public Point[] SparseData {get; set;}
        public bool[][] DenseData { get; set; }
    } 
}