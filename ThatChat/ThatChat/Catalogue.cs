using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ThatChat
{
    public class Catalogue
    {
        private static Catalogue catalogue;

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