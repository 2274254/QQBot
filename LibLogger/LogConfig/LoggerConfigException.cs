using LibLogger.LogConfig.Interfaces;
using System;

namespace LibLogger.LogConfig {
    public class LoggerConfigException : ILoggerConfig {
        public String Badge { get; set; } = "✖";
        public String Label { get; set; } = "Exception";
        public ConsoleColor Color { get; set; } = ConsoleColor.Red;
    }
}
