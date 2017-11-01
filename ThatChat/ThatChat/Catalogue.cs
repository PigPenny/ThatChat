using System;
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
        private List<Conversation> convos;

        public Conversation this[int i]
        {
            get { return convos[i]; }
            set { convos[i] = value; }
        }


        public Catalogue()
        {
            convos = new List<Conversation>();
        }
    }
}