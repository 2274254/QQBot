using ChatBotFramework.InstanceHelper.AppInstance;
using ChatBotFramework.InstanceHelper.AppInstance.Interfaces;
using System;

namespace QQBot {
    public class QQBotStartup {
        static void Main(string[] args) {
            AppEntry.InitializeApplication(args);
        }
    }

    public class QQBot : IAppEntry {

        public Boolean __AppEntry(String[] args) {
            return false;
        }
    }

}
