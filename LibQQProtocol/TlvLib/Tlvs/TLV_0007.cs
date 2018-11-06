using LibQQProtocol.Structs;
using LibQQProtocol.Util;
using System.IO;

namespace LibQQProtocol.TlvLib.Tlvs {
    [TlvTag(TlvTags.TGT)]
    internal class TLV0007 : BaseTLV {
        public TLV0007() {
            this.Command = 0x0007;
            this.Name = "TLV_TGT";
        }

        public System.Byte[] Get_Tlv(QQAccount user) {
            var bufTgt = user.TXProtocol.BufTgt;
            var buf = new BinaryWriter(new MemoryStream());
            buf.Write(bufTgt);
            this.FillHead(this.Command);
            FillBody(buf.ToByteArray(), buf.BaseStream.Length);
            this.SetLength();
            return this.GetBuffer();
        }
    }
}