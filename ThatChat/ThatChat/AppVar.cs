using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ThatChat
{
    public class AppVar<T>
    {
        private string key;
        private static HttpApplicationState context 
            = HttpContext.Current.Application;

        public static AppVar<List<Conversation>> Conversations { get; private set; }
            = new AppVar<List<Conversation>>("conversations");

        public AppVar(string key)
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