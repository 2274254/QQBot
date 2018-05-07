using System;
using System.Collections.Generic;
using System.Text;

namespace ChatBot.InstanceHelper.Singleton {
    public class NewSingletonClassException : Exception {
        public NewSingletonClassException(String message) : base(message) {
        }
    }
}
