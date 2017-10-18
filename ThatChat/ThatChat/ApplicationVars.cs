using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ThatChat
{
    public class ApplicationVars
    {
        public static ApplicationVar<List<Conversation>> Conversations 
            = new ApplicationVar<List<Conversation>>("conversations");

        public class ApplicationVar<T>
        {
            private string key;
            private HttpApplicationState context = HttpContext.Current.Application;

            public ApplicationVar(string key)
            {
                this.key = key;
            }

            public T Val
            {
                get
                {
                    T output;

                    context.Lock();
                    output = (T)context[key];
                    context.UnLock();

                    return output;
                }
                set
                {
                    context.Lock();
                    context[key] = value;
                    context.UnLock();
                }
            }
        }
    }
}