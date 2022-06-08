using System;
using NUnit.Framework;

namespace ConwayLib.Tests
{
    [TestFixture]
  public sealed class BoardConsoleExtensionsTests
  {
    [Test]
    public void ToConsoleString_ShouldReturnAString([Values(5, 25)] int size)
    {
      var original = new Board(size, size).Randomise(0.5);
      Assert.That(original.ToConsoleString(), Is.Not.Null);
    }
  }
}