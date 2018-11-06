using LibQQProtocol.Defines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LibQQProtocol.Util {
    public class Richtext {
        public List<TextSnippet> Snippets = new List<TextSnippet>();
        public Int32 Length => this.Snippets.Sum(s => s.Length);

        public static Richtext Parse(Byte[] message) {
            // TODO: 解析富文本
            return Encoding.UTF8.GetString(message);
        }

        public static Richtext FromLiteral(String message) {
            return new Richtext { Snippets = new List<TextSnippet> { new TextSnippet(message ?? "") } };
        }

        public static Richtext FromSnippets(params TextSnippet[] message) {
            return new Richtext { Snippets = message.ToList() };
        }

        public override String ToString() {
            return String.Join("", this.Snippets);
        }

        public static implicit operator String(Richtext text) {
            return text?.ToString();
        }

        public static implicit operator Richtext(String text) {
            return FromLiteral(text);
        }

        public static implicit operator Richtext(TextSnippet text) {
            return FromSnippets(text);
        }
    }

    public class TextSnippet {
        public String Content;
        public MessageType Type;

        public TextSnippet(String message = "", MessageType type = MessageType.Normal) {
            this.Content = message;
            this.Type = type;
        }

        public Int32 Length {
            get {
                switch (this.Type) {
                    case MessageType.Normal:
                        return Encoding.UTF8.GetByteCount(this.Content);
                    case MessageType.Emoji:
                        return 12;
                    case MessageType.Picture:
                    case MessageType.Xml:
                    case MessageType.Json:
                    case MessageType.At:
                    case MessageType.Shake:
                    case MessageType.ExitGroup:
                    case MessageType.GetGroupImformation:
                    case MessageType.AddGroup:
                        return 0;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        public override String ToString() {
            switch (this.Type) {
                case MessageType.Normal:
                    return this.Content;
                case MessageType.Shake:
                    return "[窗口抖动]";
                case MessageType.Picture:
                    return $"[图片{this.Content}]";
                case MessageType.Xml:
                    return $"[XML代码{this.Content}]";
                case MessageType.Json:
                    return $"[JSON代码{this.Content}]";
                case MessageType.Emoji:
                    return $"[表情{this.Content}]";
                case MessageType.At:
                    return $"[@{this.Content}]";
                case MessageType.Audio:
                    return $"[音频{this.Content}]";
                case MessageType.Video:
                    return $"[视频{this.Content}]";
                case MessageType.ExitGroup:
                    return "[退出群]";
                case MessageType.GetGroupImformation:
                    return "[获取群信息]";
                case MessageType.AddGroup:
                    return "[加群]";
                case MessageType.OfflineFile:
                    return $"[离线文件{this.Content}]";
                default:
                    return "[特殊代码]";
            }
        }

        public static implicit operator String(TextSnippet text) {
            return text?.ToString();
        }

        public static implicit operator TextSnippet(String text) {
            return new TextSnippet(text);
        }
    }
}
