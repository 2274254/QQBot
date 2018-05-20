using System;
using System.Collections.Generic;
using System.Text;

namespace ChatBotFramework.Plugin {
    public class PluginAttribute : Attribute {
        public string PluginName { get; }
        public string Author { get; }
        public string AuthorEmail { get; }
        public int MajorVersion { get; }
        public int Minorversion { get; }

        public PluginAttribute(string pluginName, string author, string authorEmail, int majorVersion, int minorversion) {
            this.PluginName = pluginName;
            this.Author = author;
            this.AuthorEmail = authorEmail;
            this.MajorVersion = majorVersion;
            this.Minorversion = minorversion;
        }

        public override bool Equals(object obj) {
            if (obj == null || GetType() != obj.GetType()) {
                return false;
            }

            return this.PluginName.Equals(((PluginAttribute)obj).PluginName) && this.Author.Equals(((PluginAttribute)obj).Author);
        }

        public override int GetHashCode() {
            return base.GetHashCode();
        }
    }
}
