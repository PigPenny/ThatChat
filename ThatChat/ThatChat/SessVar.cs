using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ThatChat
{
    public class SessVar<T>
    {
        private string key;
        private static System.Web.SessionState.HttpSessionState context
            = HttpContext.Current.Session;

        public static SessVar<User> Me { get; private set; }
            = new SessVar<User>("me");

        public SessVar(string key)
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
                context[key] = value;
            }
        }
    }
}