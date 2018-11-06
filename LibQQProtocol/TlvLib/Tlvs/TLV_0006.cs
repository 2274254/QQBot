using LibQQProtocol.Structs;
using LibQQProtocol.Util;
using System;
using System.IO;

namespace LibQQProtocol.TlvLib.Tlvs {
    [TlvTag(TlvTags.TGTGT)]
    internal class TLV0006 : BaseTLV {
        public TLV0006() {
            this.Command = 0x0006;
            this.Name = "SSO2::TLV_TGTGT_0x6";
            this.WSubVer = 0x0002;
        }

        public Byte[] Get_Tlv(QQAccount user) {
            if (this.WSubVer == 0x0002) {
                if (user.TXProtocol.BufTgtgt == null) {
                    var data = new BinaryWriter(new MemoryStream());
                    data.BeWrite(new Random(Guid.NewGuid().GetHashCode()).Next()); //随机4字节??
                    data.BeWrite(this.WSubVer); //wSubVer
                    data.BeWrite(user.QQ); //QQ号码
                    data.BeWrite(user.TXProtocol.DwSsoVersion);
                    data.BeWrite(user.TXProtocol.DwServiceId);
                    data.BeWrite(user.TXProtocol.DwClientVer);
                    data.BeWrite((UInt16)0);
                    data.Write(user.TXProtocol.BRememberPwdLogin);
                    data.Write(user.MD51); //密码的一次MD5值，服务器用该MD5值验证用户密码是否正确
                    data.BeWrite(user.TXProtocol.DwServerTime); //登录时间
                    data.Write(new Byte[13]); //固定13字节
                    data.Write(ByteHelper.IPStringToByteArray(user.TXProtocol.DwClientIP)); //IP地址
                    data.BeWrite(user.TXProtocol.DwIsp); //dwISP
                    data.BeWrite(user.TXProtocol.DwIdc); //dwIDC
                    data.WriteKey(user.TXProtocol.BufComputerIdEx); //机器码
                    data.Write(user.TXProtocol.BufTgtgtKey); //00DD临时密钥(通过验证时客户端用该密钥解密服务端发送回来的数据)

                    user.TXProtocol.BufTgtgt = QQTea.Encrypt(data.ToByteArray(), user.Md52());
                }
            } else {
                throw new Exception($"{this.Name} 无法识别的版本号 {this.WSubVer}");
            }

            var tlv = new BinaryWriter(new MemoryStream());
            tlv.Write(user.TXProtocol.BufTgtgt);
            this.FillHead(this.Command);
            this.FillBody(tlv.ToByteArray(), tlv.BaseStream.Length);
            this.SetLength();
            return this.GetBuffer();
        }

        public void Parser_Tlv(QQAccount user, BinaryReader buf) {
            var type = buf.BeReadUInt16(); //type
            var length = buf.BeReadUInt16(); //length
            user.TXProtocol.BufTgtgt =
                buf.ReadBytes(length);
        }
    }
}