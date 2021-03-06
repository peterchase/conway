using NUnit.Framework;
using ConwayWebClient;
using System.Net.Http;
using ConwayLib;
using System.Threading.Tasks;
using System.Linq;

namespace ConwayWebClientTests;

[TestFixture]
[Explicit]
public class WebClientTests
{
  private ConwayClient mClient;
  [SetUp]
  public void SetupTests()
  {
    mClient = new ConwayClient("http://localhost:5000/");
  }

  [Test]
  public async Task GettingGameState_ShouldGetCorrectBoard()
  {
    GameState state = (await mClient.GetBoardDetailAsync(1)).ToGameState();
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
    var boards = (await mClient.GetBoardsAsync()).ToArray();
    Assert.That(boards.Length, Is.EqualTo(7));
    foreach (var board in boards)
    {
      Assert.That(board, Is.Not.Null);
    }
  }
        

  [Test]
  public async Task CreatingAndDeleting_ShouldCreateAndDeleteCorrectly()
  {
    IMutableBoard board = new Board(10,10).Randomise(new System.Random(0), 0.2);
    Game game = new(board, StandardEvolution.Instance);

    for (int i = 0; i < 4; i++) { game.Turn(out _); }
    IReadableBoard currentBoard = game.Turn(out _);

    var createResponse = await mClient.CreateBoardAsync(currentBoard.ToBoardDetail("Test board"));

    var boardDetailFromDatabse = await mClient.GetBoardDetailAsync(createResponse.AbsolutePath);

    Assert.That(boardDetailFromDatabse, Is.Not.Null);

    int? id = boardDetailFromDatabse.Info.Id;
    Assert.That(id, Is.Not.Null);

            
    var deleteResponse = await mClient.DeleteBoardAsync(id.Value);

    Assert.That((int)deleteResponse, Is.InRange(200,300));
  }
}