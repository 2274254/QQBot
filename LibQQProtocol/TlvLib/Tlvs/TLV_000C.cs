using LibQQProtocol.Structs;
using LibQQProtocol.Util;
using System;
using System.IO;

namespace LibQQProtocol.TlvLib.Tlvs {
    [TlvTag(TlvTags.PingRedirect)]
    internal class TLV000C : BaseTLV {
        public TLV000C() {
            this.Command = 0x000C;
            this.Name = "SSO2::TLV_PingRedirect_0xC";
            this.WSubVer = 0x0002;
        }

        public Byte[] Get_Tlv(QQAccount user) {
            var data = new BinaryWriter(new MemoryStream());
            if (this.WSubVer == 0x0002) {
                data.BeWrite(this.WSubVer); //wSubVer
                data.BeWrite((UInt16)0);
                data.BeWrite(user.TXProtocol.DwIdc);
                data.BeWrite(user.TXProtocol.DwIsp);
                data.Write(ByteHelper.IPStringToByteArray(user.TXProtocol.DwServerIP));
                data.BeWrite(user.TXProtocol.WServerPort);
                data.BeWrite(0);
            } else {
                throw new Exception($"{this.Name} 无法识别的版本号 {this.WSubVer}");
            }

            this.FillHead(this.Command);
            this.FillBody(data.ToByteArray(), data.BaseStream.Length);
            this.SetLength();
            return this.GetBuffer();
        }

        public void Parser_Tlv(QQAccount user, BinaryReader buf) {
            var type = buf.BeReadUInt16(); //type
            var length = buf.BeReadUInt16(); //length
            this.WSubVer = buf.BeReadUInt16(); //wSubVer
            if (this.WSubVer == 0x0002) {
                buf.BeReadUInt16();
                user.TXProtocol.DwIdc = buf.BeReadUInt32(); /*dwIDC =*/
                user.TXProtocol.DwIsp = buf.BeReadUInt32(); /*dwISP =*/
                user.TXProtocol.DwRedirectIP = ByteHelper.GetIpStringFromBytes(buf.ReadBytes(4)); /*dwRedirectIP =*/
                user.TXProtocol.WRedirectPort = buf.BeReadUInt16(); /*wRedirectPort =*/
            } else {
                throw new Exception($"{this.Name} 无法识别的版本号 {this.WSubVer}");
            }
        }
    }
}