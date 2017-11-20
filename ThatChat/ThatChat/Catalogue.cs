using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;

namespace ThatChat
{
    /// <summary>
    /// Holds and allows access to many conversations.
    /// </summary>
    public class Catalogue
    {
        // Keeps track of the number of conversations that have been added.
        private int count = -1;

        // Holds the conversations.
        private ConcurrentDictionary<int, Conversation> conversations;

        /// <summary>
        /// Purpose:  Instantiates an object of the Catalogue class.
        /// Author:   Chandu Dissanayake
        /// Date:     November 6, 2017
        /// </summary>
        public Catalogue()
        {
            conversations = new ConcurrentDictionary<int, Conversation>();
        }

        /// <summary>
        /// Purpose:  Add a conversation to the catalogue.
        /// Author:   Chandu Dissanayake
        /// Date:     November 6, 2017
        /// </summary>
        /// <param name="convo"> The conversation to be added. </param>
        /// <returns> The key corresponding to the conversation. </returns>
        public int addConversation(Conversation convo)
        {
            conversations.TryAdd(Interlocked.Increment(ref count), convo);

            return count;
        }

        /// <summary>
        /// Purpose:  Removes a conversation.
        /// Author:   Chandu Dissanayake
        /// Date:     November 11, 2017
        /// </summary>
        /// <param name="id"> The key of the conversation. </param>
        public void deleteConversation(int id)
        {
            Conversation convo;
            conversations.TryRemove(id, out convo);
        }

        /// <summary>
        /// Purpose:  Gets the keys associated with all existing conversations.
        /// Author:   Chandu Dissanayake
        /// Date:     November 11, 2017
        /// </summary>
        public ICollection<int> Keys
        {
            get => conversations.Keys;
        }

        /// <summary>
        /// Purpose:  Gets the conversation associated with the given key.
        /// Author:   Chandu Dissanayake
        /// Date:     November 11, 2017
        /// </summary>
        /// <param name="key"> They key to access a conversation by. </param>
        /// <returns></returns>
        public Conversation this[int key]
        {
            get => conversations[key];
        }
    }
}