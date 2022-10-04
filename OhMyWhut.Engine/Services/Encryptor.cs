using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OhMyWhut.Engine.Services
{
    public class Encryptor
    {
        public static string DesEncodeString(string source, string[] keys)
        {
            var sourceGroupBytes = GetStringGroupBytes(source);
            var keyGroupBytes = keys.Select(x => GetStringGroupBytes(x)).SelectMany(x => x);
            var encrypted = sourceGroupBytes.Select(x => EncodeGroup(x, keyGroupBytes)).SelectMany(x => x);
            var result = Bytes2String(encrypted);
            return result;
        }

        private static byte[] EncodeGroup(byte[] source, IEnumerable<byte[]> keys)
        {
            foreach (var key in keys)
            {
                source = Encode(source, key);
            }
            return source;
        }

        private static byte[] Encode(byte[] source, byte[] key)
        {
            var keys = GenerateKeys(key);
            var ipByte = GetPermute(source);
            var ipLeftInt = (uint)(ipByte[0] << 24 | ipByte[1] << 16 |
                                    ipByte[2] << 8 | ipByte[3]);
            var ipRightInt = (uint)(ipByte[4] << 24 | ipByte[5] << 16 |
                                    ipByte[6] << 8 | ipByte[7]);
            uint ipLeftIntIv;
            for (int i = 0; i < 16; i++)
            {
                ipLeftIntIv = ipLeftInt;
                ipLeftInt = ipRightInt;
                ipRightInt = PermuteP(PermuteBox(XorEqulas(ExpandPermute(ipRightInt), keys[i]))) ^ ipLeftIntIv;
            }

            return FinalPermute((ulong)ipRightInt << 32 | ipLeftInt);
        }

        private static byte[] XorEqulas(byte[] a, byte[] b)
        {
            for (int i = 0; i < a.Length; i++)
            {
                a[i] ^= b[i];
            }
            return a;
        }

        private static readonly int[] permuteOrder = new int[] { 4, 0, 5, 1, 6, 2, 7, 3 };
        private static byte[] GetPermute(byte[] bytes) // must be byte[8]
        {
            var ipBytes = new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 };
            var mask = 0x80;
            foreach (var index in permuteOrder)
            {
                foreach (var b in bytes.Reverse())
                {
                    ipBytes[index] <<= 1;
                    ipBytes[index] |= (b & mask) > 0 ? (byte)0x01 : (byte)0x00;
                }
                mask >>= 1;
            }
            return ipBytes;
        }

        private static readonly int[][] permuteOrderEx = new int[][]
        {
            new int[] { 31, 0, 1, 2, 3, 4, 3, 4 },
            new int[] { 5, 6, 7, 8, 7, 8, 9, 10 },
            new int[] { 11, 12, 11, 12, 13, 14, 15, 16 },
            new int[] { 15, 16, 17, 18, 19, 20, 19, 20 },
            new int[] { 21, 22, 23, 24, 23, 24, 25, 26 },
            new int[] { 27, 28, 27, 28, 29, 30, 31, 0 },
        };
        private static byte[] ExpandPermute(uint rightHalfPermute)
        {
            var bits = new List<byte>(32);
            for (uint mask = (uint)0x01 << 31; mask > 0; mask >>= 1)
            {
                bits.Add((rightHalfPermute & mask) > 0 ? (byte)0x01 : (byte)0x00);
            }
            var result = new byte[] { 0, 0, 0, 0, 0, 0 };
            var index = 0;
            foreach (var byteIndex in permuteOrderEx)
            {
                foreach (var innerIndex in byteIndex)
                {
                    result[index] <<= 1;
                    result[index] |= bits[innerIndex];
                }
                index++;
            }

            return result;
        }

        private static readonly int[] permuteOrderP = new int[]
        {
              15,    6,   19,   20,   28,   11,   27,   16,
               0,   14,   22,   25,    4,   17,   30,    9,
               1,    7,   23,   13,   31,   26,    2,    8,
              18,   12,   29,    5,   21,   10,    3,   24,
        };
        private static uint PermuteP(uint data)
        {
            var bits = new List<byte>(32);
            for (uint mask = (uint)0x01 << 31; mask > 0; mask >>= 1)
            {
                bits.Add((data & mask) > 0 ? (byte)0x01 : (byte)0x00);
            }
            uint result = 0;
            foreach (var index in permuteOrderP)
            {
                result <<= 1;
                result |= bits[index];
            }
            return result;
        }

        private static readonly int[] permuteOrderFinal = new int[]
        {
            39, 7, 47, 15, 55, 23, 63, 31,
            38, 6, 46, 14, 54, 22, 62, 30,
            37, 5, 45, 13, 53, 21, 61, 29,
            36, 4, 44, 12, 52, 20, 60, 28,
            35, 3, 43, 11, 51, 19, 59, 27,
            34, 2, 42, 10, 50, 18, 58, 26,
            33, 1, 41,  9, 49, 17, 57, 25,
            32, 0, 40,  8, 48, 16, 56, 24,
        };
        private static byte[] FinalPermute(ulong data)
        {
            var bits = new List<byte>(64);
            ulong mask = (ulong)0x01 << 63;
            for (var k = 0; k < 64; k++, mask >>= 1)
            {
                bits.Add((data & mask) > 0 ? (byte)0x01 : (byte)0x00);
            }

            var result = new byte[8];
            var i = 0;
            foreach (var index in permuteOrderFinal)
            {
                var k = i / 8;
                result[k] <<= 1;
                result[k] |= bits[index];
                i++;
            }
            return result;
        }

        private static readonly uint[][] sn = new uint[][]
        {
            new uint[]
            {
                14, 4, 13, 1, 2, 15, 11, 8, 3, 10, 6, 12, 5, 9, 0, 7,
                0, 15, 7, 4, 14, 2, 13, 1, 10, 6, 12, 11, 9, 5, 3, 8,
                4, 1, 14, 8, 13, 6, 2, 11, 15, 12, 9, 7, 3, 10, 5, 0,
                15, 12, 8, 2, 4, 9, 1, 7, 5, 11, 3, 14, 10, 0, 6, 13
            },
            new uint[]
            {
                15, 1, 8, 14, 6, 11, 3, 4, 9, 7, 2, 13, 12, 0, 5, 10,
                3, 13, 4, 7, 15, 2, 8, 14, 12, 0, 1, 10, 6, 9, 11, 5,
                0, 14, 7, 11, 10, 4, 13, 1, 5, 8, 12, 6, 9, 3, 2, 15,
                13, 8, 10, 1, 3, 15, 4, 2, 11, 6, 7, 12, 0, 5, 14, 9
            },
            new uint[]
            {
                10, 0, 9, 14, 6, 3, 15, 5, 1, 13, 12, 7, 11, 4, 2, 8,
                13, 7, 0, 9, 3, 4, 6, 10, 2, 8, 5, 14, 12, 11, 15, 1,
                13, 6, 4, 9, 8, 15, 3, 0, 11, 1, 2, 12, 5, 10, 14, 7,
                1, 10, 13, 0, 6, 9, 8, 7, 4, 15, 14, 3, 11, 5, 2, 12
            },
            new uint[]
            {
                7, 13, 14, 3, 0, 6, 9, 10, 1, 2, 8, 5, 11, 12, 4, 15,
                13, 8, 11, 5, 6, 15, 0, 3, 4, 7, 2, 12, 1, 10, 14, 9,
                10, 6, 9, 0, 12, 11, 7, 13, 15, 1, 3, 14, 5, 2, 8, 4,
                3, 15, 0, 6, 10, 1, 13, 8, 9, 4, 5, 11, 12, 7, 2, 14
            },
            new uint[]
            {
                2, 12, 4, 1, 7, 10, 11, 6, 8, 5, 3, 15, 13, 0, 14, 9,
                14, 11, 2, 12, 4, 7, 13, 1, 5, 0, 15, 10, 3, 9, 8, 6,
                4, 2, 1, 11, 10, 13, 7, 8, 15, 9, 12, 5, 6, 3, 0, 14,
                11, 8, 12, 7, 1, 14, 2, 13, 6, 15, 0, 9, 10, 4, 5, 3
            },
            new uint[]
            {
                12, 1, 10, 15, 9, 2, 6, 8, 0, 13, 3, 4, 14, 7, 5, 11,
                10, 15, 4, 2, 7, 12, 9, 5, 6, 1, 13, 14, 0, 11, 3, 8,
                9, 14, 15, 5, 2, 8, 12, 3, 7, 0, 4, 10, 1, 13, 11, 6,
                4, 3, 2, 12, 9, 5, 15, 10, 11, 14, 1, 7, 6, 0, 8, 13
            },
            new uint[]
            {
                4, 11, 2, 14, 15, 0, 8, 13, 3, 12, 9, 7, 5, 10, 6, 1,
                13, 0, 11, 7, 4, 9, 1, 10, 14, 3, 5, 12, 2, 15, 8, 6,
                1, 4, 11, 13, 12, 3, 7, 14, 10, 15, 6, 8, 0, 5, 9, 2,
                6, 11, 13, 8, 1, 4, 10, 7, 9, 5, 0, 15, 14, 2, 3, 12
            },
            new uint[]
            {
                13, 2, 8, 4, 6, 15, 11, 1, 10, 9, 3, 14, 5, 0, 12, 7,
                1, 15, 13, 8, 10, 3, 7, 4, 12, 5, 6, 11, 0, 14, 9, 2,
                7, 11, 4, 1, 9, 12, 14, 2, 0, 6, 10, 13, 15, 3, 5, 8,
                2, 1, 14, 7, 4, 10, 8, 13, 15, 12, 9, 0, 3, 5, 6, 11
            },
        };

        private static uint PermuteBox(byte[] data) // byte[6]
        {
            ulong expandByte = (ulong)data[0] << 56 | (ulong)data[1] << 48 |
                               (ulong)data[2] << 40 | (ulong)data[3] << 32 |
                               (ulong)data[4] << 24 | (ulong)data[5] << 16;
            uint result = 0;
            ulong mask = (ulong)0xFC << 56;
            for (int m = 0; m < 8; m++)
            {
                var part = (expandByte & mask) >> 58 - m * 6;
                var i = part >> 4 & 0x02 | part & 0x01;
                var j = (part & 0x1E) >> 1;

                result <<= 4;
                result |= sn[m][(i << 4) + j];
                mask >>= 6;
            }
            return result;
        }

        private static readonly int[] loopCount = new int[]
        {
            1, 1, 2, 2, 2, 2, 2, 2,
            1, 2, 2, 2, 2, 2, 2, 1
        };

        private static readonly int[] pickIndex = new int[]
        {
            13,16,10,23,0,4,2,27,14,5,20,9,22,18,11,3,25,7,15,
            6,26,19,12,1,40,51,30,36,46,54,29,39,50,44,32,47,
            43,48,38,55,33,52,45,41,49,35,28,31
        };

        private static byte[][] GenerateKeys(byte[] bytes)
        {
            var keys = new byte[16][];
            var bytesReverse = bytes.Reverse();
            ulong keyInt = 0;
            for (var byteMask = 0x80; byteMask > 1; byteMask >>= 1)
            {
                foreach (var @byte in bytesReverse)
                {
                    keyInt <<= 1;
                    keyInt |= (@byte & byteMask) > 0 ? 0x01 : (ulong)0x00;
                }
            }
            var keyBits = new List<byte>(56);

            for (ulong mask = (ulong)0x01 << 55; mask > 0; mask >>= 1)
            {
                keyBits.Add((keyInt & mask) > 0 ? (byte)0x01 : (byte)0x00);
            }

            for (var i = 0; i < 16; i++)
            {
                for (int j = 0; j < loopCount[i]; j++)
                {
                    var tempLeft = keyBits[0];
                    var tempRight = keyBits[28];
                    for (var k = 0; k < 27; k++)
                    {
                        keyBits[k] = keyBits[k + 1];
                        keyBits[k + 28] = keyBits[k + 29];
                    }
                    keyBits[27] = tempLeft;
                    keyBits[55] = tempRight;
                }

                var resultKeyBytes = new byte[6];
                var index = 0;
                foreach (var k in pickIndex)
                {
                    var m = index / 8;
                    resultKeyBytes[m] <<= 1;
                    resultKeyBytes[m] |= keyBits[k];
                    index++;
                }
                keys[i] = resultKeyBytes;
            }

            return keys;
        }

        private static IEnumerable<byte[]> GetStringGroupBytes(string key)
        {
            var mod = key.Length % 4;
            var keyIter = mod == 0 ? key : key.Concat(Enumerable.Repeat('\0', 4 - mod));
            var chunks = keyIter.Chunk(4);
            return chunks.Select(x => String2Bytes(x));
        }

        private static byte[] String2Bytes(string str)
        {
            return String2Bytes(str.ToCharArray());
        }

        private static byte[] String2Bytes(char[] str)
        {
            var result = new List<byte>(str.Length * 2);
            foreach (var c in str)
            {
                result.Add(0);
                result.Add((byte)c);
            }
            return result.ToArray();
        }

        private static string Bytes2String(IEnumerable<byte> bytes)
        {
            var sb = new StringBuilder(128);
            foreach (var b in bytes)
            {
                sb.Append(b.ToString("X2"));
            }
            return sb.ToString();
        }
    }
}
