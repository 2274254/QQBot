using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ChatBot.InstanceHelper.Singleton;

namespace ChatBot {
    class SingletonTest : Singleton<SingletonTest> {
        public void ShowMessage() {
            Console.WriteLine("COOOOOOOOOOOOOOOOOOOOOOOOL");
        }
    }

    class ChatBot {
        static void Main(string[] args) {
            SingletonTest.Instance.ShowMessage();
            new SingletonTest();/*  <-- NewSingletonClassException  */
        }
    }
}
