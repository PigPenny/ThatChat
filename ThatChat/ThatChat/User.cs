using System.Threading;

namespace ThatChat
{
    /// <summary>
    /// Used to represent one active user.  
    /// Mostly composed of a "client" (A connection) and an Account.
    /// </summary>
    public class User
    {
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
        public User(dynamic client)
        {
            init(client, "");
        }

        /// <summary>
        /// Purpose:  Instantiates an object of the User class.
        /// Author:   Andrew Busto/Paul McCarlie
        /// Date:     October 29, 2017
        /// </summary>
        /// <param name="client">The client to connect to (eg. Client.Caller).</param>
        /// <param name="name">The name of this Client.</param>
        public User(dynamic client, string name)
        {
            init(client, name);
        }

        /// <summary>
        /// Purpose:  Initializes the values in this User to valid states.
        /// Author:   Andrew Busto/Paul McCarlie
        /// Date:     October 30, 2017
        /// </summary>
        /// <param name="client">The client to connect to (eg. Client.Caller).</param>
        /// <param name="name">The name of this Client.</param>
        private void init(dynamic client, string name)
        {
            this.Client = client;
            this.Accnt = new Account(name);
            this.convoAccess = new Mutex();
        }
    }
}