using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using ChatBot.InstanceHelper.Singleton;

namespace ChatBot.Plugin {
    public delegate bool PluginClassLoadCallback(Type _Class);
    public delegate void PluginMethodInvokeCallback(Object _Object, Type _Class, MethodInfo _Method);

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

        private bool GetTypeObject(Type _Type, out Object _Object) {
            if (!this.TypeObjectMap.ContainsKey(_Type)) {
                try {
                    var TypeObject = Activator.CreateInstance(_Type);
                    this.TypeObjectMap.Add(_Type, TypeObject);
                } catch (Exception) {
                    _Object = null;
                    return false;
                }
            }
            return this.TypeObjectMap.TryGetValue(_Type, out _Object);
        }

        private void InvokeMethodChecker(Object _Object, Type _Class, PluginMethodInvokeCallback Callback) {
            foreach (var Method in _Class.GetMethods()) {
                try {
                    Callback.Invoke(_Object, _Class, Method);
                } catch (Exception) {
                    /*Do Nothing*/
                }
            }
        }

        private void InvokeClassChecker(Dictionary<PluginClassLoadCallback, PluginMethodInvokeCallback> CheckerMap, Type _Type) {
            foreach (var Callback in CheckerMap.Keys) {
                bool SlouldLoadClass = false;

                try {
                    SlouldLoadClass = Callback.Invoke(_Type);
                } catch (Exception) {
                    /*Do Nothing*/
                }

                if (SlouldLoadClass) {
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

        public void LoadUserPlugin(String PluginPath) {
            LoadPluginWithChecker(this.UserPluginCheckerMap, PluginPath);
        }


        public void LoadCorePlugin(String PluginPath) {
            LoadPluginWithChecker(this.CorePluginCheckerMap, PluginPath);
        }
    }
}
