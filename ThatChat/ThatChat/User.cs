using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ThatChat
{
    public class User
    {
        public string Name { get; private set; }
        public bool Active { get; set; }

        public User(string name)
        {
            this.Name = name;
        }
    }
}