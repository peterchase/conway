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

        public int GetHashCode(byte[] bytes)
        {
            unchecked
            {
                // byte arrays do not support .Sum(), which is why I think I have to use a loop;
                int sum = 0;
                foreach (var b in bytes)
                {
                    sum+=b;
                }
                return sum;                
            }
        }
    }
}