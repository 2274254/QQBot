using System;
using System.Collections.Generic;
using System.Text;
using ChatBot.Account.Interface;
using ChatBot.Network.Interface;

namespace ChatBot.Network {
    class AsyncNetworkManager {
        Dictionary<IChatBotAccount, IAsyncNetworkClient> AccountClientMap = new Dictionary<IChatBotAccount, IAsyncNetworkClient>();
        public TypeReceiveCallback Dispatcher { private get; set; }

        public void BindAccountWithClient(IChatBotAccount Account, IAsyncNetworkClient Client) {
            if (!this.AccountClientMap.ContainsKey(Account)) {
                lock (this.AccountClientMap) {
                    this.AccountClientMap.Add(Account, Client);
                    this.AccountClientMap[Account].ReceiveCallBack = this.Dispatcher;
                    this.AccountClientMap[Account].Begin(); 
                }
            } else {
                lock (this.AccountClientMap) {
                    this.AccountClientMap[Account].Stop();
                    this.AccountClientMap[Account] = Client;
                    this.AccountClientMap[Account].ReceiveCallBack = this.Dispatcher;
                    this.AccountClientMap[Account].Begin();
                }
            }
        }

        public void RemoveAccount(IChatBotAccount Account) {
            if (this.AccountClientMap.ContainsKey(Account)) {
                lock (this.AccountClientMap) {
                    this.AccountClientMap[Account].Stop();
                    this.AccountClientMap.Remove(Account);
                }
            }
        }
    }
}
