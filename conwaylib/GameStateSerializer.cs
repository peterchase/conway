using System;
using System.Text.Json;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;

namespace ConwayLib
{
    public static class GameStateSerializer
    {
        public static async Task<GameState> DeserializeJson(string filePath)
        {
            using(FileStream br = File.OpenRead(filePath))
            {
                return  await JsonSerializer.DeserializeAsync<GameState>(br);
            }
        }
    }
}