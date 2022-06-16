using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Drawing;

namespace ConwayLib;

public enum DensityOption {Dense, Sparse}
public class GameState
{
  [JsonPropertyName("Format")]
  public string FormatText {get; set;}
  [JsonIgnore]
  public DensityOption Format { get { return Enum.Parse<DensityOption>(FormatText); } set {FormatText = value.ToString();} }
  public CellCoord[] SparseData {get; set;}
  public bool[][] DenseData { get; set; }
  public int Width { get; set; }
  public int Height { get; set; }
}