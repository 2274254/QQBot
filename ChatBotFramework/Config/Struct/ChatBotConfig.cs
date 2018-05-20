using ChatBotFramework.Config.Struct;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChatBotFramework.Config.Struct {
    public class ChatBotConfig {
        public List<CorePlugin> CorePluginList { get; set; } = new List<CorePlugin>();
    }
}
