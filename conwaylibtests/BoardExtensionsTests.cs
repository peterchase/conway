using System;
using NUnit.Framework;

namespace ConwayLib.Tests;

[TestFixture]
public sealed class BoardExtensionsTests
{
  [TestCase(0, 0, 2)]
  [TestCase(2, 2, 4)]
  [TestCase(3, 3, 3)]
  public void Neighbours_ShouldGiveCorrectResult(int x, int y, int expected)
  {
    var board = new Board(4, 4)
    {
      { 0, 1, true },
      { 1, 1, true },
      { 2, 2, true },
      { 2, 3, true },
      { 3, 2, true },
      { 3, 3, true },
    };

    Assert.That(board.Neighbours(x, y), Is.EqualTo(expected));
  }

  [Test]
  public void Randomise_ShouldGivePlausibleResult()
  {
    var board = new Board(50, 50);
    board.Randomise(new Random(), 0.5);

    // It is vanishingly unlikely that randomise could correctly give all-true or
    // all-false for this size of board. So, if we see all-true or all-false, it's a bug.
    Assert.That(board, Has.Some.True);
    Assert.That(board, Has.Some.False);
  }
}