using NUnit;
using NUnit.Framework;
using System;
using System.Linq;
using System.Diagnostics;

namespace ConwayLib.Tests
{
    [TestFixture]
    public sealed class BoolArrayHasherTests
    {
        [Test]
        public void GetUniqueHash_ShouldGiveUniqueHashes()
        {
            var hasher = new BoolArrayHasher();
            var random = new Random();
            var data = Enumerable.Range(0, 100).Select(i => Enumerable.Range(0, i).Select(j => random.Next() > 0.5).ToArray());

            var results = data.Select(x => hasher.GetUniqueHash(x));

            // All hashes should be unique
            Assert.That(results, Is.Unique);
        }
        
        [TestCase(new bool[] {true, false, false, false, false, false, false, false}, new byte[] {128})]
        [TestCase(new bool[] {true, true, true, true, true, true, true, true}, new byte[] {255})]
        [TestCase(new bool[] {true, true, true, true, true, true, true, true, true}, new byte[] {255, 1})]
        [TestCase(new bool[] {false, true, true, true, true, true, true, true, true}, new byte[] {255, 0})]
        [TestCase(new bool[] {true, false, true, false, false, false, false, false, true, false}, new byte[] {130, 2})]
        public void ConvertBoolToByteArray_ShouldGiveCorrectValues(bool[] boolData, byte[] byteExpected)
        {
            using (var hasher = new BoolArrayHasher())
            {
                var byteResult = hasher.ConvertBoolToByteArray(boolData.Reverse().ToArray());
                for (int j = 0; j < byteResult.Length; j++)
                {
                    var expected = byteExpected[j];
                    Assert.That(byteResult[j], Is.EqualTo(expected));
                }  
            }
        }
    }
}