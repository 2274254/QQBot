using LibLogger.LogConfig.Interfaces;
using System;

namespace LibLogger.LogConfig {
    public class LoggerConfigDebug : ILoggerConfig {
        public String Badge { get; set; } = "▶";
        public String Label { get; set; } = "Debug";
        public ConsoleColor Color { get; set; } = ConsoleColor.Blue;
    }
}
