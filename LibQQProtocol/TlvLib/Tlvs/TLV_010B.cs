using LibQQProtocol.Defines;
using LibQQProtocol.Structs;
using LibQQProtocol.Util;
using System;
using System.IO;

namespace LibQQProtocol.TlvLib.Tlvs {
    [TlvTag(TlvTags.QDLoginFlag)]
    internal class TLV010B : BaseTLV {
        public TLV010B() {
            this.Command = 0x010B;
            this.Name = "TLV_QDLoginFlag";
            this.WSubVer = 0x0002;
        }

        public Byte[] Get_Tlv(QQAccount user) {
            var data = new BinaryWriter(new MemoryStream());
            if (this.WSubVer == 0x0002) {
                data.BeWrite(this.WSubVer); //wSubVer
                var newbyte = user.TXProtocol.BufTgt;
                var flag = this.EncodeLoginFlag(newbyte, ClientDefines.QqexeMD5);
                data.Write(user.MD51);
                data.Write(flag);
                data.Write((Byte)0x10);
                data.BeWrite(0);
                data.BeWrite(2);
                var qddata = QdData.GetQdData(user);
                data.WriteKey(qddata);
                data.BeWrite(0);
            } else {
                throw new Exception($"{this.Name} 无法识别的版本号 {this.WSubVer}");
            }

            this.FillHead(this.Command);
            this.FillBody(data.ToByteArray(), data.BaseStream.Length);
            this.SetLength();
            return this.GetBuffer();
        }

        private Byte EncodeLoginFlag(Byte[] bufTgt /*bufTGT*/, Byte[] qqexeMD5 /*QQEXE_MD5*/,
            Byte flag = 0x01 /*固定 0x01*/) {
            var rc = flag;
            foreach (var t in bufTgt) {
                rc ^= t;
            }

            for (var i = 0; i < 4; i++) {
                var rcc = qqexeMD5[i * 4];
                rcc ^= qqexeMD5[i * 4 + 1];
                rcc ^= qqexeMD5[i * 4 + 3];
                rcc ^= qqexeMD5[i * 4 + 2];
                rc ^= rcc;
            }

            return rc;
        }
    }
}