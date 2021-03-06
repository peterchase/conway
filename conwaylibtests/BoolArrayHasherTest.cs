using NUnit;
using NUnit.Framework;
using System;
using System.Linq;
using System.Diagnostics;

namespace ConwayLib.Tests;

[TestFixture]
public sealed class BoolArrayHasherTests
{
  [Test]
  public void GetUniqueHash_ShouldGiveUniqueHashes()
  {
    var hasher = new BoolArrayHasher();
    var random = new Random();
    var data = Enumerable.Range(0, 100).Select(i => Enumerable.Range(0, i).Select(j => random.NextDouble() > 0.5).ToArray());

    var results = data.Select((x, i) => hasher.GetUniqueHash(x, i));

    // All hashes should be unique
    Assert.That(results, Is.Unique);
  }
}