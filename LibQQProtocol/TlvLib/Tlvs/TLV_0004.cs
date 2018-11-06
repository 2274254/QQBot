using LibQQProtocol.Structs;
using LibQQProtocol.Util;
using System;
using System.IO;

namespace LibQQProtocol.TlvLib.Tlvs {
    [TlvTag(TlvTags.NonUinAccount)]
    internal class TLV0004 : BaseTLV {
        public TLV0004() {
            this.Command = 0x0004;
            this.Name = "SSO2::TLV_NonUinAccount_0x4";
            this.WSubVer = 0x0000;
        }

        public Byte[] Get_Tlv(QQAccount user) {
            if (user.QQ != 0) {
                return null;
            }

            var data = new BinaryWriter(new MemoryStream());
            if (this.WSubVer == 0x0000) {
                data.BeWrite(this.WSubVer); //wSubVer 
                var bufAccount = ByteHelper.HexStringToByteArray(ByteHelper.NumToHexString(user.QQ));
                data.BeWrite((UInt16)bufAccount.Length); //账号长度
                data.Write(bufAccount); //账号
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