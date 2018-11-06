using LibQQProtocol.Structs;
using LibQQProtocol.Util;
using System.IO;

namespace LibQQProtocol.TlvLib.Tlvs {
    [TlvTag(TlvTags.GTKey_TGTGT)]
    internal class TLV001E : BaseTLV {
        public TLV001E() {
            this.Command = 0x001E;
            this.Name = "SSO2::TLV_GTKey_TGTGT_0x1e";
        }

        public void Parser_Tlv(QQAccount user, BinaryReader buf) {
            var type = buf.BeReadUInt16(); //type
            var length = buf.BeReadUInt16(); //length
            user.TXProtocol.BufTgtgtKey = buf.ReadBytes(length);
        }
    }
}