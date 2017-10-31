using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ThatChat
{
    /// <summary>
    /// Holds information about a user.
    /// </summary>
    public class Account
    {
        /// <summary>
        /// The user's name.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// True if this account is currently in use, false otherwise.
        /// </summary>
        public bool Active { get; set; }

        /// <summary>
        /// Purpose:  Instantiates an object of the Account class.
        /// Author:   Andrew Busto
        /// Date:     October 17, 2017
        /// </summary>
        /// <param name="name"></param>
        public Account(string name)
        {
            this.Name = name;
            Active = true;
        }
    }
}