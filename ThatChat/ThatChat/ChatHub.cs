using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using System.Collections.Concurrent;
using Microsoft.AspNet.SignalR.Hubs;
using System.Diagnostics;

namespace ThatChat
{
    /// <summary>
    /// Connects to users, sending and recieving messages/function calls.
    /// </summary>
    public class ChatHub : Hub
    {
        Catalogue catalogue = AppVars.Conversations.Val;
        // All users that have ever connected. (this needs addressing)
        private ConcurrentDictionary<string, User> users = AppVars.Users.Val;

        // The Admin bot that let's us know when stuff happens.  Thanks, god!
        private Account god = AppVars.Admin.Val;

        /// <summary>
        /// Purpose:  Sends a message from a user.
        /// Author:   Andrew Busto
        /// Date:     October 17, 2017
        /// </summary>
        /// <param name="content"> The text to be sent in the message. </param>
        public void send(string content)
        {
            try
            {
                User user = users[Context.ConnectionId];

                Message msg = new Message(user.Accnt, content);
 
                user.Convo.broadcast(msg, this);
            } catch (KeyNotFoundException e)
            {
                Debug.Print(e.Message);
            }
        }

        /// <summary>
        /// Purpose:  Sends a Message to a specific client.
        /// Author:   Andrew Busto
        /// Date:     October 28, 2017
        /// </summary>
        /// <param name="msg"> The message being sent. </param>
        /// <param name="client"> The client to recieve the message. </param>
        public void SendTo(Message msg, dynamic client)
        {
            client.broadcastMessage(msg.Acct.Name, msg.Content, msg.Acct.Id, msg.Acct.Active);
        }

        /// <summary>
        /// Purpose:  Initializes a client's screen and user info.
        ///             TODO: make this suck less.
        /// Author:   Andrew Busto
        /// Date:     October 28, 2017
        /// </summary>
        /// <param name="name"> The name that the client has chosen. </param>
        public void init()
        {
            // Sends the client all the messages that have been sent in their absense.
            users[Context.ConnectionId].Convo.forAllMessages(updateCaller);
        }

        public void updateCaller(Message msg)
        {
            SendTo(msg, Clients.Caller);
        }

        /// <summary>
        /// Purpose:  Adds a user's information to all relevant lists.
        /// Author:   Andrew Busto
        /// Date:     October 30, 2017
        /// </summary>
        /// <param name="name"> The name of the user. </param>
        public void addUser(string name)
        {
            users[Context.ConnectionId] = new User(Clients.Caller, name);
        }

        /// <summary>
        /// Purpose:  To be called by a client to change their name.
        /// Author:   Andrew Busto
        /// Date:     October 31, 2017
        /// </summary>
        /// <param name="name"> The user's new name. </param>
        public void setName(string name)
        {
            try
            {
                deactivate(users[Context.ConnectionId].Accnt);
                users[Context.ConnectionId].Accnt = new Account(name);
            } catch (KeyNotFoundException e)
            {
                Debug.Print(e.Message);
            }
        }

        private void deactivate(Account acct)
        {
            Clients.All.deactivateUser(acct.Id);
            acct.Active = false;
        }

        public void selectChatRoom(int chatID)
        {
            users[Context.ConnectionId].Convo = catalogue[chatID];
        }

        public void populateChats()
        {
            foreach (int key in catalogue.Keys)
                Clients.Caller.addChat(catalogue[key].Name, key);
        }

        public void addChat(string name)
        {
            int id = catalogue.addConversation(new Conversation(name));
            Clients.All.addChat(catalogue[id].Name, id);
        }
    }
}