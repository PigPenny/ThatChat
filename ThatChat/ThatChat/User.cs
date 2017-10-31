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
        public string Name
        {
            get => Accnt.Name;
        }

        public User(dynamic client)
        {
            init(client, "dog");
        }

        public User(dynamic client, string name)
        {
            init(client, name);
        }

        private void init(dynamic client, string name)
        {
            this.Client = client;
            this.Accnt = new Account(name);
        }
    }
}