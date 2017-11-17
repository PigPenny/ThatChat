using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;

namespace ThatChat
{
    /// <summary>
    /// Represents one conversation.  Contains messages and users.
    /// </summary>
    public class Conversation
    {
        private Mutex messageAccess;
        private Mutex userAccess;

        // The messages sent over the course of this Conversation.
        private List<Message> messages;

        /// <summary>
        /// The name of this Conversation.
        /// </summary>
        public string Name { get; private set; }

        private HashSet<User> users;

        /// <summary>
        /// Purpose:  Instantiates an object of the Conversation class.
        /// Author:   Andrew Busto
        /// Date:     October 17, 2017
        /// </summary>
        /// <param name="name"> The name of this Conversation. </param>
        public Conversation(string name)
        {
            this.Name = name;
            users = new HashSet<User>();
            messages = new List<Message>();

            messageAccess = new Mutex();
            userAccess = new Mutex();
        }

        public void addUser(User user)
        {
            userAccess.WaitOne();
            users.Add(user);
            userAccess.ReleaseMutex();
        }

        public void removeUser(User user)
        {
            userAccess.WaitOne();
            users.Remove(user);
            userAccess.ReleaseMutex();
        }

        public void forAllMessages(Action<Message> act)
        {
            messageAccess.WaitOne();

            foreach (Message msg in messages)
                act(msg);

            messageAccess.ReleaseMutex();
        }

        public void addMessage(Message msg)
        {
            messageAccess.WaitOne();
            messages.Add(msg);
            messageAccess.ReleaseMutex();
        }

        /// <summary>
        /// Sends a message to all users, and stores it for future users.
        /// </summary>
        /// <param name="msg"> The Message being sent. </param>
        /// <param name="hub"> The ChatHub with which the Message is sent. </param>
        public void broadcast(Message msg, ChatHub hub)
        {
            messageAccess.WaitOne();
            messages.Add(msg);

            userAccess.WaitOne();
            foreach (User user in users)
                hub.SendTo(msg, user.Client);

            userAccess.ReleaseMutex();
            messageAccess.ReleaseMutex();
        }
    }
}