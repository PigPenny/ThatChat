using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace ThatChat
{
    /// <summary>
    /// Represents one conversation.  Contains messages and users.
    /// </summary>
    public class Conversation
    {
        // Manage access to messages and users respectively.
        private Mutex messageAccess;
        private Mutex userAccess;

        // The messages sent over the course of this Conversation.
        private List<Message> messages;

        /// <summary>
        /// The name of this Conversation.
        /// </summary>
        public string Name { get; private set; }

        // A set containing all of the users in this chat.
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

        /// <summary>
        /// Purpose:  Adds a new user to the conversation.
        /// Author:   Andrew Busto
        /// Date:     November 16, 2017
        /// </summary>
        /// <param name="user"> The user to be added. </param>
        public void addUser(User user)
        {
            userAccess.WaitOne();
            users.Add(user);
            userAccess.ReleaseMutex();
        }

        /// <summary>
        /// Purpose:  Removes a user from the conversation.
        /// Author:   Andrew Busto/Paul McCarlie
        /// Date:     November 16, 2017
        /// </summary>
        /// <param name="user"> The user to be removed. </param>
        public void removeUser(User user)
        {
            userAccess.WaitOne();
            users.Remove(user);
            userAccess.ReleaseMutex();
        }

        /// <summary>
        /// Purpose:  Performs an action with each user.
        /// Author:   Andrew Busto/Paul McCarlie
        /// Date:     November 17, 2017
        /// </summary>
        /// <param name="act"> The action to be performed. </param>
        public void forAllUsers(Action<User> act)
        {
            userAccess.WaitOne();

            foreach (User usr in users)
                act(usr);

            userAccess.ReleaseMutex();
        }

        /// <summary>
        /// Purpose:  Performs an action with each message.
        /// Author:   Andrew Busto/Paul McCarlie
        /// Date:     November 17, 2017
        /// </summary>
        /// <param name="act"> The action to be performed. </param>
        public void forAllMessages(Action<Message> act)
        {
            messageAccess.WaitOne();

            foreach (Message msg in messages)
                act(msg);

            messageAccess.ReleaseMutex();
        }

        /// <summary>
        /// Purpose:  Adds a message to this conversation.
        /// Author:   Andrew Busto/Paul McCarlie
        /// Date:     November 17, 2017
        /// </summary>
        /// <param name="msg"></param>
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
            if (msg.Content.Length > 0 && msg.Content.Length < 400)
            {
                addMessage(msg);

                userAccess.WaitOne();
                foreach (User user in users)
                    hub.sendTo(msg, user.Client);

                userAccess.ReleaseMutex();
            }

        }
    }
}