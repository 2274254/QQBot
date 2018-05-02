using System;
using QQBot.Account;
using QQBot.Network;
using QQBot.Util;

namespace QQBot {
    public class QQBot {
        public void LoginWithPassword(UInt32 QQNumber, byte[] Password) {
            var Acc = AccountManager.GetNewAccount(QQNumber, Password);
            AccountManager.Login(Acc);
        }
    }
}
