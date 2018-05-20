using ChatBotFramework.Plugin;
using ChatBotFramework.Plugin.Interfaces;
using System;
using System.Reflection;

namespace ChatBotFramework.Interfaces.Plugin {
    public delegate void PluginLoadCallback(PluginAttribute Attribute, IChatBotPlugin _Object);

    public interface IPluginChecker {
        void PluginLoadCallback(PluginAttribute Attribute, IChatBotPlugin _Object);
    }
}
