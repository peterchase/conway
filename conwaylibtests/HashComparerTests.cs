using System;
using System.Collections;
using System.Collections.Generic;
using NUnit;
using NUnit.Framework;

namespace ConwayLib.TestCase
{
    [TestFixture]
    public sealed class HashComparerTests
    {
        [TestCase(new byte[] {0,1,2,3})]
        [TestCase(new byte[] {255,128,45,32})]
        public void HashSetOfByteArrays_ShouldSayItContainsArray_WhenItExists(byte[] myHash)
        {
            HashSet<byte[]> myHashSet = new HashSet<byte[]>(new HashComparer());
            myHashSet.Add(myHash);

            Assert.That(myHashSet.Contains(myHash), Is.True);
        }        
        
        [TestCase(new byte[] {0,1,2,3})]
        [TestCase(new byte[] {255,128,45,32})]
        public void HashSetOfByteArrays_ShouldSayItDoesntContainArray_WhenItDoesntExists(byte[] myHash)
        {
            HashSet<byte[]> myHashSet = new HashSet<byte[]>(new HashComparer()) {new byte[] {0,1,0,0,0,1}};
            
            Assert.That(myHashSet.Contains(myHash), Is.False);
        }
    }
}