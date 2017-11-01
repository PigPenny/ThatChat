using Microsoft.AspNet.SignalR.Hubs;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ThatChat
{
    /// <summary>
    /// Holds instances of the AppVar class so that they can be accessed by all.
    /// </summary>
    public class AppVars
    {
        /// <summary>
        /// The conversation that all users are currently to be connected to.
        /// </summary>
        public static AppVar<Conversation> Conversation { get; set; }
            = new AppVar<Conversation>("conversation");

        /// <summary>
        /// All users that have ever connected.
        /// </summary>
        public static AppVar<ConcurrentDictionary<string, User>> Users { get; set; }
            = new AppVar<ConcurrentDictionary<string, User>>("users");

        /// <summary>
        /// The administrative bot that makes announcements.
        /// </summary>
        public static AppVar<Account> Admin { get; set; }
            = new AppVar<Account>("admin");
    }
}