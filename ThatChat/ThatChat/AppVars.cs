using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ThatChat
{
    public class AppVars
    {
        public static AppVar<List<Conversation>> Conversations{get; private set;}
            = new AppVar<List<Conversation>>("conversations");
    }
}