using ChatBotFramework.InstanceHelper.AppInstance.Interfaces;
using ChatBotFramework.InstanceHelper.Singleton;
using ChatBotFramework.Interfaces.Plugin;
using ChatBotFramework.Plugin.Interfaces;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace ChatBotFramework.Plugin {
    public class PluginManager {
        private readonly Dictionary<PluginAttribute, Object> pluginObjectMap = new Dictionary<PluginAttribute, Object>();
        private readonly List<PluginLoadCallback> PluginCheckerList = new List<PluginLoadCallback>();

        private bool IsSameUserPluginLoaded(PluginAttribute attribute) {
            foreach (var loadedPluginAttribute in this.pluginObjectMap.Keys) {
                if (loadedPluginAttribute.Equals(attribute)) {
                    return true;
                }
            }
            return false;
        }

        private void InvokePluginChecker(PluginAttribute attribute, Object _Object) {
            foreach (var Checker in this.PluginCheckerList) {
                Checker.Invoke(attribute, (IChatBotPlugin)_Object);
            }
        }

        public void IsChatbotPlugin(String pluginPath) {
            var assembly = System.Reflection.Assembly.LoadFile(pluginPath);

            foreach (var type in assembly.GetTypes()) {
                if (type.GetInterface("ChatBotFramework.Plugin.Interfaces.IChatBotPlugin") != null) {
                    var attributes = Attribute.GetCustomAttributes(type);

                    if (attributes.Length == 1 && !IsSameUserPluginLoaded((PluginAttribute)attributes[0])) {
                        var pluginObject = Activator.CreateInstance(type);
                        var piuginLoadMethodInfo = type.GetMethod("OnPluginLoad");

                        try {
                            if ((bool)piuginLoadMethodInfo.Invoke(pluginObject, new object[] { this })) {
                                this.pluginObjectMap.Add((PluginAttribute)attributes[0], pluginObject);
                                InvokePluginChecker((PluginAttribute)attributes[0], pluginObject);
                            }
                        } catch (Exception e) {
                            BotInstance.Instance.Logger.Exception(e);
                        }
                    }
                }
            }
        }

        public void RegisterPluginChecker(IPluginChecker Checker) {
            this.PluginCheckerList.Add(Checker.PluginLoadCallback);
        }
    }
}