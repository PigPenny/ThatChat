﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ThatChat
{
    /// <summary>
    /// Holds and allows access to many conversations.
    /// NOT YET FULLY IMPLEMENTED, but will prevent non-threadsafe access and whatnot.
    /// it's not worth it right now to comment what's there, this isn't even being used yet.  lol.
    /// </summary>
    public class Catalogue
    {
        private ConcurrentDictionary<int, Conversation> conversations;

        public Catalogue()
        {
            conversations = new ConcurrentDictionary<int, Conversation>();
        }

        public Conversation this[int i]
        {
            get { return conversations[i]; }
            set { conversations[i] = value; }
        }
    }
}