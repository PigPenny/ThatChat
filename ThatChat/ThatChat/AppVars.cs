using Microsoft.AspNet.SignalR.Hubs;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ThatChat
{
    public class AppVars
    {
        public static AppVar<Conversation> Conversation { get; set; }
            = new AppVar<Conversation>("conversation");

        public static AppVar<ConcurrentDictionary<string, User>> Users { get; set; }
            = new AppVar<ConcurrentDictionary<string, User>>("users");

        public static AppVar<Account> Admin { get; set; }
            = new AppVar<Account>("admin");
    }
}