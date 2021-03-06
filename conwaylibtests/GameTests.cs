using NSubstitute;
using NUnit.Framework;
using System;

namespace ConwayLib.Tests;

[TestFixture]
public sealed class GameTests
{
  [Test]
  public void Turn_ShouldGiveIdenticalBoard_WhenEvolutionDoesNothing()
  {
    var initial = new Board(10, 10).Randomise(0.5);

    // Set up an evolution that just returns current state
    var evolution = Substitute.For<IEvolution>();
    evolution.GetNextState(Arg.Any<bool>(), Arg.Any<int>())
      .Returns(ci => ci.Arg<bool>());

    var game = new Game(initial, evolution);
    IReadableBoard afterTurn = game.Turn(out bool stop);            


    // The returned board should have the same alive/dead cells as the initial...
    Assert.That(afterTurn, Is.EqualTo(initial));

    // ... but it should not be the same object
    Assert.That(afterTurn, Is.Not.SameAs(initial));
  }


  [Test]
  public void Turn_ShouldSignalStop_WhenPreviousExists([Values(0,1,2,3,4,5)] int seed)
  {
    var initial = new Board(10, 10).Randomise(new Random(seed), 0.5);

    // Set up an evolution that just returns current state
    var evolution = Substitute.For<IEvolution>();
    evolution.GetNextState(Arg.Any<bool>(), Arg.Any<int>())
      .Returns(ci => ci.Arg<bool>());

    var game = new Game(initial, evolution);
    IReadableBoard afterTurn = game.Turn(out bool stop);        

    // Next evolution should exist in history
    Assert.That(stop, Is.True);
  }

  [Test]
  public void Turn_ShouldNotSignalStop_WhenPreviousDoesntExist([Values(0,1,2,3,4,5)] int seed)
  {
    var initial = new Board(10, 10).Randomise(new Random(seed), 0.5);

    // Set up an evolution that just returns current state
    var evolution = StandardEvolution.Instance;

    var game = new Game(initial, evolution);
    IReadableBoard afterTurn = game.Turn(out bool stop);            

    // Next evolution should not exist in history
    Assert.That(stop, Is.False);
  }

  [Test]
  public void Turn_ShouldGiveConstantBoard_WhenEvolutionGivesConstant([Values(true, false)] bool value)
  {
    var initial = new Board(10, 10).Randomise(0.5);

    // Set up an evolution that just returns constant value
    var evolution = Substitute.For<IEvolution>();
    evolution.GetNextState(Arg.Any<bool>(), Arg.Any<int>())
      .Returns(ci => value);

    var game = new Game(initial, evolution);

    IReadableBoard afterTurn = game.Turn(out _);
    Assert.That(afterTurn, Has.All.EqualTo(value));
  }

  [Test]
  public void Turn_ShouldBehaveCorrectly_OnSecondCall()
  {
    bool value = true;
    var initial = new Board(10, 10).Randomise(0.5);

    // Set up an evolution that returns what's in value at time of Turn()
    var evolution = Substitute.For<IEvolution>();
    evolution.GetNextState(Arg.Any<bool>(), Arg.Any<int>())
      .Returns(ci => value);

    var game = new Game(initial, evolution);

    IReadableBoard afterTurn1 = game.Turn(out _);
    Assert.That(afterTurn1, Has.All.EqualTo(value));

    value = false;
    IReadableBoard afterTurn2 = game.Turn(out bool stop);
    Assert.That(afterTurn2, Has.All.EqualTo(value));
  }
}