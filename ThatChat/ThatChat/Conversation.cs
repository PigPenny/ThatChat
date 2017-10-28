using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ThatChat
{
    public class Conversation
    {
        public string Name { get; private set; }

        public List<Message> Messages { get; private set; }

        public HashSet<User> Users { get; set; }

        public Conversation(string name)
        {
            this.Name = name;
            Users = new HashSet<User>();
            Messages = new List<Message>();
        }

        public void broadcast(Message msg, ChatHub hub)
        {
            Messages.Add(msg);
            foreach (User user in Users)
            {
                hub.SendTo(msg.Acct.Name, msg.Content, user.Client);
            }
        }
    }
}