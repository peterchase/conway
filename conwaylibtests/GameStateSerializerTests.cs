using NUnit.Framework;
using System;
using System.Threading.Tasks;
using System.Drawing;

namespace ConwayLib.Tests
{
    [TestFixture]
    public class GameStateSerializerTests
    {
        [Test]
        public async Task GameStateSerializer_ShouldDeserializeCorrectly()
        {
            string myJsonPath = @"C:\Users\user3\Git\conway\conwaylibtests\TestGameState.json";
            
            GameState result = await GameStateSerializer.DeserializeJson(myJsonPath);
            Point[] pointData = result.SparseData;

            Assert.That(result.Format, Is.EqualTo(DensityOption.Sparse));
            Assert.That(pointData, Is.Not.Null);

            Assert.That(pointData[0], Is.EqualTo(new Point(0,5)));
        }

    }
}