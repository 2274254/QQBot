using ChatBotFramework.InstanceHelper.Singleton;
using ChatBotFramework.Interfaces.Plugin;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace ChatBotFramework.Plugin {
    public class PluginManager : Singleton<PluginManager> {
        private readonly Dictionary<Type, Object> TypeObjectMap = new Dictionary<Type, Object>();
        private readonly Dictionary<PluginClassLoadCallback, PluginMethodInvokeCallback> CorePluginCheckerMap = new Dictionary<PluginClassLoadCallback, PluginMethodInvokeCallback>();
        private readonly Dictionary<PluginClassLoadCallback, PluginMethodInvokeCallback> UserPluginCheckerMap = new Dictionary<PluginClassLoadCallback, PluginMethodInvokeCallback>();

        public static bool IsTypeHaveMethodWithAttribute(Type _Type, Type Attribute) {
            foreach (var Method in _Type.GetMethods()) {
                if (Method.GetCustomAttribute(Attribute) != null) {
                    return true;
                }
            }
            return false;
        }

        #region Private Functions
        private bool GetTypeObject(Type _Type, out Object _Object) {
            if (!this.TypeObjectMap.ContainsKey(_Type)) {
                lock (this.TypeObjectMap) {
                    if (!this.TypeObjectMap.ContainsKey(_Type)) {
                        var TypeObject = Activator.CreateInstance(_Type);
                        this.TypeObjectMap.Add(_Type, TypeObject);
                    }
                }
            }
            return this.TypeObjectMap.TryGetValue(_Type, out _Object);
        }

        private void InvokeMethodChecker(Object _Object, Type _Class, PluginMethodInvokeCallback Callback) {
            foreach (var Method in _Class.GetMethods()) {
                Callback.Invoke(_Object, _Class, Method);
            }
        }

        private void InvokeClassChecker(Dictionary<PluginClassLoadCallback, PluginMethodInvokeCallback> CheckerMap, Type _Type) {
            foreach (var Callback in CheckerMap.Keys) {
                if (Callback.Invoke(_Type)) {
                    Object _Object = new Object();
                    if (GetTypeObject(_Type, out _Object)) {
                        InvokeMethodChecker(_Object, _Type, CheckerMap[Callback]);
                    }
                }
            }
        }

        private void LoadPluginWithChecker(Dictionary<PluginClassLoadCallback, PluginMethodInvokeCallback> CheckerMap, String PluginPath) {
            var Assembly = System.Reflection.Assembly.LoadFile(PluginPath);
            foreach (var _Type in Assembly.GetTypes()) {
                InvokeClassChecker(CheckerMap, _Type);
            }
        }
        #endregion

        public void RegisterPluginChecker(bool IsCorePluginChecker, IPluginChecker Checker) {
            PluginClassLoadCallback K = Checker.PluginClassLoadCallback;
            PluginMethodInvokeCallback V = Checker.PluginMethodInvokeCallback;

            if (IsCorePluginChecker) {
                if (!this.CorePluginCheckerMap.ContainsKey(K)) {
                    lock (this.CorePluginCheckerMap) {
                        if (!this.CorePluginCheckerMap.ContainsKey(K)) {
                            this.CorePluginCheckerMap.Add(K, V);
                        }
                    }
                }
            } else {
                if (!this.UserPluginCheckerMap.ContainsKey(K)) {
                    lock (this.UserPluginCheckerMap) {
                        if (!this.UserPluginCheckerMap.ContainsKey(K)) {
                            this.UserPluginCheckerMap.Add(K, V);
                        }
                    }
                }
            }
        }

        public void LoadUserPlugin(String PluginPath) {
            lock (this.UserPluginCheckerMap) {
                LoadPluginWithChecker(this.UserPluginCheckerMap, PluginPath);
            }
        }

        public void LoadCorePlugin(String PluginPath) {
            lock (this.CorePluginCheckerMap) {
                LoadPluginWithChecker(this.CorePluginCheckerMap, PluginPath);
            }
        }
    }
}