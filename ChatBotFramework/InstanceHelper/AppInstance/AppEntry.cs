using ChatBotFramework.Log;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace ChatBotFramework.InstanceHelper.AppInstance {
    public static class AppEntry {
        private static List<Type> AutoLoadClassList = new List<Type>();

        private static bool IsAutoloadClass(Type _Class) {
            return _Class.GetInterface("ChatBotFramework.InstanceHelper.AppInstance.Interfaces.IAutoLoad") != null;
        }

        private static Boolean InvokeAutoLoad(Type _Class) {
            var Instance = Activator.CreateInstance(_Class);
            var OnStartupAutoLoadMethod = _Class.GetMethod("__OnStartupAutoLoad");

            BotInstance.Instance.Logger.Debug("begin invoke entry class: " + _Class.FullName);
            Object Result = null;
            try {
                Result = OnStartupAutoLoadMethod.Invoke(Instance, null);
            } catch (Exception e) {
                BotInstance.Instance.Logger.Exception(e);
                throw e;
            }
            return (Result is Boolean) ? (Boolean)Result : false;
        }

        private static bool InitializeClassList(Type[] LoadedEntryTypesList) {
            foreach (var LoadedEntryType in LoadedEntryTypesList) {
                if (IsAutoloadClass(LoadedEntryType)) {
                    if (!AutoLoadClassList.Contains(LoadedEntryType)) {
                        BotInstance.Instance.Logger.Debug("Add new autoload class to list :" + LoadedEntryType.FullName);
                        AutoLoadClassList.Add(LoadedEntryType);
                    }
                }
            }
            return true;
        }

        public static void InitializeApplication() {
            var LoadedEntryTypesList = Assembly.GetEntryAssembly().GetTypes();
            BotInstance.Instance.Logger.Info("Begin Initialize ...");
            if (InitializeClassList(LoadedEntryTypesList)) {
                foreach (var AutoLoadClass in AutoLoadClassList) {
                    BotInstance.Instance.Logger.Debug("begin invoke autoload class: " + AutoLoadClass.FullName);
                    InvokeAutoLoad(AutoLoadClass);
                }
            }
        }
    }
}
