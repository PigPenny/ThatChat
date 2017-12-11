using System;
using System.Collections.Generic;
using System.Threading;

namespace ThatChat
{
    /// <summary>
    /// Holds information about a user.
    /// </summary>
    public class Account
    {
        // invalidCount - Keeps track of the number of invalid accounts
        // accntCount   - Keeps track of the number of accounts
        // Both of the above refer to all that have been created, not just those active.
        private static int invalidCount = 0;
        private static int accntCount = 0;
        private static HashSet<string> namesInUse;

        private const int MAX_NAME_LENGTH = 64;

        /// <summary>
        /// The user's name.
        /// </summary>
        public string Name {
            get => name;
            private set
            {
                name = value;
                namesInUse.Add(name);
            }
        }
        private string name;

        /// <summary>
        /// True if this account is currently in use, false otherwise.
        /// </summary>
        public bool Active { get; private set; }

        /// <summary>
        /// This Accounts unique identifier.
        /// </summary>
        public int Id {
            get
            {
                return id;
            }
        }
        private int id;

        static Account()
        {
            namesInUse = new HashSet<string>();
        }

        /// <summary>
        /// Purpose:  Instantiates an object of the Account class.
        /// Author:   Andrew Busto
        /// Date:     October 17, 2017
        /// </summary>
        /// <param name="name"> The username associated with this Account </param>
        public Account(string name)
        {
            Active = true;

            if (((object)name) == null)
                name = "";

            applyName(name.Trim());

            id = Interlocked.Increment(ref accntCount);
        }

        private void applyName(string name)
        {
            if (name.Length > MAX_NAME_LENGTH)
                name = "";
                
            // Checks to make sure that the given name is valid.
            // If it isn't a different one will be assigned.
            if (validName(name))
            {
                Name = name;
            }
            else
            {
                string hexAppend = 
                    String.Format("{0:X}", Interlocked.Increment(ref invalidCount));
                applyName(name + hexAppend);
            }
        }

        /// <summary>
        /// Purpose:  Generates an automatic name.
        ///           To be used when the client fails to supply a valid one.
        /// Author:   Andrew Busto
        /// Date:     October 31, 2017
        /// </summary>
        /// <returns> An auto generated name. </returns>
        private string generateName(int inv)
        {
            return "Invalid name #" + inv;
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
            return !name.Equals("") && !namesInUse.Contains(name);
        }

        /// <summary>
        /// Purpose:  Deactivates an account.
        /// Author:   Chandu Dissanayake
        /// Date:     November 17, 2017
        /// </summary>
        public void deactivate()
        {
            namesInUse.Remove(Name);
            Active = false;
        }
    }
}