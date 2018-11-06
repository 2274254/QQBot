using LibQQProtocol.Structs;
using LibQQProtocol.Util;
using System.IO;

namespace LibQQProtocol.TlvLib.Tlvs {
    [TlvTag(TlvTags.SigIP2)]
    internal class TLV0112 : BaseTLV {
        public TLV0112() {
            this.Command = 0x0112;
            this.Name = "SSO2::TLV_SigIP2_0x112";
        }

        public System.Byte[] Get_Tlv(QQAccount user) {
            var data = new BinaryWriter(new MemoryStream());
            data.Write(user.TXProtocol.BufSigClientAddr);
            this.FillHead(this.Command);
            this.FillBody(data.ToByteArray(), data.BaseStream.Length);
            this.SetLength();
            return this.GetBuffer();
        }

        public void Parser_Tlv(QQAccount user, BinaryReader buf) {
            var type = buf.BeReadUInt16(); //type
            var length = buf.BeReadUInt16(); //length
            user.TXProtocol.BufSigClientAddr = buf.ReadBytes(length); //bufSigClientAddr
        }
    }
}