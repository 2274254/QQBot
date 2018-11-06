using LibQQProtocol.Structs;
using LibQQProtocol.Util;
using System;
using System.IO;

namespace LibQQProtocol.TlvLib.Tlvs {
    [TlvTag(TlvTags._0x0104)]
    internal class TLV0104 : BaseTLV {
        public TLV0104() {
            this.Command = 0x0104;
            this.Name = "TLV_0104";
        }

        public Byte PngData { get; private set; }

        /// <summary>
        ///     是否还有验证码数据
        /// </summary>
        public Byte Next { get; private set; }

        public void Parser_Tlv(QQAccount user, BinaryReader buf) {
            var type = buf.BeReadUInt16(); //type
            var length = buf.BeReadUInt16(); //length
            var Data = buf.ReadBytes(length);
            var bufData = new BinaryReader(new MemoryStream(Data));
            this.WSubVer = bufData.BeReadUInt16(); //wSubVer
            if (this.WSubVer == 0x0001) {
                var wCsCmd = bufData.BeReadUInt16();
                var errorCode = bufData.BeReadUInt32();

                bufData.ReadByte(); //0x00
                bufData.ReadByte(); //0x05
                this.PngData = bufData.ReadByte(); //是否需要验证码：0不需要，1需要
                Int32 len;
                if (this.PngData == 0x00) {
                    len = bufData.ReadByte();
                    while (len == 0) {
                        len = bufData.ReadByte();
                    }
                } else //ReplyCode != 0x01按下面走 兼容多版本
                  {
                    bufData.BeReadInt32(); //需要验证码时为00 00 01 23，不需要时为全0
                    len = bufData.BeReadUInt16();
                }

                var buffer = bufData.ReadBytes(len);
                user.TXProtocol.BufSigPic = buffer;
                if (this.PngData == 0x01) //有验证码数据
                {
                    len = bufData.BeReadUInt16();
                    buffer = bufData.ReadBytes(len);
                    user.QQPacket00BaVerifyCode = buffer;
                    this.Next = bufData.ReadByte();
                    bufData.ReadByte();
                    //var directory = Util.MapPath("Verify");
                    //var filename = Path.Combine(directory, user.QQ + ".png");
                    //if (!Directory.Exists(directory))
                    //{
                    //    Directory.CreateDirectory(directory);
                    //}

                    //var fs = Next == 0x00
                    //    ? new FileStream(filename, FileMode.Create, FileAccess.ReadWrite, FileShare.Read)
                    //    : new FileStream(filename, FileMode.Append, FileAccess.Write, FileShare.Read);

                    ////fs.Seek(0, SeekOrigin.End);
                    //fs.Write(buffer, 0, buffer.Length);
                    //fs.Close();
                    len = bufData.BeReadUInt16();
                    buffer = bufData.ReadBytes(len);
                    user.TXProtocol.PngToken = buffer;
                    if (bufData.BaseStream.Length > bufData.BaseStream.Position) {
                        bufData.BeReadUInt16();
                        len = bufData.BeReadUInt16();
                        buffer = bufData.ReadBytes(len);
                        user.TXProtocol.PngKey = buffer;
                    }
                }
            } else {
                throw new Exception($"{this.Name} 无法识别的版本号 {this.WSubVer}");
            }
        }
    }
}