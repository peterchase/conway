using System;
using NUnit.Framework;

namespace ConwayLib.Tests;

[TestFixture]
public sealed class BoardExtensionsTests
{
  [Test]
  public void MutableCopy_ShouldGiveIdenticalBoard([Values(5, 25)] int size)
  {
    var original = new Board(size, size).Randomise(0.5);
    var copy = original.MutableCopy();
    Assert.That(original, Is.EqualTo(copy));
  }

  [TestCase(0, 0, 2)]
  [TestCase(2, 2, 6)]
  [TestCase(3, 3, 3)]
  [TestCase(3, 0, 1)]
  [TestCase(0, 3, 1)]
  public void Neighbours_ShouldGiveCorrectResult(int x, int y, int expected)
  {
    var board = new Board(4, 4)
    {
      { 0, 1, true },
      { 1, 1, true },
      { 1, 2, true },
      { 2, 2, true },
      { 3, 1, true },
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