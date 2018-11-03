using LibLogger.LogConfig.Interfaces;
using System;

namespace LibLogger.LogConfig {
    public class LoggerConfigError : ILoggerConfig {
        public String Badge { get; set; } = "✖";
        public String Label { get; set; } = "Error";
        public ConsoleColor Color { get; set; } = ConsoleColor.Red;
    }
}
