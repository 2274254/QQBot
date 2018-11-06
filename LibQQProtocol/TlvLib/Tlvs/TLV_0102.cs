using LibQQProtocol.Structs;
using LibQQProtocol.Util;
using System;
using System.IO;

namespace LibQQProtocol.TlvLib.Tlvs {
    [TlvTag(TlvTags.Official)]
    internal class TLV0102 : BaseTLV {
        public TLV0102() {
            this.Command = 0x0102;
            this.Name = "SSO2::TLV_Official_0x102";
            this.WSubVer = 0x0001;
        }

        public Byte[] Get_Tlv(QQAccount user) {
            var data = new BinaryWriter(new MemoryStream());
            if (this.WSubVer == 0x0001) {
                data.BeWrite(this.WSubVer);
                //OfficialKey
                data.Write(new Byte[]
                    {0x9e, 0x9b, 0x03, 0x23, 0x6d, 0x7f, 0xa8, 0x81, 0xa8, 0x10, 0x72, 0xec, 0x50, 0x97, 0x96, 0x8e});
                var bufSigPic = user.TXProtocol.BufSigPic ?? ByteHelper.RandomKey(56);
                data.WriteKey(bufSigPic);
                //Official
                data.WriteKey(new Byte[]
                {
                    0x60, 0x6f, 0x27, 0xd7, 0xdc, 0x40, 0x46, 0x33, 0xa6, 0xc4, 0xb9, 0x05, 0x7e, 0x60, 0xfb, 0x64,
                    0x1e, 0x75, 0x65, 0x6
                });
            } else {
                throw new Exception($"{this.Name} 无法识别的版本号 {this.WSubVer}");
            }

            this.FillHead(this.Command);
            this.FillBody(data.ToByteArray(), data.BaseStream.Length);
            this.SetLength();
            return this.GetBuffer();
        }
    }
}