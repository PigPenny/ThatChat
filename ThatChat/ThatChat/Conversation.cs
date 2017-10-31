using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ThatChat
{
    /// <summary>
    /// Represents one conversation.  Contains messages and users.
    /// </summary>
    public class Conversation
    {
        /// <summary>
        /// The name of this Conversation.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// The messages sent over the course of this Conversation.
        /// </summary>
        public List<Message> Messages { get; private set; }

        /// <summary>
        /// The users presently involved in this Conversation.
        /// </summary>
        public HashSet<User> Users { get; set; }

        /// <summary>
        /// Purpose:  Instantiates an object of the Conversation class.
        /// Author:   Andrew Busto
        /// Date:     October 17, 2017
        /// </summary>
        /// <param name="name"> The name of this Conversation. </param>
        public Conversation(string name)
        {
            this.Name = name;
            Users = new HashSet<User>();
            Messages = new List<Message>();
        }

        /// <summary>
        /// Sends a message to all users, and stores it for future users.
        /// </summary>
        /// <param name="msg"> The Message being sent. </param>
        /// <param name="hub"> The ChatHub with which the Message is sent. </param>
        public void broadcast(Message msg, ChatHub hub)
        {
            Messages.Add(msg);
            foreach (User user in Users)
            {
                hub.SendTo(msg.Acct.Name, msg.Content, user.Client);
            }
        }
    }
}