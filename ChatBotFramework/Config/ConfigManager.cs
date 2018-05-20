using ChatBotFramework.Config.Struct;
using ChatBotFramework.Config.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ChatBotFramework.Config {
    public class ConfigManager {
        public static ChatBotConfig Config { get; private set; } = null;

        public static ChatBotConfig LoadConfig(String ConfigPath) {
            if (Config == null) {
                Config = new ChatBotConfig();

                if (!File.Exists(ConfigPath)) {
                    var ParentPath = Directory.GetParent(ConfigPath);
                    if (!ParentPath.Exists) {
                        ParentPath.Create();
                    }
                }
                File.WriteAllBytes(ConfigPath, Encoding.UTF8.GetBytes(JsonHelper.SerializeObject(Config)));
            } else {
                Config = JsonHelper.DeserializeJsonToObject<ChatBotConfig>(Encoding.UTF8.GetString(File.ReadAllBytes(ConfigPath)));
            }
            return Config;
        }
    }
}
