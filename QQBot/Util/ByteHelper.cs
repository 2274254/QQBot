using System;

namespace QQBot.Util {
    public static class ByteHelper {
        public static byte[] MergeByteArray(params byte[][] arrays) {
            var newArraylength = 0L;

            foreach (var array in arrays) {
                newArraylength += array.LongLength;
            }

            var newArray = new byte[newArraylength];
            var copiedArraylength = 0L;

            foreach (var array in arrays) {
                Array.Copy(array, 0, newArray, copiedArraylength, array.LongLength);
                copiedArraylength += array.LongLength;
            }

            return newArray;
        }

        public static byte[] GetRandomBytes(long length) {
            Random random = new Random();
            var result = new byte[length];
            random.NextBytes(result);
            return result;
        }

        public static byte[] GetHexQQnumber(UInt32 QQNumber) {
            var HexQQnumber = BitConverter.GetBytes(QQNumber);
            Array.Reverse(HexQQnumber);
            return HexQQnumber;
        }
    }
}
