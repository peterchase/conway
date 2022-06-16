using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Linq;

namespace ConwayLib;

public class BoolArrayHasher : IDisposable
{
  private readonly HashAlgorithm msha;

  public BoolArrayHasher()
  {
    msha = SHA256.Create();
  }
  public byte[] GetUniqueHash(IEnumerable<bool> boolValues)
  {
    var rawBytes = ConvertBoolToByteArray(boolValues.ToArray());
    return msha.ComputeHash(rawBytes);
  } 

  public void Dispose()
  {
    msha.Dispose();
  }
        
  /// <summary>
  /// For reference:
  /// A bool array reads
  /// [Byte 2 ----->] | [Byte 1 ----->] | [Byte 0 ----->]
  /// 1 0 0 0 0 0 0 0 | 0 0 1 0 0 0 0 1 | 1 0 0 1 0 0 0 0
  /// </summary>
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