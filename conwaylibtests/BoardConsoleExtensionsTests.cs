using System;
using NUnit.Framework;
using System.Drawing;
using NSubstitute;

namespace ConwayLib.Tests
{
    [TestFixture]
  public sealed class BoardConsoleExtensionsTests
  {
    [Test]
    public void ToConsoleString_ShouldReturnAString_WhenNoWindowSpecified([Values(5, 25)] int size)
    {
      var board = new Board(size, size).Randomise(0.5);
      Assert.That(board.ToConsoleString(), Is.Not.Null);
    }

    [Test]
    public void ToConsoleString_ShouldReturnAString_WhenWindowSpecified()
    {
      var board = new Board(20, 20).Randomise(0.5);
      var rectangle = new Rectangle(10, 10, 10, 5);

      Assert.That(board.ToConsoleString(rectangle), Is.Not.Null);
    }

    [Test]
    public void ToConsoleString_ShouldReturnAStringWithExpectedColourCode_WhenColourBySpecified([Values(5, 25)] int size)
    {
      var board = new Board(size, size).Randomise(0.5);
      var getValueForColour = Substitute.For<Func<IReadableBoard, int, int, int>>();
      getValueForColour.Invoke(board, Arg.Any<int>(), Arg.Any<int>()).Returns(3);
      string expectedCode = BoardConsoleExtensions.ColourCode(3);
      string notExpectedCode = BoardConsoleExtensions.ColourCode(0);
      string result = board.ToConsoleString(getValueForColour: getValueForColour);
      Assert.That(result, Is.Not.Null);
      Assert.That(result, Does.Contain(expectedCode));
      Assert.That(result, Does.Not.Contain(notExpectedCode));
    }
  }
}