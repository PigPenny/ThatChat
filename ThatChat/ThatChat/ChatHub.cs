using System.Collections.Generic;
using Microsoft.AspNet.SignalR;
using System.Collections.Concurrent;
using System.Diagnostics;
using System;

namespace ThatChat
{
    /// <summary>
    /// Connects to users, sending and recieving messages/function calls.
    /// </summary>
    public class ChatHub : Hub
    {
        // The catalogue to access conversations from.
        private Catalogue catalogue = AppVars.Conversations.Val;

        // All users that have ever connected.
        private ConcurrentDictionary<string, User> users = AppVars.Users.Val;

        // The Admin bot that let's us know when stuff happens.
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
            }
            catch (KeyNotFoundException e)
            {
                Debug.Print(e.Message);
            }
            catch (NullReferenceException e)
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
        public void sendTo(Message msg, dynamic client)
        {
            try
            {
                client.broadcastMessage(msg.Acct.Name, msg.Content, msg.Acct.Id, msg.Acct.Active);
            }
            catch (NullReferenceException e)
            {
                Debug.Print(e.Message);
            }
            
        }

        /// <summary>
        /// Purpose:  Initializes a client's screen and user info.
        /// Author:   Connor Goudie
        /// Date:     October 28, 2017
        /// </summary>
        /// <param name="name"> The name that the client has chosen. </param>
        public void init()
        {
            try
            {
                // Sends the client all the messages that have been sent in their absense.
                users[Context.ConnectionId].Convo.forAllMessages(updateCaller);
            } catch (NullReferenceException e)
            {
                Debug.Print(e.Message);
            }
        }

        /// <summary>
        /// Purpose:  Sends a message to the person making this call.
        /// Author:   Andrew Busto
        /// Date:     November 18, 2017
        /// </summary>
        /// <param name="msg"></param>
        protected void updateCaller(Message msg)
        {
            sendTo(msg, Clients.Caller);
        }

        /// <summary>
        /// Purpose:  Adds a user's information to all relevant lists.
        /// Author:   Andrew Busto/Connor Goudie
        /// Date:     October 30, 2017
        /// </summary>
        /// <param name="name"> The name of the user. </param>
        public void addUser(string name)
        {
            string connect = Context.ConnectionId;
            if (!users.Keys.Contains(connect))
            {
                users[connect]
                    = new User(Clients.Caller, name, connect);
            }
            else
            {
                users[connect].Accnt = new Account(name);
            }
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
                Clients.Caller.displayName(name);
            }
            catch (KeyNotFoundException e)
            {
                Debug.Print(e.Message);
            }
        }

        /// <summary>
        /// Purpose:  Deactivates an account.
        /// Author:   Chandu Dissanayake
        /// Date:     November 6, 2017
        /// </summary>
        /// <param name="acct"></param>
        private void deactivate(Account acct)
        {
            Clients.All.deactivateUser(acct.Id);
            acct.deactivate();
        }

        /// <summary>
        /// Purpose:  Selects a chat room from a given id.
        /// Author:   Andrew Busto
        /// Date:     October 31, 2017
        /// </summary>
        /// <param name="chatID"> The id/key used to access the conversation </param>
        public void selectChatRoom(int chatID)
        {
            try
            {
                users[Context.ConnectionId].Convo = catalogue[chatID];
            }
            catch (KeyNotFoundException e)
            {
                Debug.Print(e.Message);
            }
        }

        /// <summary>
        /// Purpose:  Fills the conversation list of a client.
        /// Author:   Paul McCarlie
        /// Date:     November 6, 2017
        /// </summary>
        public void populateChats()
        {
            foreach (int key in catalogue.Keys)
            {
                Clients.Caller.addChat(catalogue[key].Name, key, catalogue[key].getNumberUsers());
            }
        }

        /// <summary>
        /// Purpose:  Adds a new conversation to the catalogue.
        /// Author:   Andrew Busto
        /// Date:     November 15, 2017
        /// </summary>
        /// <param name="name"></param>
        public void addChat(string name)
        {
            try
            {
                int id = catalogue.addConversation(new Conversation(name));
                Clients.All.addChat(catalogue[id].Name, id, catalogue[id].getNumberUsers());
            }
            catch (NullReferenceException e)
            {
                Debug.Print(e.Message);
            }
            catch (ArgumentException e)
            {
                Debug.Print(e.Message);
            }
        }

        public void respond()
        {
            try
            {
                users[Context.ConnectionId].cancelDel();
            }
            catch (KeyNotFoundException e)
            {
                Debug.Print(e.Message);
            }
        }
    }
}