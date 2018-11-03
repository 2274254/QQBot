using LibLogger.LogConfig.Interfaces;
using System;

namespace LibLogger.LogConfig {
    public class LoggerConfigInfo : ILoggerConfig {
        public String Badge { get; set; } = "ｉ";
        public String Label { get; set; } = "Info";
        public ConsoleColor Color { get; set; } = ConsoleColor.White;
    }
}
