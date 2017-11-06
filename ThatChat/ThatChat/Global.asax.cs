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

            // Creates a default conversation to load.  for testing.
            Account acct1 = new Account("face");
            Account acct2 = new Account("facade");
            Account acct3 = new Account("arcade");
            // Initializes the Conversation.  IMPORTANT.
            AppVars.Conversations.Val = new Catalogue();

            AppVars.Conversations.Val[0] = new Conversation("convo1");
            AppVars.Conversations.Val[0].Messages.Add(new Message(acct1, "hey guys"));
            AppVars.Conversations.Val[0].Messages.Add(new Message(acct3, "hi!"));
            AppVars.Conversations.Val[0].Messages.Add(new Message(acct2, "ugh, this guy."));
            AppVars.Conversations.Val[0].Messages.Add(new Message(acct3, "hey!  don't be rude!"));
            AppVars.Conversations.Val[0].Messages.Add(new Message(acct2, "I'll say whatever I want"));
            AppVars.Conversations.Val[0].Messages.Add(new Message(acct1, "ur meen :,,,c"));

            AppVars.Conversations.Val[1] = new Conversation("convo2");
            AppVars.Conversations.Val[1].Messages.Add(new Message(acct1, "hey gals"));

            AppVars.Conversations.Val[2] = new Conversation("convo3");
            AppVars.Conversations.Val[2].Messages.Add(new Message(acct1, "hey gays"));
  
            // Initializes Admin.  IMPORTANT.
            AppVars.Admin.Val = new Account("GOD");
            // Initializes Users.  IMPORTANT.
            AppVars.Users.Val = new ConcurrentDictionary<string, User>();
        }
    }
}