using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;

namespace ThatChat
{
    /// <summary>
    /// Holds and allows access to many conversations.
    /// </summary>
    public class Catalogue
    {
        // Holds the conversations.
        private ConcurrentDictionary<int, Conversation> conversations;

        /// <summary>
        /// Purpose:  Instantiates an object of the Catalogue class.
        /// Author:   Chandu Dissanayake/Connor Goudie
        /// Date:     November 6, 2017
        /// </summary>
        public Catalogue()
        {
            conversations = new ConcurrentDictionary<int, Conversation>();
        }

        /// <summary>
        /// Purpose:  Add a conversation to the catalogue.
        /// Author:   Chandu Dissanayake/Connor Goudie
        /// Date:     November 6, 2017
        /// </summary>
        /// <param name="convo"> The conversation to be added. </param>
        /// <returns> The key corresponding to the conversation. </returns>
        public int addConversation(Conversation convo)
        {
            conversations.TryAdd(convo.Id, convo);
            return convo.Id;
        }

        /// <summary>
        /// Purpose:  Removes a conversation.
        /// Author:   Chandu Dissanayake/Connor Goudie
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
        /// Author:   Chandu Dissanayake/Connor Goudie
        /// Date:     November 11, 2017
        /// </summary>
        public ICollection<int> Keys
        {
            get => conversations.Keys;
        }

        /// <summary>
        /// Purpose:  Gets the conversation associated with the given key.
        /// Author:   Chandu Dissanayake/Connor Goudie
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