using System;
using System.Text.Json;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;

namespace ConwayLib;

public static class GameStateSerializer
{
  public static async Task<GameState> DeserializeJson(Stream stream)
  {
    return  await JsonSerializer.DeserializeAsync<GameState>(stream);
  }

  public static async Task<GameState> DeserializeJson(string filePath)
  {
    using FileStream stream = File.OpenRead(filePath);
    return await DeserializeJson(stream);
  }

  public static async Task SerializeJson(GameState state, Stream stream)
  {
    await JsonSerializer.SerializeAsync(stream, state);
    await stream.FlushAsync();
  }

  public static async Task SerializeJson(GameState state, string filePath)
  {
    using FileStream stream = File.Open(filePath, FileMode.Create);
    await SerializeJson(state, stream);
  }
}