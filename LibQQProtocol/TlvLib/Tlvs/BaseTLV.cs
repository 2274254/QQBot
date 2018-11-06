using LibQQProtocol.Util;
using System.IO;

namespace LibQQProtocol.TlvLib.Tlvs {
    internal class BaseTLV {
        ///private readonly System.Int64 _max = 128;

        /// <summary>
        ///     包长度
        /// </summary>
        //internal System.Int64 BodyLength;

        protected readonly BinaryWriter _buffer;

        /// <summary>
        ///     tlv命令
        /// </summary>
        internal System.UInt16 Command;

        /// <summary>
        ///     包头长度
        /// </summary>
        internal System.Int32 HeadLength = 4;

        public BaseTLV() {
            this._buffer = new BinaryWriter(new MemoryStream());
        }

        /// <summary>
        ///     TLV名称
        /// </summary>
        public System.String Name { get; set; }

        /// <summary>
        ///     TLV版本
        /// </summary>
        public System.UInt16 WSubVer { get; set; }

        protected void FillBody(System.Byte[] bufdata, System.Int64 length) {
            this._buffer.BeWrite((System.UInt16)length);
            this._buffer.Write(bufdata);
        }

        protected void FillHead(System.UInt16 cmd) {
            this._buffer.BeWrite(cmd);
        }

        protected System.Byte[] GetBuffer() {
            return this._buffer.ToByteArray();
        }


        protected void SetLength() {
        }
    }
}