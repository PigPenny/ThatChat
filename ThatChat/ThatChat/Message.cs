using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ThatChat
{
    /// <summary>
    /// Contains all information associated with a Message.
    /// </summary>
    public class Message
    {
        /// <summary>
        /// The Account of the user that sent this message.
        /// </summary>
        public Account Acct { get; private set; }

        /// <summary>
        /// The text being sent in this message.
        /// </summary>
        public string Content { get; private set; }

        /// <summary>
        /// Purpose:  Instantiates an object of the Message class.
        /// Author:   Andrew Busto
        /// Date:     October 18, 2017
        /// </summary>
        /// <param name="acct">The Account of the user sending this message.</param>
        /// <param name="content">The text being sent.</param>
        public Message(Account acct, string content)
        {
            this.Acct = acct;
            this.Content = content;
        }
    }
}