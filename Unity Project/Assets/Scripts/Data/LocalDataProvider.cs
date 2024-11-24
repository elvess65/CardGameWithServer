using System.Linq;
using UnityEngine;
using System.Collections.Generic;

namespace CardGame.Data
{
    /// <summary>
    /// Server simulation
    /// </summary>
    public class LocalDataProvider : IDataProvider
    {
        private const int HEALTH = 15;
        
        private static int CARD_ID = 0;
        private static int CARD_INSTANCE_ID = 0;
        
        private readonly List<CardData> m_cardsDB = new ()
        {
            new CardData(GetNextID(), 1, 1, 1, 1, 10, 0, "Harry P."),
            new CardData(GetNextID(), 2, 2, 2, 2, 9, 1, "Hermione G."),
            new CardData(GetNextID(), 3, 3, 3, 3, 7, 0, "Ron W."),
            new CardData(GetNextID(), 1, 1, 4, 4, 10, 1, "Albus D."),
            new CardData(GetNextID(), 2, 2, 5, 5, 8, 1, "Severus S."),
            new CardData(GetNextID(), 3, 3, 6, 6, 10, 0, "L. Voldemort"),
            new CardData(GetNextID(), 1, 1, 7, 7, 9, 0, "Sirius B."),
            new CardData(GetNextID(), 2, 2, 8, 8, 6, 1, "Draco M."),
            new CardData(GetNextID(), 3, 3, 9, 9, 7, 1, "Luna L."),
            new CardData(GetNextID(), 1, 1, 10, 10, 9, 0, "Bellatrix L."),
            new CardData(GetNextID(), 2, 2, 11, 11, 8, 1, "Neville L."),
            new CardData(GetNextID(), 3, 3, 12, 12, 7, 0, "Ginny W."),
            new CardData(GetNextID(), 1, 1, 13, 13, 9, 1, "Minerva McG."),
            new CardData(GetNextID(), 2, 2, 14, 14, 6, 0, "Rubeus H."),
            new CardData(GetNextID(), 3, 3, 1, 15, 5, 1, "Dolores U.")
        };
        
        public PlayerData GetPlayer(int deckSize, bool isEnemy)
        {
            var deck = GetDeck(deckSize, isEnemy);
            var health = GetHealth();
            
            return new PlayerData(deck, health);
        }

        public void IncreaseCardPopularity(int dataBaseID)
        {
            var entry = m_cardsDB.FirstOrDefault(c => c.DataBaseId == dataBaseID);
            if (entry == null)
                return;
            
            entry.Popularity++;
        }

        private CardData[] GetDeck(int deckSize, bool isEnemy) => isEnemy ? BuildRandomDeck(deckSize) : BuildPopularDeck(deckSize);

        private int GetHealth() => HEALTH;
        
        private static int GetNextID() => CARD_ID++;
        private static int GetNextInstanceID() => CARD_INSTANCE_ID++;

        private CardData[] BuildRandomDeck(int deckSize)
        {
            List<CardData> deck = new();
            for (int i = 0; i < deckSize; i++)
            {
                var card = m_cardsDB[Random.Range(0, m_cardsDB.Count)];
                var cardInstance = new CardData(card, GetNextInstanceID()); 
                deck.Add(cardInstance);
            }
            
            return deck.ToArray();
        }

        private CardData[] BuildPopularDeck(int deckSize)
        {
            var deck = m_cardsDB.OrderByDescending(card => card.Popularity).Take(deckSize).ToArray();
            var instanceDeck = new CardData[deck.Length];
            
            for (var i = 0 ; i < deck.Length; i++)
            {
                instanceDeck[i] = new CardData(deck[i], GetNextInstanceID());
            }
            return instanceDeck;
        }
    }
}
