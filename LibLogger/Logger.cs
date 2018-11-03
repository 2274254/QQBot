using LibLogger.LogConfig;
using LibLogger.LogConfig.Interfaces;
using System;
using System.Diagnostics;

namespace LibLogger {
    public static class Logger {
        public static LogLevel Level { get; set; } = LogLevel.INFO;
        public static Boolean EnableBadge { get; set; } = false;
        public static Boolean EnableTime { get; set; } = true;
        public static Boolean EnableCaller { get; set; } = true;
        public static Boolean EnableColor { get; set; } = true;

        public static ILoggerConfig ConfigDebug { get; set; } = new LoggerConfigDebug();
        public static ILoggerConfig ConfigInfo { get; set; } = new LoggerConfigInfo();
        public static ILoggerConfig ConfigSuccess { get; set; } = new LoggerConfigSuccess();
        public static ILoggerConfig ConfigWarning { get; set; } = new LoggerConfigWarning();
        public static ILoggerConfig ConfigError { get; set; } = new LoggerConfigError();
        public static ILoggerConfig ConfigException { get; set; } = new LoggerConfigException();

        private static String GetCallerType() {
            if (EnableCaller) {
                var trace = new StackTrace();
                return trace.GetFrame(2).GetMethod().ReflectedType.Name;
            } else {
                return null;
            }
        }

        private static String GetCallerName() {
            if (EnableCaller) {
                var trace = new StackTrace();
                return trace.GetFrame(2).GetMethod().Name;
            } else {
                return null;
            }
        }

        private static void LogToConsole(ILoggerConfig Config, String CallerType, String CallerFunction, String Message) {
            ConsoleColor OldColor = Console.ForegroundColor;
            var PrefixText = EnableBadge ? String.Format("[{0} {1,-10}]", Config.Badge, Config.Label) : String.Format("[{0,-10}]", Config.Label);
            var TimeText = EnableTime ? String.Format("[{0}]", DateTime.Now.ToString("G")) : String.Empty;
            var CallerText = EnableCaller ? String.Format("[{0}::{1}()]", CallerType, CallerFunction) : String.Empty;
            var FullUncolorMessage = String.Format("{0}{1}: {2}\n", TimeText, CallerText, Message);

            if (EnableColor) {
                Console.ForegroundColor = Config.Color;
                Console.Write(PrefixText);
                Console.ForegroundColor = OldColor;
                Console.Write(FullUncolorMessage);
            } else {
                Console.Write(PrefixText + FullUncolorMessage);
            }
        }

        public static void Debug(String Message) {
            if (Level <= LogLevel.DEBUG) {
                LogToConsole(ConfigDebug, GetCallerType(), GetCallerName(), Message);
            }
        }

        public static void Success(String Message) {
            if (Level <= LogLevel.INFO) {
                LogToConsole(ConfigSuccess, GetCallerType(), GetCallerName(), Message);
            }
        }

        public static void Info(String Message) {
            if (Level <= LogLevel.INFO) {
                LogToConsole(ConfigInfo, GetCallerType(), GetCallerName(), Message);
            }
        }

        public static void Warning(String Message) {
            if (Level <= LogLevel.WARNING) {
                LogToConsole(ConfigWarning, GetCallerType(), GetCallerName(), Message);
            }
        }

        public static void Error(String Message) {
            if (Level <= LogLevel.ERROR) {
                LogToConsole(ConfigError, GetCallerType(), GetCallerName(), Message);
            }
        }

        public static void Exception(Exception exception) {
            if (Level <= LogLevel.ERROR) {

                Exception exception_t = exception;
                var FinalMessage = "";

                while (exception_t != null) {
                    var HelpLink = exception_t.HelpLink ?? "[No HelpLink]";
                    var StackTrace = exception_t.StackTrace ?? "[No StackTrace]";
                    var Message = String.Format("\nType: {0}\nMessage: {1}\nHelpLink: {2}\nStackTrace: \n{3}\n", exception_t.GetType().Name, exception_t.Message, HelpLink, StackTrace);

                    FinalMessage = FinalMessage.Length == 0 ? Message : FinalMessage + "\nCaused by:\n" + Message;
                    exception_t = exception_t.InnerException;
                }

                LogToConsole(ConfigException, GetCallerType(), GetCallerName(), FinalMessage);
            }
        }
    }
}
