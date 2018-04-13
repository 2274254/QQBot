namespace QQBot.Plugin {
    public interface QQBotPlugin {
        bool OnPluginLoad(PluginsManager manager);
        void OnPluginUnoad();
    }
}
