using System;

namespace QQBot {
    class Program {
        static void Main(string[] args) {
            QQBot bot = new QQBot();

            bot.LoginWithPassword(2929199157, BitConverter.GetBytes(123456));

            Console.WriteLine("just test");

            while (true) {
                
            }
        }
    }
}
