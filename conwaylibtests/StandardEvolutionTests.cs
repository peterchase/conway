using NUnit.Framework;

namespace ConwayLib.Tests;

[TestFixture]
public sealed class StandardEvolutionTests
{
  [TestCase(false, 0, false)]
  [TestCase(false, 2, false)]
  [TestCase(false, 3, true)]
  [TestCase(true, 0, false)]
  [TestCase(true, 2, true)]
  [TestCase(true, 3, true)]
  [TestCase(true, 4, false)]
  public void GetNextState_ShouldReturnCorrectState(bool curState, int neighbours, bool expected)
  {
    Assert.That(StandardEvolution.Instance.GetNextState(curState, neighbours), Is.EqualTo(expected));
  }
}