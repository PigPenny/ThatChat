﻿using System;
using System.Collections.Generic;
using System.Threading;
using System.Timers;

namespace ThatChat
{
    /// <summary>
    /// Represents one conversation.  Contains messages and users.
    /// </summary>
    public class Conversation
    {
        // Holds the maximum name length.
        private const int MAX_NAME_LENGTH = 64;

        // Keeps track of the number of conversations that exist.
        private static int count = -1;
        public int Id { get; private set; }

        // Keeps track of all names currently in use.
        private static HashSet<string> namesInUse = new HashSet<string>();

        // Manage access to messages and users respectively.
        private Mutex messageAccess;
        private Mutex userAccess;

        // Used for deleting this conversation when it's inactive for too long.
        private double delTime = 600000;
        private System.Timers.Timer delTrigger;

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
            users = new HashSet<User>();
            messages = new List<Message>();

            messageAccess = new Mutex();
            userAccess = new Mutex();

            Id = Interlocked.Increment(ref count);

            delTrigger = new System.Timers.Timer(delTime);
            delTrigger.AutoReset = false;
            delTrigger.Elapsed += delete;
            delTrigger.Start();

            // Ensures no invalid names are given.
            name = name.Trim();
            if (name.Length == 0 || name.Length > MAX_NAME_LENGTH)
                throw new ArgumentException("Name invalid length.");
            if (namesInUse.Contains(name))
                throw new ArgumentException("Name already in use.");

            Name = name;
            namesInUse.Add(name);
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
            updateUserCount();
            userAccess.ReleaseMutex();

            delTrigger.Stop();
        }

        /// <summary>
        /// Purpose:  Updates all users with the correct 
        ///           user count for this conversation.
        /// Author:   Andrew Busto/Paul McCarlie
        /// Date:     November 24, 2017
        /// </summary>
        private void updateUserCount()
        {
            foreach (KeyValuePair<string, User> user2 in AppVars.Users.Val)
                user2.Value.Client.updateChatUserCount(Id, Name, this.users.Count);
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
            updateUserCount();
            userAccess.ReleaseMutex();

            // Starts countdown to deletion if this conversation is not in use.
            if (users.Count == 0)
                delTrigger.Start();
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
        /// <param name="msg"> The message to be added. </param>
        public void addMessage(Message msg)
        {
            messageAccess.WaitOne();
            messages.Add(msg);
            messageAccess.ReleaseMutex();
        }

        /// <summary>
        /// Purpose:  Sends a message to all users, and stores it for future users.
        /// Author:   Andrew Busto/Paul McCarlie
        /// Date:     November 17, 2017
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

        /// <summary>
        /// Purpose:  Removes this converation.
        /// Author:   Andrew Busto
        /// Date:     November 28, 2017
        /// </summary>
        /// <param name="source"> Unused. </param>
        /// <param name="e"> Unused. </param>
        public void delete(Object source, ElapsedEventArgs e)
        {
            AppVars.Conversations.Val.deleteConversation(this.Id);
            foreach (KeyValuePair<string, User> usr in AppVars.Users.Val)
                usr.Value.Client.removeChat(Id);
        }

        /// <summary>
        /// Purpose:  Gets the number of users in this Conversation.
        /// Author:   Andrew Busto/Paul McCarlie
        /// Date:     November 24, 2017
        /// </summary>
        /// <returns> The number of users in this Conversation. </returns>
        public int getNumberUsers()
        {
            return users.Count;
        }
    }
}