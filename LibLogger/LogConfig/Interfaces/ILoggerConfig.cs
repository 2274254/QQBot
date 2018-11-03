using System;

namespace LibLogger.LogConfig.Interfaces {
    public interface ILoggerConfig {
        String Badge { get; set; }
        String Label { get; set; }
        ConsoleColor Color { get; set; }
    }
}
