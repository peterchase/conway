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
    public class BoolArrayHasher
    {
        public byte[] GetUniqueHash(IEnumerable<bool> boolValues)
        {
            using(HashAlgorithm sha = SHA256.Create())
            {
                using (var stream = new MemoryStream())
                {
                    IFormatter formatter = new BinaryFormatter(); 
                    formatter.Serialize(stream, boolValues.ToArray());
                    var rawBytes = stream.ToArray();
                    stream.Close();
                    return sha.ComputeHash(rawBytes);
                }
            }
        } 

        public byte[] ConvertBoolToByteArray(IEnumerable<bool> boolValues)
        {
            int byteLength = (int)(boolValues.Count()/8+1);
            var bits = new BitArray(boolValues.ToArray()); 
            byte[] bytes = new byte[byteLength];
            bits.CopyTo(bytes, 0);
            return bytes;
        }
    }
}