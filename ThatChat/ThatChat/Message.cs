using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ThatChat
{
    public class Message
    {
        public User User { get; private set; }
        public string Content { get; private set; }

        public Message(User user, string content)
        {
            this.User = user;
            this.Content = content;
        }
    }
}