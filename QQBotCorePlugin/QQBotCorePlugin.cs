using System;
using QQBot.Plugin;

namespace QQBotCorePlugin {
    [Plugin("QQBotCorePlugin", "VeroFess", "coderzeng@hotmail.com", 0, 1)]
    public class QQBotCorePlugin : QQBotPlugin {
        public bool OnPluginLoad(PluginsManager manager) {
            Console.WriteLine("I'm loaded!!");
            return false;
        }

        public void OnPluginUnoad() {
            throw new NotImplementedException();
        }
    }
}
