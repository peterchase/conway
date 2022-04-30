using NUnit.Framework;

namespace ConwayLib.Tests
{
    [TestFixture]
    public sealed class BoardTests
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
    }
}
