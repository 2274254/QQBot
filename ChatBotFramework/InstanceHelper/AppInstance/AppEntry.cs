using System;
using System.Collections.Generic;
using System.Reflection;

namespace ChatBotFramework.InstanceHelper.AppInstance {
    public static class AppEntry {
        private static List<Type> AutoLoadClassList = new List<Type>();
        private static Type AppEntryType = null;

        private static bool IsAutoloadClass(Type _Class) {
            return _Class.GetInterface("ChatBotFramework.InstanceHelper.AppInstance.Interfaces.IAutoLoad") != null;
        }

        private static bool IsAppEntryClass(Type _Class) {
            return _Class.GetInterface("ChatBotFramework.InstanceHelper.AppInstance.Interfaces.IAppEntry") != null;
        }

        private static void InvokeAutoLoad(Type _Class) {
            var Instance = Activator.CreateInstance(_Class);
            var OnStartupAutoLoadMethod = _Class.GetMethod("__OnStartupAutoLoad");
            OnStartupAutoLoadMethod.Invoke(Instance, null);
        }

        private static Boolean InvokeAppEntry(Type _Class, string[] args) {
            var Instance = Activator.CreateInstance(_Class);
            var OnStartupAutoLoadMethod = _Class.GetMethod("__AppEntry");
            var Result = OnStartupAutoLoadMethod.Invoke(Instance, new Object[] { args });
            return (Result is Boolean) ? (Boolean)Result : false;
        }

        private static bool InitializeClassList(Type[] LoadedEntryTypesList) {
            foreach (var LoadedEntryType in LoadedEntryTypesList) {
                if (IsAutoloadClass(LoadedEntryType)) {
                    if (!AutoLoadClassList.Contains(LoadedEntryType)) {
                        AutoLoadClassList.Add(LoadedEntryType);
                    }
                } else if (IsAppEntryClass(LoadedEntryType)) {
                    if (AppEntryType == null) {
                        AppEntryType = LoadedEntryType;
                    } else {
                        return false;
                    }
                }
            }
            return AppEntryType != null;
        }

        public static void InitializeApplication(string[] args) {
            var LoadedEntryTypesList = Assembly.GetCallingAssembly().GetTypes();
            if (InitializeClassList(LoadedEntryTypesList)) {
                foreach (var AutoLoadClass in AutoLoadClassList) {
                    InvokeAutoLoad(AutoLoadClass);
                }
                InvokeAppEntry(AppEntryType, args);
            }
        }
    }
}
