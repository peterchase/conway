using NUnit.Framework;
using ConwayWebClient;
using ConwayLib;
using System.Threading.Tasks;
using System.Linq;

namespace ConwayWebClientTests
{
    [TestFixture]
    public class WebClientTests
    {

        [Test]
        public async Task GettingGameState_ShouldGetCorrectBoard()
        {
            GameState state = await ConwayClient.GetGameStateAsync(1);
            CellCoord[] expectedCells = new CellCoord[] { new CellCoord(5, 5), new CellCoord(7, 8), new CellCoord(2, 1) };

            Assert.That(state.SparseData.Length, Is.EqualTo(expectedCells.Length));

            for (int i = 0; i < state.SparseData.Length; i++)
            {
                Assert.That(state.SparseData[i], Is.EqualTo(expectedCells[i]));
            }
        }

        [Test]
        public async Task GettingBoards_ShouldReturnBoards()
        {
            var boards = (await ConwayClient.GetBoardsAsync()).ToArray();
            Assert.That(boards.Length, Is.EqualTo(7));
            foreach (var board in boards)
            {
                Assert.That(board, Is.Not.Null);
            }
        }
    }
}