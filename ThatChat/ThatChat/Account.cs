using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;

namespace ThatChat
{
    /// <summary>
    /// Holds information about a user.
    /// </summary>
    public class Account
    {
        private static Int32 invalidCount = 0;

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
            Active = true;

            // Checks to make sure that the given name is valid.
            // If it isn't a different one will be assigned.
            if (validName(name))
            {
                this.Name = name;
            }
            else
            {
                Interlocked.Increment(ref invalidCount);
                this.Name = "Invalid name #" + invalidCount;
            }
        }

        /// <summary>
        /// Purpose:  Determines if a name is valid.
        /// Author:   Andrew Busto
        /// Date:     October 31, 2017
        /// </summary>
        /// <param name="name"> The name to be validated. </param>
        /// <returns></returns>
        private bool validName(string name)
        {
            return !name.Equals("");
        }
    }
}