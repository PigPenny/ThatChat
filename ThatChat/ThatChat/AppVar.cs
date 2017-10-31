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

        public AppVar(string key)
        {
            this.key = key;
        }

        public T Val
        {
            get
            {
                return (T)context[key];
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