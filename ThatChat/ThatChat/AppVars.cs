using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ThatChat
{
    public class AppVars
    {
        public static AppVar<Conversation> Conversation { get; set; }
             = new AppVar<Conversation>("conversation");
    }
}