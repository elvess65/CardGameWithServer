package com.cardgame.sdk.data;

public class DataRetriever {
    private IDataProvider m_dataProvider;

    public DataRetriever(boolean simulateServer) {
        m_dataProvider = simulateServer ? new LocalDataProvider() : new RemoteDataProvider();
    }

    public PlayerData GetPlayer(int deckSize, boolean isEnemy) {
        return m_dataProvider.GetPlayer(deckSize, isEnemy);
    }

    public void IncreaseCardPopularity(int dataBaseID) {
        m_dataProvider.IncreaseCardPopularity(dataBaseID);
    }
}
