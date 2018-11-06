using LibQQProtocol.Structs;
using LibQQProtocol.Util;
using System;
using System.IO;

namespace LibQQProtocol.TlvLib.Tlvs {
    [TlvTag(TlvTags.Ping_Strategy)]
    internal class TLV0309 : BaseTLV {
        public TLV0309() {
            this.Command = 0x0309;
            this.Name = "SSO2::TLV_Ping_Strategy_0x309";
            this.WSubVer = 0x0001;
        }

        public Byte[] Get_Tlv(QQAccount user) {
            var data = new BinaryWriter(new MemoryStream());
            if (this.WSubVer == 0x0001) {
                data.BeWrite(this.WSubVer); //wSubVer
                data.Write(ByteHelper.IPStringToByteArray(user.TXProtocol.DwServerIP)); //LastServerIP - 服务器最后的登录IP，可以为0
                data.Write((Byte)user.TXProtocol.RedirectIP.Count); //cRedirectCount - 重定向的次数（IP的数量）
                foreach (var ip in user.TXProtocol.RedirectIP) {
                    data.Write(ip);
                }

                data.Write(user.TXProtocol.CPingType); //cPingType 
            } else {
                throw new Exception($"{this.Name} 无法识别的版本号 {this.WSubVer}");
            }

            this.FillHead(this.Command);
            this.FillBody(data.ToByteArray(), data.BaseStream.Length);
            this.SetLength();
            return this.GetBuffer();
        }


        public Byte GetPingType(Int32 val) {
            switch (val) {
                case 10: //0xA
                case 20: //0x14
                    return 1;
                case 30: //0x1E
                    return 2;
                case 40: //0x28
                    return 3;
                case 50: //0x32
                case 60: //0x3C
                    return 4;
                case 70: //0x46
                    return 6;
                case 25: //0x19
                    return 7;
                default:
                    return 4;
            }
        }
    }
}