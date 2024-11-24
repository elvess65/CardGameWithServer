package com.cardgame.sdk.data;

import java.util.ArrayList;
import java.util.Comparator;
import java.util.List;
import java.util.Random;

public class LocalDataProvider implements IDataProvider {
    private static final int HEALTH = 20;

    private static int CARD_ID = 0;
    private static int CARD_INSTANCE_ID = 0;

    private final ArrayList<CardData> cardsDB = new ArrayList<>();

    public LocalDataProvider() {
        cardsDB.add(new CardData(getNextID(), 1, 1, 1, 1, 10, 0, "Harry P."));
        cardsDB.add(new CardData(getNextID(), 2, 2, 2, 2, 9, 1, "Hermione G."));
        cardsDB.add(new CardData(getNextID(), 3, 3, 3, 3, 7, 0, "Ron W."));
        cardsDB.add(new CardData(getNextID(), 1, 1, 4, 4, 10, 1, "Albus D."));
        cardsDB.add(new CardData(getNextID(), 2, 2, 5, 5, 8, 1, "Severus S."));
        cardsDB.add(new CardData(getNextID(), 3, 3, 6, 6, 10, 0, "L. Voldemort"));
        cardsDB.add(new CardData(getNextID(), 1, 1, 7, 7, 9, 0, "Sirius B."));
        cardsDB.add(new CardData(getNextID(), 2, 2, 8, 8, 6, 1, "Draco M."));
        cardsDB.add(new CardData(getNextID(), 3, 3, 9, 9, 7, 1, "Luna L."));
        cardsDB.add(new CardData(getNextID(), 1, 1, 10, 10, 9, 0, "Bellatrix L."));
        cardsDB.add(new CardData(getNextID(), 2, 2, 11, 11, 8, 1, "Neville L."));
        cardsDB.add(new CardData(getNextID(), 3, 3, 12, 12, 7, 0, "Ginny W."));
        cardsDB.add(new CardData(getNextID(), 1, 1, 13, 13, 9, 1, "Minerva McG."));
        cardsDB.add(new CardData(getNextID(), 2, 2, 14, 14, 6, 0, "Rubeus H."));
        cardsDB.add(new CardData(getNextID(), 3, 3, 1, 15, 5, 1, "Dolores U."));
    }

    @Override
    public PlayerData GetPlayer(int deckSize, boolean isEnemy)
    {
        var deck = getDeck(deckSize, isEnemy);
        var health = getHealth();

        return new PlayerData(deck, health);
    }

    @Override
    public void IncreaseCardPopularity(int dataBaseID) {
        for (CardData card : cardsDB) {
            if (card.DataBaseId == dataBaseID) {
                card.Popularity++;
                return;
            }
        }
    }


    private CardData[] getDeck(int deckSize, boolean isEnemy) {
        return isEnemy ? buildRandomDeck(deckSize) : buildPopularDeck(deckSize);
    }

    private int getHealth() {
        return HEALTH;
    }

    private static int getNextID() {
        return CARD_ID++;
    }
    private static int GetNextInstanceID() { return CARD_INSTANCE_ID++; }


    private CardData[] buildRandomDeck(int deckSize) {
        List<CardData> deck = new ArrayList<>();
        Random random = new Random();

        for (int i = 0; i < deckSize; i++) {
            CardData card = cardsDB.get(random.nextInt(cardsDB.size()));
            CardData cardInstance = new CardData(card, GetNextInstanceID());
            deck.add(cardInstance);
        }

        return deck.toArray(new CardData[0]);
    }

    private CardData[] buildPopularDeck(int deckSize) {
        List<CardData> sortedCards = new ArrayList<>(cardsDB);
        sortedCards.sort(Comparator.comparingInt(CardData::getPopularity).reversed());
        CardData[] deck = sortedCards.stream().limit(deckSize).toArray(CardData[]::new);

        var instanceDeck = new CardData[deck.length];

        for (var i = 0 ; i < deck.length; i++)
        {
            instanceDeck[i] = new CardData(deck[i], GetNextInstanceID());
        }

        return instanceDeck;
    }
}
