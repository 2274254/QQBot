using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace ChatBotFramework.Logger {
    public static class Logger {
        private readonly static ConcurrentQueue<String> MessageQueue = new ConcurrentQueue<String>();
        public static void Log<T>(String Message) {

        }
    }
    public class Logger<T> {

    }
}
