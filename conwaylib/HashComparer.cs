using System;
using System.Collections.Generic;
using System.Linq;

namespace ConwayLib
{
    public class HashComparer : IEqualityComparer<byte[]>
    {
        public bool Equals(byte[] hash1, byte[] hash2)
        {
            if (hash1 == null && hash2 == null)
                return true;
            else if (hash1 == null || hash2 == null)
                return false;
            else
                return hash1.SequenceEqual(hash2);
        }

        public int GetHashCode(byte[] bx)
        {
            int sum = 0;
            bx.Select(x => sum+=x);
            return sum;
        }
    }
}