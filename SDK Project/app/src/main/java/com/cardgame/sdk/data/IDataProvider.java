package com.cardgame.sdk.data;

public interface IDataProvider {
    PlayerData GetPlayer(int deckSize, boolean isEnemy);
    void IncreaseCardPopularity(int dataBaseID);
}
