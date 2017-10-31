using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;
using System.Collections.Concurrent;

namespace ThatChat
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            // Code that runs on application startup
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            Account acct1 = new Account("face");
            Account acct2 = new Account("facade");
            Account acct3 = new Account("arcade");
            AppVars.Conversation.Val = new Conversation("Convo1");
            AppVars.Conversation.Val.Messages.Add(new Message(acct1, "hey guys"));
            AppVars.Conversation.Val.Messages.Add(new Message(acct3, "hi!"));
            AppVars.Conversation.Val.Messages.Add(new Message(acct2, "ugh, this guy."));
            AppVars.Conversation.Val.Messages.Add(new Message(acct3, "hey!  don't be rude!"));
            AppVars.Conversation.Val.Messages.Add(new Message(acct2, "I'll say whatever I want"));
            AppVars.Conversation.Val.Messages.Add(new Message(acct1, "ur meen :,,,c"));

            AppVars.Admin.Val = new Account("GOD");

            AppVars.Users.Val = new ConcurrentDictionary<string, User>();
        }
    }
}