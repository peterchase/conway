using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Linq;

namespace ConwayLib
{
    public class BoolArrayHasher : IDisposable
    {
        private HashAlgorithm sha;
        public byte[] GetUniqueHash(IEnumerable<bool> boolValues)
        {
            using(sha = SHA256.Create())
            {
                var rawBytes = ConvertBoolToByteArray(boolValues.ToArray());
                return sha.ComputeHash(rawBytes);
            }
        } 

        public void Dispose()
        {
            sha.Dispose();
        }

        internal byte[] ConvertBoolToByteArray(bool[] boolValues)
        {
            int byteLength = Math.DivRem(boolValues.Length, 8, out int remainder);
            byteLength += remainder == 0 ? 0 : 1;
            var bits = new BitArray(boolValues); 
            byte[] bytes = new byte[byteLength];
            bits.CopyTo(bytes, 0);
            return bytes;
        }
    }
}