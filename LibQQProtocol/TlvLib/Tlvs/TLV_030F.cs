using LibQQProtocol.Structs;
using LibQQProtocol.Util;
using System.IO;
using System.Text;

namespace LibQQProtocol.TlvLib.Tlvs {
    [TlvTag(TlvTags.ComputerName)]
    internal class TLV030F : BaseTLV {
        public TLV030F() {
            this.Command = 0x030F;
            this.Name = "SSO2::TLV_ComputerName";
        }

        public System.Byte[] Get_Tlv(QQAccount user) {
            var data = new BinaryWriter(new MemoryStream());
            data.BeWrite((System.UInt16)Encoding.UTF8.GetBytes(user.TXProtocol.BufComputerName).Length);
            data.Write(Encoding.UTF8.GetBytes(user.TXProtocol.BufComputerName));
            this.FillHead(this.Command);
            this.FillBody(data.ToByteArray(), data.BaseStream.Length);
            this.SetLength();
            return this.GetBuffer();
        }
    }
}