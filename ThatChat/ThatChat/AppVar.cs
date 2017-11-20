using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ThatChat
{
    /// <summary>
    /// A wrapper for an application variable that makes it strictly typed and threadsafe.
    /// </summary>
    /// <typeparam name="T"> The type of the variable. </typeparam>
    public class AppVar<T>
    {
        // The key used in the ApplicationState's indexer.
        private string key;

        // All keys that have been used.
        private static HashSet<String> keys = new HashSet<string>();

        // The ApplicationState associated with this application.
        private static HttpApplicationState context 
            = HttpContext.Current.Application;

        /// <summary>
        /// Purpose:  Instantiates an object of the AppVar class.
        /// Author:   Andrew Busto
        /// Date:     October 17, 2017
        /// </summary>
        /// <param name="key"> The key that will be used in Application's indexer. </param>
        public AppVar(string key)
        {
            if (keys.Contains(key))
                throw new ArgumentException("Key already in use.");

            keys.Add(key);
            this.key = key;
        }

        /// <summary>
        /// Purpose:  Gets and sets the appropriate value in Application.
        /// Author:   Andrew Busto
        /// Date:     October 17, 2017
        /// </summary>
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