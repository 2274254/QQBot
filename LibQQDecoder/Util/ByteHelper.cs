using System;

namespace LibQQDecoder.Util {
    public static class ByteHelper {
        public static Byte[] MergeByteArray(params Byte[][] arrays) {
            var newArraylength = 0L;

            foreach (var array in arrays) {
                newArraylength += array.LongLength;
            }

            var newArray = new Byte[newArraylength];
            var copiedArraylength = 0L;

            foreach (var array in arrays) {
                Array.Copy(array, 0, newArray, copiedArraylength, array.LongLength);
                copiedArraylength += array.LongLength;
            }

            return newArray;
        }

        public static Byte[] GetRandomBytes(Int64 length) {
            var random = new Random();
            var result = new Byte[length];
            random.NextBytes(result);
            return result;
        }

        public static Byte[] GetHexQQnumber(UInt32 QQNumber) {
            var HexQQnumber = BitConverter.GetBytes(QQNumber);
            Array.Reverse(HexQQnumber);
            return HexQQnumber;
        }

        public static Boolean ArrCmp(Byte[] Arr1, Byte[] Arr2, UInt32 Arr1StartPos, UInt32 Arr2StartPos, Int32 N) {
            for (UInt32 i = 0; i < N; i++) {
                if (Arr1[Arr1StartPos + i] != Arr2[Arr2StartPos + i]) {
                    return false;
                }
            }
            return true;
        }
    }
}
