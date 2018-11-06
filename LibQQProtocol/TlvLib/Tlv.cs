using LibQQProtocol.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QQ.Framework.TlvLib {
    /// <summary>
    ///     TLV data.
    /// </summary>
    public class Tlv {
        private readonly Int32 _valueOffset;

        private Tlv(Int32 tag, Int32 length, Int32 valueOffset, Byte[] data) {
            this.Tag = tag;
            this.Length = length;
            this.Data = data;
            this.Children = new List<Tlv>();

            this._valueOffset = valueOffset;
        }

        /// <summary>
        ///     The raw TLV data.
        /// </summary>
        public Byte[] Data { get; }

        /// <summary>
        ///     The raw TLV data.
        /// </summary>
        public String HexData => GetHexString(this.Data);

        /// <summary>
        ///     The TLV tag.
        /// </summary>
        public Int32 Tag { get; }

        /// <summary>
        ///     The TLV tag.
        /// </summary>
        public String HexTag => ByteHelper.NumToHexString(this.Tag, 4);

        /// <summary>
        ///     The length of the TLV value.
        /// </summary>
        public Int32 Length { get; }

        /// <summary>
        ///     The length of the TLV value.
        /// </summary>
        public String HexLength => ByteHelper.NumToHexString(this.Length, 4);

        /// <summary>
        ///     The TLV value.
        /// </summary>
        public Byte[] Value {
            get {
                var result = new Byte[this.Length];
                Array.Copy(this.Data, this._valueOffset, result, 0, this.Length);
                return result;
            }
        }

        /// <summary>
        ///     The TLV value.
        /// </summary>
        public String HexValue => GetHexString(this.Value);

        /// <summary>
        ///     TLV children.
        /// </summary>
        public ICollection<Tlv> Children { get; set; }

        /// <summary>
        ///     Parse TLV data.
        /// </summary>
        /// <param name="tlv">The hex TLV blob.</param>
        /// <returns>A collection of TLVs.</returns>
        public static ICollection<Tlv> ParseTlv(String tlv) {
            if (String.IsNullOrWhiteSpace(tlv)) {
                throw new ArgumentException("tlv");
            }

            return ParseTlv(GetBytes(tlv));
        }

        /// <summary>
        ///     Parse TLV data.
        /// </summary>
        /// <param name="tlv">The byte array TLV blob.</param>
        /// <returns>A collection of TLVs.</returns>
        public static ICollection<Tlv> ParseTlv(Byte[] tlv) {
            if (tlv == null || tlv.Length == 0) {
                throw new ArgumentException("tlv");
            }

            var result = new List<Tlv>();
            ParseTlv(tlv, result);

            return result;
        }

        private static void ParseTlv(Byte[] rawTlv, ICollection<Tlv> result) {
            for (Int32 i = 0, start = 0; i < rawTlv.Length; start = i) {
                var constructedTlv = (rawTlv[i] & 0x20) != 0;
                var moreBytes = (rawTlv[i] & 0x1F) == 0x1F;
                while (moreBytes && (rawTlv[++i] & 0x80) != 0) {
                    ;
                }

                i += 2;

                var tag = GetInt(rawTlv, start, i - start);

                i += 2;
                start += 2;
                var length = GetInt(rawTlv, start, i - start);

                i += length;

                var rawData = new Byte[i - start];
                Array.Copy(rawTlv, start, rawData, 0, i - start);
                var tlv = new Tlv(tag, length, rawData.Length - length, rawData);
                result.Add(tlv);

                if (constructedTlv) {
                    ParseTlv(tlv.Value, tlv.Children);
                }
            }
        }

        private static String GetHexString(Byte[] arr) {
            var sb = new StringBuilder(arr.Length * 2);
            foreach (var b in arr) {
                sb.AppendFormat("{0:X2}", b);
            }

            return sb.ToString();
        }

        private static Byte[] GetBytes(String hexString) {
            return Enumerable
                .Range(0, hexString.Length)
                .Where(x => x % 2 == 0)
                .Select(x => Convert.ToByte(hexString.Substring(x, 2), 16))
                .ToArray();
        }

        private static Int32 GetInt(Byte[] data, Int32 offset, Int32 length) {
            var result = 0;
            for (var i = 0; i < length; i++) {
                result = (result << 8) | data[offset + i];
            }

            return result;
        }
    }
}