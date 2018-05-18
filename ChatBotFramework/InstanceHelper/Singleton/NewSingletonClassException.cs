using System;

namespace ChatBotFramework.InstanceHelper.Singleton {
    public class NewSingletonClassException : Exception {
        public NewSingletonClassException(String message) : base(message) {
        }
    }
}
