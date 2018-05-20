using System;
using System.Collections.Generic;
using System.Text;

namespace ChatBotFramework.Plugin.Interfaces {
    public interface IChatBotPlugin {
        bool OnPluginLoad(PluginManager manager);
    }
}
