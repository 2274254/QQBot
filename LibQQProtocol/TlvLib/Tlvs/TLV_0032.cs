using LibQQProtocol.Structs;
using LibQQProtocol.Util;

namespace LibQQProtocol.TlvLib.Tlvs {
    [TlvTag(TlvTags.QdData)]
    internal class TLV0032 : BaseTLV {
        public TLV0032() {
            this.Command = 0x0032;
            this.Name = "TLV_QdData";
            this.WSubVer = 0x0002;
        }

        public System.Byte[] Get_Tlv(QQAccount user) {
            var qddata = QdData.GetQdData(user);
            this.FillHead(this.Command);
            this.FillBody(qddata, qddata.Length);
            this.SetLength();
            return this.GetBuffer();
        }
    }
}