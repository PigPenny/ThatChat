using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ThatChat
{
    public class Message
    {
        public Account Acct { get; private set; }
        public string Content { get; private set; }

        public Message(Account acct, string content)
        {
            this.Acct = acct;
            this.Content = content;
        }
    }
}