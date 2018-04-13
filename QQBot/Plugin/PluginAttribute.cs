using System;

namespace QQBot.Plugin {
    public class PluginAttribute : Attribute {
        public string pluginName { get; }
        public string author { get; }
        public string authorEmail { get; }
        public int majorVersion { get; }
        public int minorversion { get; }

        public PluginAttribute(string pluginName, string author, string authorEmail, int majorVersion, int minorversion) {
            this.pluginName = pluginName;
            this.author = author;
            this.authorEmail = authorEmail;
            this.majorVersion = majorVersion;
            this.minorversion = minorversion;
        }

        public override bool Equals(object obj) {
            if (obj == null || GetType() != obj.GetType()) {
                return false;
            }

            return pluginName.Equals(((PluginAttribute)obj).pluginName) && author.Equals(((PluginAttribute)obj).author);
        }

        public override int GetHashCode() {
            return base.GetHashCode();
        }
    }
}
