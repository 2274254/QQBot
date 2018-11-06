using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace LibQQProtocol.Util {
    public static class QQTea {
        private static void Code(Byte[] In, Int32 inOffset, Int32 inPos, Byte[] Out, Int32 outOffset, Int32 outPos, Byte[] key) {
            if (outPos > 0) {
                for (var i = 0; i < 8; i++) {
                    In[outOffset + outPos + i] =
                        BitConverter.GetBytes(In[inOffset + inPos + i] ^ Out[outOffset + outPos + i - 8])[0];
                }
            }

            var array = FormatKey(key);
            var num = ConvertByteArrayToUInt(In, outOffset + outPos);
            var num2 = ConvertByteArrayToUInt(In, outOffset + outPos + 4);
            var num3 = 0u;
            var num4 = 2654435769u;
            var num5 = 16u;
            while (num5-- > 0u) {
                num3 += num4;
                num += ((num2 << 4) + array[0]) ^ (num2 + num3) ^ ((num2 >> 5) + array[1]);
                num2 += ((num << 4) + array[2]) ^ (num + num3) ^ ((num >> 5) + array[3]);
            }

            Array.Copy(ConvertUIntToByteArray(num), 0, Out, outOffset + outPos, 4);
            Array.Copy(ConvertUIntToByteArray(num2), 0, Out, outOffset + outPos + 4, 4);
            if (inPos > 0) {
                for (var j = 0; j < 8; j++) {
                    Out[outOffset + outPos + j] =
                        BitConverter.GetBytes(Out[outOffset + outPos + j] ^ In[inOffset + inPos + j - 8])[0];
                }
            }
        }

        private static void Decode(Byte[] In, Int32 inOffset, Int32 inPos, Byte[] Out, Int32 outOffset, Int32 outPos,
            Byte[] key) {
            if (outPos > 0) {
                for (var i = 0; i < 8; i++) {
                    Out[outOffset + outPos + i] =
                        BitConverter.GetBytes(In[inOffset + inPos + i] ^ Out[outOffset + outPos + i - 8])[0];
                }
            } else {
                Array.Copy(In, inOffset, Out, outOffset, 8);
            }

            var array = FormatKey(key);
            var num = ConvertByteArrayToUInt(Out, outOffset + outPos);
            var num2 = ConvertByteArrayToUInt(Out, outOffset + outPos + 4);
            var num3 = 3816266640u;
            var num4 = 2654435769u;
            var num5 = 16u;
            while (num5-- > 0u) {
                num2 -= ((num << 4) + array[2]) ^ (num + num3) ^ ((num >> 5) + array[3]);
                num -= ((num2 << 4) + array[0]) ^ (num2 + num3) ^ ((num2 >> 5) + array[1]);
                num3 -= num4;
            }

            Array.Copy(ConvertUIntToByteArray(num), 0, Out, outOffset + outPos, 4);
            Array.Copy(ConvertUIntToByteArray(num2), 0, Out, outOffset + outPos + 4, 4);
        }

        public static Byte[] Decrypt(Byte[] In, Byte[] key) {
            var into = new List<Byte>();
            var tail = true;
            for (var i = In.Length - 1; i >= 0; i--) {
                if (tail) {
                    if (In[i] == 0x03) {
                        tail = false;
                    } else if (In[i] == 0x00) {
                    } else {
                        into.Insert(0, In[i]);
                        tail = false;
                    }
                } else {
                    into.Insert(0, In[i]);
                }
            }

            return Decrypt(into.ToArray(), 0, into.Count, key);
        }

        public static Byte[] Decrypt(Byte[] In, Int32 offset, Int32 len, Byte[] key) {
            var temp = new Byte[In.Length];
            Buffer.BlockCopy(In, 0, temp, 0, In.Length);
            if (len % 8 != 0 || len < 16) {
                return null;
            }

            var array = new Byte[len];
            for (var i = 0; i < len; i += 8) {
                Decode(temp, offset, i, array, 0, i, key);
            }

            for (var j = 8; j < len; j++) {
                array[j] ^= temp[offset + j - 8];
            }

            var num = array[0] & 7;
            len = len - num - 10;
            var array2 = new Byte[len];
            Array.Copy(array, num + 3, array2, 0, len);
            return array2;
        }

        public static Byte[] Encrypt(Byte[] In, Byte[] key) {
            return Encrypt(In, 0, In.Length, key);
        }

        public static Byte[] Encrypt(Byte[] In, Int32 offset, Int32 len, Byte[] key) {
            var temp = new Byte[In.Length];
            Buffer.BlockCopy(In, 0, temp, 0, In.Length);
            var random = new Random();
            var num = (len + 10) % 8;
            if (num != 0) {
                num = 8 - num;
            }

            var array = new Byte[len + num + 10];
            array[0] = (Byte)((random.Next() & 248) | num);
            for (var i = 1; i < num + 3; i++) {
                array[i] = (Byte)(random.Next() & 255);
            }

            Array.Copy(temp, 0, array, num + 3, len);
            for (var j = num + 3 + len; j < array.Length; j++) {
                array[j] = 0;
            }

            var array2 = new Byte[len + num + 10];
            for (var k = 0; k < array2.Length; k += 8) {
                Code(array, 0, k, array2, 0, k, key);
            }

            return array2;
        }

        private static UInt32[] FormatKey(Byte[] key) {
            if (key.Length == 0) {
                throw new ArgumentException("Key must be between 1 and 16 characters in length");
            }

            var array = new Byte[16];
            if (key.Length < 16) {
                Array.Copy(key, 0, array, 0, key.Length);
                for (var i = key.Length; i < 16; i++) {
                    array[i] = 32;
                }
            } else {
                Array.Copy(key, 0, array, 0, 16);
            }

            var array2 = new UInt32[4];
            var num = 0;
            for (var j = 0; j < array.Length; j += 4) {
                array2[num++] = ConvertByteArrayToUInt(array, j);
            }

            return array2;
        }

        private static Byte[] ConvertUIntToByteArray(UInt32 v) {
            return new[]
            {
                (Byte) ((v >> 24) & 255u),
                (Byte) ((v >> 16) & 255u),
                (Byte) ((v >> 8) & 255u),
                (Byte) (v & 255u)
            };
        }

        private static UInt32 ConvertByteArrayToUInt(Byte[] v, Int32 offset) {
            if (offset + 4 > v.Length) {
                return 0u;
            }

            var num = (UInt32)v[offset] << 24;
            num |= (UInt32)v[offset + 1] << 16;
            num |= (UInt32)v[offset + 2] << 8;
            return num | v[offset + 3];
        }


        /// <summary>
        ///     这是个随机因子产生器，用来填充头部的，如果为了调试，可以用一个固定值
        ///     随机因子可以使相同的明文每次加密出来的密文都不一样
        /// </summary>
        /// <returns>随机因子</returns>
        private static Int32 Rand() {
            var random = new Random();
            return random.Next();
        }

        /// <summary>
        ///     MD5加密
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        public static Byte[] MD5(Byte[] data) {
            var md5Instance = System.Security.Cryptography.MD5.Create();
            return md5Instance.ComputeHash(data);
        }

        /// <summary>
        ///     MD5加密
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static String Md5(String text) {
            MD5 md5 = new MD5CryptoServiceProvider();
            var buffer = md5.ComputeHash(Encoding.UTF8.GetBytes(text));
            var result = "";
            foreach (var b in buffer) {
                result += b.ToString("x2");
            }

            return result;
        }
    }
}

