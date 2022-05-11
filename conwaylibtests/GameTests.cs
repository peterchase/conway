using NSubstitute;
using NUnit.Framework;

namespace ConwayLib.Tests
{
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
            IReadableBoard afterTurn = game.Turn();

            // The returned board should have the same alive/dead cells as the initial...
            Assert.That(afterTurn, Is.EqualTo(initial));

            // ... but it should not be the same object
            Assert.That(afterTurn, Is.Not.SameAs(initial));
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
            IReadableBoard afterTurn = game.Turn();

            Assert.That(afterTurn, Has.All.EqualTo(value));
        }
    }
}