using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ThatChat
{
    public class ApplicationVars
    {
        private const string KEY_CONVERSATIONS = "conversations";

        private static HttpApplicationState context = HttpContext.Current.Application;

        private static object appGet(string key)
        {
            object output;

            context.Lock();
            output = context[KEY_CONVERSATIONS];
            context.UnLock();

            return output;
        }

        private static void appSet(string key, object val)
        {
            context.Lock();
            context[key] = val;
            context.UnLock();
        }

        public static List<Conversation> Conversations
        {
            get
            {
                return (List<Conversation>) appGet(KEY_CONVERSATIONS);
            }
            set
            {
                appSet(KEY_CONVERSATIONS, value);
            }
        }
    }
}