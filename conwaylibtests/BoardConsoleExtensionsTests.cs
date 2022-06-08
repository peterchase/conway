using System;
using NUnit.Framework;
using System.Drawing;
namespace ConwayLib.Tests
{
    [TestFixture]
  public sealed class BoardConsoleExtensionsTests
  {
    [Test]
    public void ToConsoleString_ShouldReturnAString_WhenNoWindowSpecified([Values(5, 25)] int size)
    {
      var original = new Board(size, size).Randomise(0.5);
      Assert.That(original.ToConsoleString(), Is.Not.Null);
    }

    [Test]
    public void ToConsoleString_ShouldReturnAString_WhenWindowSpecified()
    {
      var original = new Board(20, 20).Randomise(0.5);
      var rectangle = new Rectangle(10, 10,10, 5);

      Assert.That(original.ToConsoleString(rectangle), Is.Not.Null);
    }
  }
}