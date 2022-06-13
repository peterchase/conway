using NUnit.Framework;
using System;
using System.Threading.Tasks;
using System.Reflection;
using System.IO;

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
        public async Task GameStateSerializer_ShouldSerializeCorrectly(int seed, int width, int height, DensityOption option)
        {
            var board = new Board(width, height).Randomise(new Random(seed), 0.5);

            var state = board.GetCurrentState(option);

            using MemoryStream stream = new();
            await GameStateSerializer.SerializeJson(state, stream);               

            stream.Seek(0, SeekOrigin.Begin);

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