using NUnit.Framework;
using System;
using System.Threading.Tasks;
using System.Drawing;
using System.Reflection;

namespace ConwayLib.Tests
{
    [TestFixture]
    public class GameStateSerializerTests
    {
        [Test]
        public async Task GameStateSerializer_ShouldDeserializeCorrectly()
        {
            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("ConwayLib.Tests.TestGameState.json"))
            {
                GameState result = await GameStateSerializer.DeserializeJson(stream);
                Point[] pointData = result.SparseData;

                Assert.That(result.Format, Is.EqualTo(DensityOption.Sparse));
                Assert.That(pointData, Is.Not.Null);                
                Assert.That(result.Width, Is.EqualTo(10));
                Assert.That(pointData[0], Is.EqualTo(new Point(0, 5)));
            }
        }

    }
}