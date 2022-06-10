using NUnit.Framework;
using System.Linq;
using System.Threading.Tasks;
using System.Drawing;
using System.Reflection;

namespace ConwayLib.Tests
{
    [TestFixture]
    public sealed class BoardTests
    {
        [TestCase("ConwayLib.Tests.TestSparseGameState.json")]
        [TestCase("ConwayLib.Tests.TestDenseGameState.json")]
        public async Task BoardConstructor_ShouldFromSparseFileCorrectly(string resourcePath)
        {
            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourcePath))
            {
                GameState result = await GameStateSerializer.DeserializeJson(stream);
                Board myBoard = new Board(result);
                bool[][] expected = new bool[][] {
                    new bool[] {false,false,false,false,false},
                    new bool[] {false,false,false,false,false},
                    new bool[] {false,false,false,false,false},
                    new bool[] {false,false,false,false,false},
                    new bool[] {true,true,false,false,false}
                };

                Assert.That(expected.FirstOrDefault().Length, Is.EqualTo(myBoard.Width));
                Assert.That(expected.Length, Is.EqualTo(myBoard.Height));

                for (int x = 0; x < result.Width; x++)
                {
                    for (int y = 0; y < result.Height; y++)
                    {
                        Assert.That(myBoard.Cell(x,y), Is.EqualTo(expected[y][x]));
                    }
                }
            }
        }
    }
}