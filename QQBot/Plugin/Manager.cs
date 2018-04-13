using System;
using System.Collections.Generic;

namespace QQBot.Plugin {
    public class PluginsManager {
        Dictionary<PluginAttribute, object> pluginObjectMap = new Dictionary<PluginAttribute, object>();

        private bool IsSamePluginLoaded(PluginAttribute attribute) {
            foreach (var loadedPluginAttribute in pluginObjectMap.Keys) {
                if (loadedPluginAttribute.Equals(attribute)) {
                    return true;
                }
            }
            return false;
        }

        public bool LoadPlugin(String pluginPath) {
            var assembly = System.Reflection.Assembly.LoadFile(pluginPath);
            foreach (var type in assembly.GetTypes()) {
                if (type.GetInterface("QQBot.PluginsManager.QQBotPlugin") == null) {
                    break;
                } else {
                    var attributes = Attribute.GetCustomAttributes(type);
                    if (attributes.Length == 1 && !IsSamePluginLoaded((PluginAttribute)attributes[ 0 ])) {
                        var pluginObject = Activator.CreateInstance(type);
                        var piuginLoadMethodInfo = type.GetMethod("OnPluginLoad");

                        try {
                            object result = piuginLoadMethodInfo.Invoke(pluginObject, new object[] { this });
                            if (!(bool)result) { return false; }
                        } catch (Exception) {
                            return false;
                        }

                        lock (pluginObjectMap) {
                            pluginObjectMap.Add((PluginAttribute)attributes[ 0 ], pluginObject);
                        }

                        return true;
                    }
                }
            }
            return false;
        }

    }
}