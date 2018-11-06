using LibQQProtocol.Structs;
using LibQQProtocol.Util;
using System;
using System.IO;

namespace LibQQProtocol.TlvLib.Tlvs {
    [TlvTag(TlvTags.Ping)]
    internal class TLV0018 : BaseTLV {
        public TLV0018() {
            this.Command = 0x0018;
            this.Name = "SSO2::TLV_Ping_0x18";
            this.WSubVer = 0x0001;
        }

        /// <summary>
        ///     Ping
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public Byte[] Get_Tlv(QQAccount user) {
            var data = new BinaryWriter(new MemoryStream());
            if (this.WSubVer == 0x0001) {
                data.BeWrite(this.WSubVer); //wSubVer 
                data.BeWrite(user.TXProtocol.DwSsoVersion); //dwSSOVersion
                data.BeWrite(user.TXProtocol.DwServiceId); //dwServiceId
                data.BeWrite(user.TXProtocol.DwClientVer); //dwClientVer
                data.BeWrite(user.QQ); //dwUin
                data.BeWrite(user.TXProtocol.WRedirectCount); //wRedirectCount 
                data.BeWrite((UInt16)0); //NullBuf
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