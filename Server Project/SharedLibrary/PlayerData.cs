using System.Collections.Generic;
using System.Linq;

namespace CardGame.Data
{
    [System.Serializable]
    public class PlayerData
    {
        public List<CardData> Deck;
        public HealthData HealthData;

        public PlayerData(CardData[] deck, int maxHealth)
        {
            Deck = deck.ToList();
            HealthData = new HealthData(maxHealth);
        }

        public bool HasCards() => Deck?.Count > 1;
    }
}
