using System;

/*

 Copied from https://www.cnblogs.com/yeye518/p/3368406.html

*/

namespace LibQQDecoder.Util {
    public static class QQTea {
        private static void TeaEncipher(Byte[] In, Int32 inOffset, Int32 inPos, Byte[] Out, Int32 outOffset, Int32 outPos, Byte[] key) {
            if (outPos > 0) {
                for (var i = 0; i < 8; i++) {
                    In[outOffset + outPos + i] = (Byte)(In[inOffset + inPos + i] ^ Out[outOffset + outPos + i - 8]);
                }
            }
            var formattedKey = FormatKey(key);
            var y = ConvertByteArrayToUInt(In, outOffset + outPos);
            var z = ConvertByteArrayToUInt(In, outOffset + outPos + 4);
            UInt32 sum = 0;
            var delta = 0x9e3779b9;
            UInt32 n = 16;

            while (n-- > 0) {
                sum += delta;
                y += ((z << 4) + formattedKey[0]) ^ (z + sum) ^ ((z >> 5) + formattedKey[1]);
                z += ((y << 4) + formattedKey[2]) ^ (y + sum) ^ ((y >> 5) + formattedKey[3]);
            }
            Array.Copy(ConvertUIntToByteArray(y), 0, Out, outOffset + outPos, 4);
            Array.Copy(ConvertUIntToByteArray(z), 0, Out, outOffset + outPos + 4, 4);
            if (inPos > 0) {
                for (var i = 0; i < 8; i++) {
                    Out[outOffset + outPos + i] = (Byte)(Out[outOffset + outPos + i] ^ In[inOffset + inPos + i - 8]);
                }
            }
        }

        private static void TeaDecipher(Byte[] In, Int32 inOffset, Int32 inPos, Byte[] Out, Int32 outOffset, Int32 outPos, Byte[] key) {
            if (outPos > 0) {
                for (var i = 0; i < 8; i++) {
                    Out[outOffset + outPos + i] = (Byte)(In[inOffset + inPos + i] ^ Out[outOffset + outPos + i - 8]);
                }
            } else {
                Array.Copy(In, inOffset, Out, outOffset, 8);
            }
            var formattedKey = FormatKey(key);
            var y = ConvertByteArrayToUInt(Out, outOffset + outPos);
            var z = ConvertByteArrayToUInt(Out, outOffset + outPos + 4);
            var sum = 0xE3779B90;
            var delta = 0x9e3779b9;
            UInt32 n = 16;

            while (n-- > 0) {
                z -= ((y << 4) + formattedKey[2]) ^ (y + sum) ^ ((y >> 5) + formattedKey[3]);
                y -= ((z << 4) + formattedKey[0]) ^ (z + sum) ^ ((z >> 5) + formattedKey[1]);
                sum -= delta;
            }
            Array.Copy(ConvertUIntToByteArray(y), 0, Out, outOffset + outPos, 4);
            Array.Copy(ConvertUIntToByteArray(z), 0, Out, outOffset + outPos + 4, 4);
        }

        public static Byte[] Decrypt(Byte[] In, Int32 offset, Int32 len, Byte[] key) {
            if ((len % 8 != 0) || (len < 16)) {
                return null;
            }
            var Out = new Byte[len];
            for (var i = 0; i < len; i += 8) {
                TeaDecipher(In, offset, i, Out, 0, i, key);
            }
            for (var i = 8; i < len; i++) {
                Out[i] = (Byte)(Out[i] ^ In[offset + i - 8]);
            }
            var pos = Out[0] & 0x07;
            len = len - pos - 10;
            var res = new Byte[len];
            Array.Copy(Out, pos + 3, res, 0, len);
            return res;
        }

        public static Byte[] Encrypt(Byte[] In, Int32 offset, Int32 len, Byte[] key) {
            var pos = (len + 10) % 8;
            if (pos != 0) {
                pos = 8 - pos;
            }
            var plain = new Byte[len + pos + 10];
            var Rnd = new Random();
            plain[0] = (Byte)((Rnd.Next() & 0xF8) | pos);
            for (var i = 1; i < pos + 3; i++) {
                plain[i] = (Byte)(Rnd.Next() & 0xFF);
            }
            Array.Copy(In, 0, plain, pos + 3, len);
            for (var i = pos + 3 + len; i < plain.Length; i++) {
                plain[i] = 0x0;
            }
            var outer = new Byte[len + pos + 10];
            for (var i = 0; i < outer.Length; i += 8) {
                TeaEncipher(plain, 0, i, outer, 0, i, key);
            }
            return outer;
        }

        private static UInt32[] FormatKey(Byte[] key) {
            if (key.Length == 0) {
                throw new ArgumentException("Key must be between 1 and 16 characters in length");
            }
            var refineKey = new Byte[16];
            if (key.Length < 16) {
                Array.Copy(key, 0, refineKey, 0, key.Length);
                for (var k = key.Length; k < 16; k++) {
                    refineKey[k] = 0x20;
                }
            } else {
                Array.Copy(key, 0, refineKey, 0, 16);
            }
            var formattedKey = new UInt32[4];
            var j = 0;
            for (var i = 0; i < refineKey.Length; i += 4) {
                formattedKey[j++] = ConvertByteArrayToUInt(refineKey, i);
            }
            return formattedKey;
        }

        private static Byte[] ConvertUIntToByteArray(UInt32 v) {
            var result = new Byte[4];
            result[0] = (Byte)((v >> 24) & 0xFF);
            result[1] = (Byte)((v >> 16) & 0xFF);
            result[2] = (Byte)((v >> 8) & 0xFF);
            result[3] = (Byte)((v >> 0) & 0xFF);
            return result;
        }

        private static UInt32 ConvertByteArrayToUInt(Byte[] v, Int32 offset) {
            if (offset + 4 > v.Length) {
                return 0;
            }
            UInt32 output;
            output = (UInt32)(v[offset] << 24);
            output |= (UInt32)(v[offset + 1] << 16);
            output |= (UInt32)(v[offset + 2] << 8);
            output |= (UInt32)(v[offset + 3] << 0);
            return output;
        }
    }
}
