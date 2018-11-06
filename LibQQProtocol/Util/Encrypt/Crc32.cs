using System;
using System.Text;

namespace LibQQProtocol.Util {
    public class CRC32 {
        private static UInt16[] _crc16Table;

        private static UInt32[] _crc32Table;

        protected static UInt64[] _crc32Table2;

        private static void MakeCRC16Table() {
            if (_crc16Table != null) {
                return;
            }

            _crc16Table = new UInt16[256];
            for (UInt16 num = 0; num < 256; num += 1) {
                var num2 = num;
                for (var i = 0; i < 8; i++) {
                    if (num2 % 2 == 0) {
                        num2 = (UInt16)(num2 >> 1);
                    } else {
                        num2 = (UInt16)((num2 >> 1) ^ 33800);
                    }
                }

                _crc16Table[num] = num2;
            }
        }

        private static void MakeCRC32Table() {
            if (_crc32Table != null) {
                return;
            }

            _crc32Table = new UInt32[256];
            for (var num = 0u; num < 256u; num += 1u) {
                var num2 = num;
                for (var i = 0; i < 8; i++) {
                    if (num2 % 2u == 0u) {
                        num2 >>= 1;
                    } else {
                        num2 = (num2 >> 1) ^ 3988292384u;
                    }
                }

                _crc32Table[(Int32)(UIntPtr)num] = num2;
            }
        }

        private static UInt16 UpdateCRC16(Byte aByte, UInt16 aSeed) {
            return (UInt16)(_crc16Table[(aSeed & 255) ^ aByte] ^ (aSeed >> 8));
        }

        private static UInt32 UpdateCRC32(Byte aByte, UInt32 aSeed) {
            return _crc32Table[(Int32)(UIntPtr)((aSeed & 255u) ^ aByte)] ^ (aSeed >> 8);
        }

        private static UInt16 CRC16(Byte[] aBytes) {
            MakeCRC16Table();
            UInt16 num = 65535;
            foreach (var aByte in aBytes) {
                num = UpdateCRC16(aByte, num);
            }

            return num;
        }

        private static UInt16 CRC16(String aString, Encoding aEncoding) {
            return CRC16(aEncoding.GetBytes(aString));
        }

        private static UInt16 CRC16(String aString) {
            return CRC16(aString, Encoding.UTF8);
        }

        public static UInt32 CRC32Imp(Byte[] aBytes) {
            MakeCRC32Table();
            var num = 4294967295u;
            foreach (var aByte in aBytes) {
                num = UpdateCRC32(aByte, num);
            }

            return CRC32ToUint(~num);
        }

        public static UInt32 CRC32Reverse(Byte[] aBytes) {
            MakeCRC32Table();
            var num = 4294967295u;
            foreach (var aByte in aBytes) {
                num = UpdateCRC32(aByte, num);
            }

            return CRC32ToUintReverse(~num);
        }

        //生成CRC32码表
        public static void GetCRC32Table() {
            _crc32Table2 = new UInt64[256];
            Int32 i;
            for (i = 0; i < 256; i++) {
                var crc = (UInt64)i;
                Int32 j;
                for (j = 8; j > 0; j--) {
                    if ((crc & 1) == 1) {
                        crc = (crc >> 1) ^ 0xEDB88320;
                    } else {
                        crc >>= 1;
                    }
                }

                _crc32Table2[i] = crc;
            }
        }

        //获取字符串的CRC32校验值
        public static UInt64 GetCRC32Str(String sInputString) {
            //生成码表
            GetCRC32Table();
            var buffer = Encoding.ASCII.GetBytes(sInputString);
            UInt64 value = 0xffffffff;
            var len = buffer.Length;
            for (var i = 0; i < len; i++) {
                value = (value >> 8) ^ _crc32Table2[(value & 0xFF) ^ buffer[i]];
            }

            return value ^ 0xffffffff;
        }

        public static UInt64 GetCRC32(Byte[] buffer) {
            //生成码表
            GetCRC32Table();
            UInt64 value = 0xffffffff;
            var len = buffer.Length;
            for (var i = 0; i < len; i++) {
                value = (value >> 8) ^ _crc32Table2[(value & 0xFF) ^ buffer[i]];
            }

            return value ^ 0xffffffff;
        }

        private static UInt32 CRC32Imp(String aString, Encoding aEncoding) {
            return CRC32Imp(aEncoding.GetBytes(aString));
        }

        private static UInt32 CRC32Imp(String aString) {
            return CRC32Imp(aString, Encoding.UTF8);
        }

        private static UInt32 CRC32ToUint(UInt32 crc32) {
            var text = crc32.ToString("X2");
            if (text.Length == 7) {
                text = "0" + text;
            }

            var text2 = "";
            for (var i = 6; i >= 0; i -= 2) {
                text2 = text2 + text.Substring(i, 2) + " ";
            }

            UInt32 result;
            try {
                result = Convert.ToUInt32(text2.Replace(" ", ""), 16);
            } catch {
                result = 0u;
            }

            return result;
        }

        private static UInt32 CRC32ToUintReverse(UInt32 crc32) {
            var text = crc32.ToString("X2");
            var text2 = "";
            for (var i = 6; i >= 0; i -= 2) {
                text2 = text.Substring(i, 2) + " " + text2;
            }

            UInt32 result;
            try {
                result = Convert.ToUInt32(text2.Replace(" ", ""), 16);
            } catch {
                result = 0u;
            }

            return result;
        }
    }
}
