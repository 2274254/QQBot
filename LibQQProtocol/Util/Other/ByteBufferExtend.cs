using LibQQProtocol.Defines;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace LibQQProtocol.Util {
    public static class ByteBufferExtend {
        public static Byte[] ToByteArray(this BinaryWriter bw) {
            var savedPos = bw.BaseStream.Position;
            var resultArr = new Byte[bw.BaseStream.Position];
            bw.BaseStream.Position = 0;
            bw.BaseStream.Read(resultArr);
            bw.BaseStream.Position = savedPos;
            return resultArr;
        }


        public static void BeWrite(this BinaryWriter bw, UInt16 v) {
            var arr = BitConverter.GetBytes(v);
            Array.Reverse(arr);
            bw.Write(arr);
        }

        public static void BeWrite(this BinaryWriter bw, DateTime v) {
            bw.BeWrite(ByteHelper.GetTimeSeconds(v));
        }

        public static void BeWrite(this BinaryWriter bw, Char v) {
            var arr = BitConverter.GetBytes(v);
            Array.Reverse(arr);
            bw.Write(arr);
        }

        public static void BeWrite(this BinaryWriter bw, Int32 v) {
            var arr = BitConverter.GetBytes(v);
            Array.Reverse(arr);
            bw.Write(arr);
        }

        public static void BeWrite(this BinaryWriter bw, UInt32 v) {
            var arr = BitConverter.GetBytes(v);
            Array.Reverse(arr);
            bw.Write(arr);
        }

        public static void BeUshortWrite(this BinaryWriter bw, UInt16 v) {
            bw.BeWrite(v);
        }

        public static void BeWrite(this BinaryWriter bw, Int64 v) {
            var arr = BitConverter.GetBytes(v);
            Array.Reverse(arr);
            bw.Write(arr);
        }

        public static void BeWrite(this BinaryWriter bw, UInt64 v) {
            var arr = BitConverter.GetBytes(v);
            Array.Reverse(arr);
            bw.Write(arr);
        }

        public static void WriteKey(this BinaryWriter bw, Byte[] v) {
            bw.BeWrite((UInt16)v.Length);
            bw.Write(v);
        }

        public static List<Byte[]> WriteSnippet(TextSnippet snippet, Int32 length) {
            // TODO: 富文本支持
            var ret = new List<Byte[]>();
            var bw = new BinaryWriter(new MemoryStream());
            switch (snippet.Type) {
                case MessageType.Normal: {
                        if (length + 6 >= 699) // 数字应该稍大点，但是我不清楚具体是多少
                        {
                            length = 0;
                            ret.Add(new Byte[0]);
                        }

                        bw.BaseStream.Position = 6;
                        foreach (var chr in snippet.Content) {
                            var bytes = Encoding.UTF8.GetBytes(chr.ToString());
                            // 705 = 699 + 6个byte: (byte + short + byte + short)
                            if (length + bw.BaseStream.Length + bytes.Length > 705) {
                                var pos = bw.BaseStream.Position;
                                bw.BaseStream.Position = 0;
                                bw.Write(new Byte[] { 0x01 });
                                bw.BeWrite((UInt16)(pos - 3)); // 本来是+3和0的，但是提前预留了6个byte给它们，所以变成了-3和-6。下同理。
                                bw.Write(new Byte[] { 0x01 });
                                bw.BeWrite((UInt16)(pos - 6));
                                bw.BaseStream.Position = pos;
                                var arr = new Byte[bw.BaseStream.Length];
                                bw.BaseStream.Read(arr);
                                ret.Add(arr);
                                bw = new BinaryWriter(new MemoryStream());
                                bw.BaseStream.Position = 6;
                                length = 0;
                            }

                            bw.Write(bytes);
                        }

                        // 在最后一段的开头补充结构 
                        {
                            var pos = bw.BaseStream.Position;
                            bw.BaseStream.Position = 0;
                            bw.Write(new Byte[] { 0x01 });
                            bw.BeWrite((UInt16)(pos - 3));
                            bw.Write(new Byte[] { 0x01 });
                            bw.BeWrite((UInt16)(pos - 6));
                            bw.BaseStream.Position = pos;
                        }
                        break;
                    }
                case MessageType.At:
                    break;
                case MessageType.Emoji: {
                        if (length + 12 > 699) {
                            ret.Add(new Byte[0]);
                        }

                        var faceIndex = Convert.ToByte(snippet.Content);
                        if (faceIndex > 199) {
                            faceIndex = 0;
                        }

                        bw.Write(new Byte[] { 0x02, 0x00, 0x14, 0x01, 0x00, 0x01 });
                        bw.Write(faceIndex);
                        bw.Write(new Byte[] { 0xFF, 0x00, 0x02, 0x14 });
                        bw.Write((Byte)(faceIndex + 65));
                        bw.Write(new Byte[] { 0x0B, 0x00, 0x08, 0x00, 0x01, 0x00, 0x04, 0x52, 0xCC, 0x85, 0x50 });
                        break;
                    }
                case MessageType.Picture:
                    break;
                case MessageType.Xml:
                    break;
                case MessageType.Json:
                    break;
                case MessageType.Shake:
                    break;
                case MessageType.Audio:
                    break;
                case MessageType.Video:
                    break;
                case MessageType.ExitGroup:
                    break;
                case MessageType.GetGroupImformation:
                    break;
                case MessageType.AddGroup:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (bw.BaseStream.Position != 0) {
                //var arr = new Byte[bw.BaseStream.Length];
                // bw.BaseStream.Read(arr);
                ret.Add(bw.ToByteArray());
            }

            return ret;
        }

        public static List<Byte[]> WriteRichtext(Richtext richtext) {
            if (richtext.Snippets.Count > 1) {
                if (!richtext.Snippets.TrueForAll(s =>
                    s.Type == MessageType.Normal || s.Type == MessageType.At || s.Type == MessageType.Emoji ||
                    s.Type == MessageType.Picture)) {
                    throw new NotSupportedException("富文本中包含多个非聊天代码");
                }
            }

            // TODO: 富文本支持
            var ret = new List<Byte[]>();
            var bw = new BinaryWriter(new MemoryStream());
            foreach (TextSnippet snippet in richtext.Snippets) {
                List<Byte[]> list = WriteSnippet(snippet, (Int32)bw.BaseStream.Position);
                for (var i = 0; i < list.Count; i++) {
                    bw.Write(list[i]);
                    // 除最后一个以外别的都开新的包
                    //   如果有多个，那前几个一定是太长了被分段了，所以开新的包
                    //   如果只有一个/是最后一个，那就不开
                    if (i == list.Count - 1) {
                        break;
                    }

                    var arr_t = new Byte[bw.BaseStream.Length];
                    bw.BaseStream.Read(arr_t);
                    ret.Add(arr_t);
                    bw = new BinaryWriter(new MemoryStream());
                }
            }

            var arr = new Byte[bw.BaseStream.Length];
            bw.BaseStream.Read(arr);
            ret.Add(arr);
            return ret;
        }

        public static Char BeReadChar(this BinaryReader br) {
            return (Char)br.BeReadUInt16();
        }

        public static UInt16 BeReadUInt16(this BinaryReader br) {
            return (UInt16)((br.ReadByte() << 8) + br.ReadByte());
        }

        public static Int32 BeReadInt32(this BinaryReader br) {
            return (br.ReadByte() << 24) | (br.ReadByte() << 16) | (br.ReadByte() << 8) | br.ReadByte();
        }

        public static UInt32 BeReadUInt32(this BinaryReader br) {
            return (UInt32)((br.ReadByte() << 24) | (br.ReadByte() << 16) | (br.ReadByte() << 8) | br.ReadByte());
        }

        public static Richtext ReadRichtext(this BinaryReader br) {
            // TODO: 解析富文本
            // 目前进度: 仅读取第一部分
            return Richtext.Parse(br.ReadBytes(br.BeReadChar()));
        }
    }
}
