using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ThatChat
{
    public class Account
    {
        public string Name { get; private set; }
        public bool Active { get; set; }

        public Account(string name)
        {
            this.Name = name;
            Active = true;
        }
    }
}