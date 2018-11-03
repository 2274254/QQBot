using LibLogger.LogConfig.Interfaces;
using System;

namespace LibLogger.LogConfig {
    public class LoggerConfigSuccess : ILoggerConfig {
        public String Badge { get; set; } = "✔";
        public String Label { get; set; } = "Success";
        public ConsoleColor Color { get; set; } = ConsoleColor.Green;
    }
}
