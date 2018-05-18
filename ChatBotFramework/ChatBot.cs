using ChatBotFramework.InstanceHelper.AppInstance.Interfaces;
using ChatBotFramework.InstanceHelper.Singleton;
using System;

namespace ChatBotFramework {
    class SingletonTest : Singleton<SingletonTest> {
        public void ShowMessage() {
            Console.WriteLine("COOOOOOOOOOOOOOOOOOOOOOOOL");
        }
    }

    ///public class ChatBot : IAppEntry {
    //    public Boolean AppEntry(String[] args) {
       //     SingletonTest.Instance.ShowMessage();
      //      new SingletonTest();/*  <-- NewSingletonClassException  */
       //     return false;
      //  }
   // }
}
