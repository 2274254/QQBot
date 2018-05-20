using ChatBotFramework.Account.Interface;
using ChatBotFramework.Config;
using ChatBotFramework.Config.CommandLine;
using ChatBotFramework.Config.Interfaces;
using ChatBotFramework.Config.Util;
using ChatBotFramework.Event;
using ChatBotFramework.InstanceHelper.AppInstance;
using ChatBotFramework.InstanceHelper.Singleton;
using ChatBotFramework.Log;
using ChatBotFramework.Network;
using ChatBotFramework.Plugin;
using CommandLine;
using System;
using System.IO;

namespace ChatBotFramework {
    public class BotInstance : Singleton<BotInstance> {
        private static CommandLineArguments CommandLineArguments = null;

        public readonly EventManager EventManager = EventManager.Instance;
        public readonly AsyncNetworkManager NetworkManager = AsyncNetworkManager.Instance;
        public readonly PluginManager PluginManager = new PluginManager();
        public readonly Logger Logger = new Logger() { Level = LogLevel.ALL };

        private static Boolean ParseCommandLine(String[] args) {
            return (Parser.Default.ParseArguments<CommandLineArguments>(args).WithParsed(options => { CommandLineArguments = options; }).Tag == ParserResultType.Parsed);
        }


        public void BeginNewBotInstance(String Protcol, IChatBotAccount Account, IChatBotConfig Config) {

        }

        public static void Main(String[] args) {
            if (ParseCommandLine(args)) {
                AppEntry.InitializeApplication();
            }
        }

        private static Object RunAndReturnExitCode(CommandLineArguments options) {
            throw new NotImplementedException();
        }
    }
}
