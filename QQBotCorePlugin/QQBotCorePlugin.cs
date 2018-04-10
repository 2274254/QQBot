using System;

using QQBot.PluginsManager;

namespace QQBotCorePlugin {
    [Plugin("QQBotCorePlugin", "VeroFess", "coderzeng@hotmail.com", 0, 1)]
    public class QQBotCorePlugin : QQBotPlugin {
        public bool OnPluginLoad(Manager manager) {
            Console.WriteLine("I'm loaded!!");
            return false;
        }

        public bool OnPluginUnoad() {
            throw new NotImplementedException();
        }
    }
}
