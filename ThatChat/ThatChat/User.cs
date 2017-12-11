using System.Threading;
using System.Timers;
using System;
using System.Collections.Generic;

namespace ThatChat
{
    /// <summary>
    /// Used to represent one active user.  
    /// Mostly composed of a "client" (A connection) and an Account.
    /// </summary>
    public class User
    {
        // lifeTime     - The amount of time before a client will be pinged.
        // responseTime - The amount of time after a ping before a 
        //                user will be removed.
        private double lifeTime = 5000;
        private double responseTime = 5000;

        // pingTrigger  - The timer that pings the client.
        // delTrigger   - The timer that deletes the client.
        private System.Timers.Timer pingTrigger;
        private System.Timers.Timer delTrigger;

        // The connection string to the client.
        private string connectString;

        // Controls access to this user's conversation.
        private Mutex convoAccess;

        /// <summary>
        /// The account associated with this user.
        /// </summary>
        public Account Accnt { get; set; }

        /// <summary>
        /// The object connecting this application to the user.
        /// </summary>
        public dynamic Client { get; private set; }

        /// <summary>
        /// The name of this User (equivalent to User.Accnt.Name)
        /// </summary>
        public string Name { get => Accnt.Name; }

        /// <summary>
        /// The unique identifier of this user's current account.
        /// </summary>
        public long Id { get => Accnt.Id; }

        /// <summary>
        /// The conversation this user is currently involved in.
        /// </summary>
        public Conversation Convo {
            get
            {
                return convo;
            }

            set
            {
                convoAccess.WaitOne();

                // Ensures this user is only ever in one conversation.
                if (((object) convo) != null)
                    convo.removeUser(this);

                convo = value;
                convo.addUser(this);

                convoAccess.ReleaseMutex();
            }
        }
        private Conversation convo;

        /// <summary>
        /// Purpose:  Instantiates an object of the User class.
        /// Author:   Connor Goudie/Chandu Dissanayake
        /// Date:     October 17, 2017
        /// </summary>
        /// <param name="client">The client to connect to (eg. Client.Caller).</param>
        public User(dynamic client, string connect)
        {
            init(client, "", connect);
        }

        /// <summary>
        /// Purpose:  Instantiates an object of the User class.
        /// Author:   Andrew Busto/Paul McCarlie
        /// Date:     October 29, 2017
        /// </summary>
        /// <param name="client">The client to connect to (eg. Client.Caller).</param>
        /// <param name="name">The name of this Client.</param>
        public User(dynamic client, string name, string connect)
        {
            init(client, name, connect);
        }

        /// <summary>
        /// Purpose:  Initializes the values in this User to valid states.
        /// Author:   Andrew Busto/Paul McCarlie
        /// Date:     October 30, 2017
        /// </summary>
        /// <param name="client">The client to connect to (eg. Client.Caller).</param>
        /// <param name="name">The name of this Client.</param>
        private void init(dynamic client, string name, string connect)
        {
            connectString = connect;

            Client = client;
            Accnt = new Account(name);
            convoAccess = new Mutex();

            prepTimer(out pingTrigger, lifeTime, pingClient);
            prepTimer(out delTrigger, responseTime, delUser);

            pingTrigger.Start();
        }

        /// <summary>
        /// Purpose:  Initializes a timer.
        /// Author:   Andrew Busto
        /// Date:     November 28, 2017
        /// </summary>
        /// <param name="timer"> The timer to be initialized. </param>
        /// <param name="time"> The amount of time before the elapsed event. </param>
        /// <param name="handler"> the handler for the elapsed event. </param>
        public void prepTimer(out System.Timers.Timer timer, 
            double time, ElapsedEventHandler handler)
        {
            timer = new System.Timers.Timer(time);
            timer.Elapsed += handler;
            timer.AutoReset = false;
        }

        /// <summary>
        /// Purpose:  Requests a response from this user's client.
        /// Author:   Andrew Busto
        /// Date:     November 28, 2017
        /// </summary>
        /// <param name="source"> Unused. </param>
        /// <param name="e"> Unused. </param>
        public void pingClient(Object source, ElapsedEventArgs e)
        {
            Client.ping();
            delTrigger.Start();
        }

        /// <summary>
        /// Purpose:  Deletes this user.
        /// Author:   Andrew Busto
        /// Date:     November 28, 2017
        /// </summary>
        /// <param name="source"> Unused. </param>
        /// <param name="e"> Unused. </param>
        public void delUser(Object source, ElapsedEventArgs e)
        {
            pingTrigger.Stop();
            Accnt.deactivate();

            // Informs all clients that this user's 
            // account has been deactivated.
            foreach (KeyValuePair<string, User> user in AppVars.Users.Val)
                user.Value.Client.deactivateUser(Id);

            // Removes this user from their current convo.
            if (((object)convo) != null)
                convo.removeUser(this);

            User usr;
            AppVars.Users.Val.TryRemove(connectString, out usr);
        }

        /// <summary>
        /// Purpose:  Called in response to a ping, prevents deletion.
        /// Author:   Andrew Busto
        /// Date:     November 28, 2017
        /// </summary>
        public void cancelDel()
        {
            delTrigger.Stop();
            pingTrigger.Start();
        }
    }
}