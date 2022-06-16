using System;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace ConwayLib;

public class BoolArrayHasher : IDisposable
{
  private readonly HashAlgorithm msha;

  public BoolArrayHasher()
  {
    msha = SHA256.Create();
  }
  public byte[] GetUniqueHash(IEnumerable<bool> boolValues, int length)
  {
    int rawBytesLength = Math.DivRem(length, 8, out int remainder) + (remainder == 0 ? 0 : 1);
    var rawBytes = new byte[rawBytesLength+4];
    byte mask = 1;
    int index = 0;
    foreach (bool boolValue in boolValues)
    {
      if (boolValue)
      {
        rawBytes[index] |= mask;
      }

      mask <<= 1;
      if (mask == 0)
      {
        ++index;
        ++mask;
      }
    }

    rawBytes[index++] = (byte)(length & 0xFF);
    rawBytes[index++] = (byte)(length >> 8 & 0xFF);
    rawBytes[index++] = (byte)(length >> 16 & 0xFF);
    rawBytes[index++] = (byte)(length >> 24 & 0xFF);

    return msha.ComputeHash(rawBytes);
  } 

  public void Dispose()
  {
    msha.Dispose();
  }
}
