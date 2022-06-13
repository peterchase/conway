using NUnit.Framework;
using System;
using System.Threading.Tasks;
using System.Drawing;
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
                Point[] pointData = result.SparseData;

                Assert.That(result.Format, Is.EqualTo(DensityOption.Sparse));
                Assert.That(pointData, Is.Not.Null);                
                Assert.That(result.Width, Is.EqualTo(5));
                Assert.That(pointData[0], Is.EqualTo(new Point(0, 4)));
            }
        }

        [Test]
        public async Task GameStateSerializer_ShouldSserializeSparseCorrectly()
        {
            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("ConwayLib.Tests.TestSparseGameState.json"))
            {
                GameState result = await GameStateSerializer.DeserializeJson(stream);

                //state.Format = DensityOption.Sparse;
                Assert.That(result.Format, Is.EqualTo(DensityOption.Sparse));

                await GameStateSerializer.SerializeJson(@"C:\Users\user3\Git\conway\conwaylibtests\SerializeTestSparse.json", result);
            }
        }

        [Test]
        public async Task GameStateSerializer_ShouldSserializeDenseCorrectly()
        {
            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("ConwayLib.Tests.TestDenseGameState.json"))
            {
                GameState result = await GameStateSerializer.DeserializeJson(stream);

                //state.Format = DensityOption.Sparse;
                Assert.That(result.Format, Is.EqualTo(DensityOption.Dense));

                await GameStateSerializer.SerializeJson(@"C:\Users\user3\Git\conway\conwaylibtests\SerializeTestDense.json", result);
            }
        }

        [TestCase(@"C:\Users\user3\Git\conway\conwaylibtests\SerializeTestDense.json")]
        public async Task GameStateSerializer_ShouldReDeserializeSparseCorrectly(string path)
        {
            using (var stream = File.OpenRead(path))
            {
                GameState result = await GameStateSerializer.DeserializeJson(stream);

                Assert.That(result.Format, Is.EqualTo(DensityOption.Dense));
            }
        }
    }
}