using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace ThatChat
{
    public class ChatHub : Hub
    {
        public void Send(string name, string message)
        {
            // Call the broadcastMessage method to update clients.
            AppVar<int>.Conversation.Val.broadcast(new Message(new Account(name), message), this);
        }

        public void SendTo(string name, string message, dynamic client)
        {
            client.broadcastMessage(name, message);
        }

        public void init()
        {
            addUser();
            foreach (Message msg in AppVar<int>.Conversation.Val.Messages)
                Clients.Caller.broadcastMessage(msg.Acct.Name, msg.Content);
        }

        private void addUser()
        {
            AppVar<int>.Conversation.Val.broadcast(new Message(new Account ("GOD"), "A NEW USER HAS JOINED"), this);
            AppVar<int>.Conversation.Val.Users.Add(new User(Clients.Caller));
        }
    }
}