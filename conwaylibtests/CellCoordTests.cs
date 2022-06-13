using System;
using System.Collections;
using System.Collections.Generic;
using NUnit;
using NUnit.Framework;

namespace ConwayLib.TestCase
{
    [TestFixture]
    public sealed class CellCoordTests
    {
        [Test]
        public void TwoCellCoords_ShouldBeEqual_WhenSameXY()
        {            
            CellCoord c1 = new(0,0);
            CellCoord c2 = new(0,0);

            Assert.That(c1.X, Is.EqualTo(0));
            Assert.That(c1, Is.EqualTo(c2));
            Assert.That(c1.GetHashCode(), Is.EqualTo(c2.GetHashCode()));
        }
    }
}