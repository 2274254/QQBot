using CommandLine;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChatBotFramework.Config.CommandLine {
    public class CommandLineArguments {
        [Option('c', "config", Required = false, HelpText = "Specify the configuration file path")]
        public String ConfigPath { get; set; } = "config/chatbot.json";
    }
}
