using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using System.Collections.Concurrent;
using Microsoft.AspNet.SignalR.Hubs;

namespace ThatChat
{
    public class ChatHub : Hub
    {
        private Conversation convo = AppVars.Conversation.Val;
        private ConcurrentDictionary<string, User> users = AppVars.Users.Val;
        private Account god = AppVars.Admin.Val;

        public void send(string name, string content)
        {
            try
            {
                Message msg = new Message(users[Context.ConnectionId].Accnt, content);

                convo.broadcast(msg, this);
            } catch (KeyNotFoundException e)
            {
                convo.broadcast(new Message(god, "A user not in our system is trying to send a message.  Observe:"), this);
                convo.broadcast(new Message(god, e.Message), this);
            }
        }

        public void SendTo(string name, string message, dynamic client)
        {
            client.broadcastMessage(name, message);
        }

        public void init()
        {
            addUser("name");

            foreach (Message msg in AppVars.Conversation.Val.Messages)
                Clients.Caller.broadcastMessage(msg.Acct.Name, msg.Content);
        }

        private void addUser(string name)
        {
            users[Context.ConnectionId] = new User(Clients.Caller, name);
            convo.broadcast(new Message(god, users[Context.ConnectionId].Name + " HAS JOINED YOUR GLORIOUS CHAT"), this);
            convo.Users.Add(users[Context.ConnectionId]);
        }
    }
}