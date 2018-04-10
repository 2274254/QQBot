using System;
using System.Collections.Generic;
using System.Text;

namespace QQBot.PluginsManager {
    public interface QQBotPlugin {
        bool OnPluginLoad(Manager manager);
        bool OnPluginUnoad();
    }
}
