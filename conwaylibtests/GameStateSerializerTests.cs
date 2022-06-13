using NUnit.Framework;
using System;
using System.Threading.Tasks;
using System.Drawing;
using System.Reflection;
using System.IO;
using System.Diagnostics;

namespace ConwayLib.Tests
{
    [TestFixture]
    public class GameStateSerializerTests
    {
        [Test]
        public async Task GameStateSerializer_ShouldDeserializeSparseCorrectly()
        {
            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("ConwayLib.Tests.TestSparseGameState.json"))
            {
                GameState result = await GameStateSerializer.DeserializeJson(stream);
                CellCoord[] pointData = result.SparseData;

                Assert.That(result.Format, Is.EqualTo(DensityOption.Sparse));
                Assert.That(pointData, Is.Not.Null);                
                Assert.That(result.Width, Is.EqualTo(5));
                Assert.That(pointData[0], Is.EqualTo(new CellCoord(0, 4)));
            }
        }

        [TestCase(0,5,5, DensityOption.Sparse)]
        [TestCase(0,5,5, DensityOption.Dense)]
        [TestCase(0,50,50, DensityOption.Dense)]
        [TestCase(0,150,150, DensityOption.Sparse)]
        [TestCase(0,1500,1500, DensityOption.Sparse)]
        public async Task GameStateSerializer_ShouldSerializeCorrectly(int seed, int width, int height, DensityOption option, bool outputToFile = false)
        {
            var sw = new Stopwatch();
            var board = new Board(width, height).Randomise(new Random(seed), 0.5);

            sw.Start();
            var state = board.GetCurrentState(option);

            using MemoryStream stream = new();
            await GameStateSerializer.SerializeJson(state, stream);
            sw.Stop();

            if (outputToFile)
            {
                using (StreamWriter fs = new StreamWriter(File.Open(@"C:\Users\user3\Git\conway\conwaylibtests\TempSerialized.data", FileMode.Create)))
                    await fs.WriteLineAsync($"{width}x{height}: {sw.ElapsedMilliseconds}ms");
                using (Stream fs = File.Open(@"C:\Users\user3\Git\conway\conwaylibtests\TempSerialized.json", FileMode.Create))
                    await GameStateSerializer.SerializeJson(state, fs);
            }
                

            stream.Position = 0;

            var reDeserialized = await GameStateSerializer.DeserializeJson(stream);

            Assert.That(reDeserialized.Format, Is.EqualTo(option));

            if (option == DensityOption.Sparse && width * height > 30)
            {
                int count = reDeserialized.SparseData.Length;
                static bool inRange(double val, double min, double max) { return min <= val && val < max; }
                Assert.That(inRange(count, width * height * 0.4, width * height * 0.6), "Number of live cells was not in the 40-60th percentile (expected 50% alive)");
            }
        }
    }
}