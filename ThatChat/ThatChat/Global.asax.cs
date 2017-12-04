using System;
using System.Web;
using System.Web.Optimization;
using System.Web.Routing;
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

            // Initializes the Conversations.  IMPORTANT.
            AppVars.Conversations.Val = new Catalogue();

            AppVars.Conversations.Val.addConversation(new Conversation("convo0"));
            AppVars.Conversations.Val[0].addMessage(new Message(acct1, "hey guys"));
            AppVars.Conversations.Val[0].addMessage(new Message(acct3, "hi!"));
            AppVars.Conversations.Val[0].addMessage(new Message(acct2, "ugh, this guy."));
            AppVars.Conversations.Val[0].addMessage(new Message(acct3, "hey!  don't be rude!"));
            AppVars.Conversations.Val[0].addMessage(new Message(acct2, "I'll say whatever I want"));
            AppVars.Conversations.Val[0].addMessage(new Message(acct1, "ur meen :,,,c"));

            AppVars.Conversations.Val.addConversation(new Conversation("convo1"));
            AppVars.Conversations.Val[1].addMessage(new Message(acct1, "hey gals"));

            AppVars.Conversations.Val.addConversation(new Conversation("convo2"));
            AppVars.Conversations.Val[2].addMessage(new Message(acct1, "hey gents"));
            AppVars.Conversations.Val[2].addMessage(new Message(acct2, "and ladies!"));

            acct1.deactivate();
            acct2.deactivate();
            acct3.deactivate();

            // Initializes Admin.  IMPORTANT.
            AppVars.Admin.Val = new Account("GOD");

            // Initializes Users.  IMPORTANT.
            AppVars.Users.Val = new ConcurrentDictionary<string, User>();
        }
    }
}