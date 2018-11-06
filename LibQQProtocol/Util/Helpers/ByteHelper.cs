using LibQQProtocol.Defines;
using System;
using System.Text;

namespace LibQQProtocol.Util {
    public class ByteHelper {
        private static readonly DateTime BaseDateTime = DateTime.Parse("1970-1-01 00:00:00.000");

        public static Byte[] RandomKey() {
            var key = new Byte[PacketDefines.QQLengthKey];
            new Random().NextBytes(key);
            return key;
        }

        public static Byte[] RandomKey(Int32 length) {
            var key = new Byte[length];
            new Random().NextBytes(key);
            return key;
        }

        public static Byte[] GetBytes(String s) {
            return Encoding.Default.GetBytes(s);
        }

        public static Int64 GetTimeMillis(DateTime dateTime) {
            return (Int64)(dateTime - BaseDateTime).TotalMilliseconds;
        }

        public static Int64 GetTimeSeconds(DateTime dateTime) {
            return (Int64)(dateTime - BaseDateTime).TotalSeconds;
        }

        public static String NumToHexString(Int64 qq, Int32 length = 8) {
            var text = Convert.ToString(qq, 16);
            if (text.Length == length) {
                return text;
            }

            if (text.Length > length) {
                return null;
            }

            var num = length - text.Length;
            var str = "";
            for (var i = 0; i < num; i++) {
                str += "0";
            }

            text = (str + text).ToUpper();
            var stringBuilder = new StringBuilder();
            for (var j = 0; j < text.Length; j++) {
                stringBuilder.Append(text[j]);
                if ((j + 1) % 2 == 0) {
                    stringBuilder.Append(" ");
                }
            }

            return stringBuilder.ToString();
        }

        public static Byte[] HexStringToByteArray(String hexString) {
            hexString = hexString.Replace(" ", "").Replace("\n", "");
            if (hexString.Length % 2 != 0) {
                hexString += " ";
            }

            var array = new Byte[hexString.Length / 2];
            for (var i = 0; i < array.Length; i++) {
                array[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            }

            return array;
        }

        public static Byte[] IPStringToByteArray(String ip) {
            var array = new Byte[4];
            var array2 = ip.Split('.');
            if (array2.Length == 4) {
                for (var i = 0; i < 4; i++) {
                    array[i] = (Byte)Int32.Parse(array2[i]);
                }
            }

            return array;
        }

        public static String GetIpStringFromBytes(Byte[] ip) {
            return $"{ip[0]}.{ip[1]}.{ip[2]}.{ip[3]}";
        }

        public static DateTime GetDateTimeFromMillis(Int64 millis) {
            return BaseDateTime.AddTicks(millis * TimeSpan.TicksPerSecond).AddHours(8);
        }

        public static long CurrentTimeMillis() {
            return GetTimeMillis(DateTime.Now);
        }
    }
}
