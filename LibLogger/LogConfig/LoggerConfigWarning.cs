using LibLogger.LogConfig.Interfaces;
using System;

namespace LibLogger.LogConfig {
    public class LoggerConfigWarning : ILoggerConfig {
        public String Badge { get; set; } = "⚠";
        public String Label { get; set; } = "Warning";
        public ConsoleColor Color { get; set; } = ConsoleColor.Yellow;
    }
}
