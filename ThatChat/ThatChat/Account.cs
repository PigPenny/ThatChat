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
        // A counter keeping track of the number of times an 
        // Account has been instantiated with invalid inputs.
        private static Int32 invalidCount = 0;

        private static Int32 accntCount = 0;

        /// <summary>
        /// The user's name.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// True if this account is currently in use, false otherwise.
        /// </summary>
        public bool Active { get; set; }

        public int Id { get; private set; }

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
            }else
            {
                Interlocked.Increment(ref invalidCount);
                this.Name = generateName();
            }

            Id = accntCount;
            Interlocked.Increment(ref accntCount);
        }

        /// <summary>
        /// Purpose:  Generates an automatic name.
        ///           To be used when the client fails to supply a valid one.
        /// Author:   Andrew Busto
        /// Date:     October 31, 2017
        /// </summary>
        /// <returns> An auto generated name. </returns>
        private string generateName()
        {
            return "Invalid name #" + invalidCount;
        }

        /// <summary>
        /// Purpose:  Determines if a name is valid.
        /// Author:   Andrew Busto
        /// Date:     October 31, 2017
        /// </summary>
        /// <param name="name"> The name to be validated. </param>
        /// <returns> True if name is valid, false otherwise. </returns>
        private bool validName(string name)
        {
            return !name.Equals("");
        }
    }
}