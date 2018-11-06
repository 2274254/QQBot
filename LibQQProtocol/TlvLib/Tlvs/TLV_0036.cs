using LibQQProtocol.Structs;
using LibQQProtocol.Util;
using System;
using System.IO;

namespace LibQQProtocol.TlvLib.Tlvs {
    [TlvTag(TlvTags.LoginReason)]
    internal class TLV0036 : BaseTLV {
        public TLV0036() {
            this.Command = 0x0036;
            this.Name = "SSO2::TLV_LoginReason_0x36";
            this.WSubVer = 0x0002;
        }

        /// <summary>
        ///     LoginReason
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public Byte[] Get_Tlv(QQAccount user) {
            var data = new BinaryWriter(new MemoryStream());
            if (this.WSubVer == 0x0002) {
                data.BeWrite(this.WSubVer); //wSubVer
                data.BeUshortWrite(1);
                data.BeUshortWrite(0);
                data.BeUshortWrite(1);
                data.BeUshortWrite(0);
                data.BeUshortWrite(0);
                data.BeUshortWrite(0);
                data.BeUshortWrite(0);
                data.BeUshortWrite(0);
            } else if (this.WSubVer == 0x0001) {
                data.BeWrite(this.WSubVer); //wSubVer
                data.BeUshortWrite(1);
                data.BeUshortWrite(0);
                data.BeUshortWrite(0);
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