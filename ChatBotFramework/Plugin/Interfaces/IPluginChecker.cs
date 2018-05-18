using System;
using System.Reflection;

namespace ChatBotFramework.Interfaces.Plugin {
    public delegate bool PluginClassLoadCallback(Type _Class);
    public delegate void PluginMethodInvokeCallback(Object _Object, Type _Class, MethodInfo _Method);

    public interface IPluginChecker {
        bool PluginClassLoadCallback(Type _Class);
        void PluginMethodInvokeCallback(Object _Object, Type _Class, MethodInfo _Method);
    }
}
