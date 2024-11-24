package com.cardgame.sdk.data;

import java.util.ArrayList;
import java.util.Collections;

public class PlayerData {
    public ArrayList<CardData> Deck;
    public HealthData HealthData;

    public PlayerData(CardData[] deck, int maxHealth) {
        this.Deck = new ArrayList<>();
        Collections.addAll(this.Deck, deck);

        this.HealthData = new HealthData(maxHealth);
    }
}
