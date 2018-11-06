using LibQQProtocol.Structs;
using LibQQProtocol.Util;

namespace LibQQProtocol.TlvLib.Tlvs {
    [TlvTag(TlvTags.GTKeyTGTGTCryptedData)]
    internal class TLV001A : BaseTLV {
        public TLV001A() {
            this.Command = 0x001A;
            this.Name = "SSO2::TLV_GTKeyTGTGTCryptedData_0x1a";
        }

        public System.Byte[] Get_Tlv(QQAccount user) {
            var data = new TLV0015().Get_Tlv(user);

            var encode = QQTea.Encrypt(data, user.TXProtocol.BufTgtgtKey);

            this.FillHead(this.Command);
            this.FillBody(encode, encode.Length);
            this.SetLength();
            return this.GetBuffer();
        }
    }
}