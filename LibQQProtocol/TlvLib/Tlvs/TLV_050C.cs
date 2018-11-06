using LibQQProtocol.Structs;
using LibQQProtocol.Util;
using System;
using System.IO;

namespace LibQQProtocol.TlvLib.Tlvs {
    [TlvTag(TlvTags._0x050C)]
    internal class TLV050C : BaseTLV {
        public TLV050C() {
            this.Command = 0x050C;
            this.Name = "SSO2::TLV_050C";
        }

        public Byte[] Get_Tlv(QQAccount user) {
            var Buf = new BinaryWriter(new MemoryStream());
            DateTime _dataTime = DateTime.Now;
            Buf.BeWrite(0);
            Buf.BeWrite(user.QQ);
            Buf.Write(new Byte[] { 0x76, 0x71, 0x01, 0x9d });
            Buf.BeWrite(ByteHelper.GetTimeMillis(_dataTime));
            Buf.BeWrite(user.TXProtocol.DwServiceId);
            Buf.Write(new Byte[] { 0x77, 0x69, 0x6e, 0x64, 0x6f, 0x77, 0x73, 0x00, 0x04, 0x5f, 0x80, 0x33, 0x01, 0x01 });
            Buf.BeWrite(user.TXProtocol.DwClientVer);
            Buf.Write(new Byte[]
                {0x66, 0x35, 0x4d, 0xf1, 0xab, 0xdc, 0x98, 0xf0, 0x70, 0x69, 0xfc, 0x2a, 0x2b, 0x86, 0x06, 0x1b});
            Buf.BeWrite(user.TXProtocol.SubVer);

            var Data = new BinaryWriter(new MemoryStream());
            Data.BeWrite(0);
            Data.BeWrite(user.QQ);
            Data.Write(new Byte[] { 0x76, 0x71, 0x01, 0x9d });
            Data.BeWrite(ByteHelper.GetTimeMillis(_dataTime));
            Data.Write(user.TXProtocol.DwPubNo);

            Buf.Write((Byte)Data.BaseStream.Length * 3);
            Buf.Write(Data.ToByteArray());
            Buf.Write(Data.ToByteArray());
            Buf.Write(Data.ToByteArray());


            this.FillHead(this.Command);
            this.FillBody(Buf.ToByteArray(), Buf.BaseStream.Length);
            this.SetLength();
            return this.GetBuffer();
        }

        public void Parser_Tlv(QQAccount user, BinaryReader buf) {
            var type = buf.BeReadUInt16(); //type
            var length = buf.BeReadUInt16(); //length
            buf.ReadBytes(length);
        }
    }
}