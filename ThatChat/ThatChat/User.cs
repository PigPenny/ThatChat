using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ThatChat
{
    public class User
    {
        public Account Accnt { get; set; }
        public dynamic Client { get; private set; }

        public User(dynamic client)
        {
            this.Client = client;
        }
    }
}