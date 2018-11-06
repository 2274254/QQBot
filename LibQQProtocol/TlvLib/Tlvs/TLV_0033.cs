using LibQQProtocol.Structs;
using LibQQProtocol.Util;
using System.IO;

namespace LibQQProtocol.TlvLib.Tlvs {
    [TlvTag(TlvTags._0x0033)]
    internal class TLV0033 : BaseTLV {
        public TLV0033() {
            this.Command = 0x0033;
            this.Name = "SSO2::TLV_LoginReason_0x33";
            this.WSubVer = 0x0002;
        }

        public void Parser_Tlv(QQAccount user, BinaryReader buf) {
            var type = buf.BeReadUInt16(); //type
            var length = buf.BeReadUInt16(); //length
            buf.ReadBytes(length);
        }
    }
}