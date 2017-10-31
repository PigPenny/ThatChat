using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using System.Collections.Concurrent;
using Microsoft.AspNet.SignalR.Hubs;

namespace ThatChat
{
    /// <summary>
    /// Connects to users, sending and recieving messages/function calls.
    /// </summary>
    public class ChatHub : Hub
    {
        // The Conversation that all users are presently engaged in.
        private Conversation convo = AppVars.Conversation.Val;

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
                Message msg = new Message(users[Context.ConnectionId].Accnt, content);

                convo.broadcast(msg, this);
            } catch (KeyNotFoundException e)
            {
                convo.broadcast(new Message(god, "A user not in our system is trying to send a message.  Observe:"), this);
                convo.broadcast(new Message(god, e.Message), this);
            }
        }

        /// <summary>
        /// Purpose:  Sends a message (not a Message) to a specific client.
        /// Author:   Andrew Busto
        /// Date:     October 28, 2017
        /// </summary>
        /// <param name="name"> The name associated with the message. </param>
        /// <param name="message"> The text to be sent as a message. </param>
        /// <param name="client"> The client to send the message to. </param>
        public void SendTo(string name, string message, dynamic client)
        {
            client.broadcastMessage(name, message);
        }

        /// <summary>
        /// Purpose:  Initializes a client's screen and user info.
        ///             TODO: make this suck less.
        /// Author:   Andrew Busto
        /// Date:     October 28, 2017
        /// </summary>
        /// <param name="name"> The name that the client has chosen. </param>
        public void init(string name)
        {
            addUser(name);

            // Sends the client all the messages that have been sent in their absense.
            foreach (Message msg in AppVars.Conversation.Val.Messages)
                Clients.Caller.broadcastMessage(msg.Acct.Name, msg.Content);
        }

        /// <summary>
        /// Purpose:  Adds a user's information to all relevant lists.
        /// Author:   Andrew Busto
        /// Date:     October 30, 2017
        /// </summary>
        /// <param name="name"> The name of the user. </param>
        private void addUser(string name)
        {
            users[Context.ConnectionId] = new User(Clients.Caller, name);
            convo.broadcast(new Message(god, users[Context.ConnectionId].Name + " HAS JOINED YOUR GLORIOUS CHAT"), this);
            convo.Users.Add(users[Context.ConnectionId]);
        }
    }
}