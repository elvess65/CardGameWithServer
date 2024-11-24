package com.cardgame.sdk.data;

import android.util.Log;
import com.google.gson.Gson;

public class DataController {

    private static final int CARD_SIZE = 7;

    private static DataRetriever dataRetriever;
    public static String URL;

    public void Init(boolean simulateServer, String url) {
        dataRetriever = new DataRetriever(simulateServer);
        UpdateUrl(url);
    }

    public void UpdateUrl(String url) {
        URL = url;
    }

    public String GetPlayerData(boolean isEnemy)
    {
        if (dataRetriever == null)
        {
            Log.e("[SDK]", "DataRetriever is not initialized");
            return null;
        }

        PlayerData data = dataRetriever.GetPlayer(CARD_SIZE, isEnemy);
        Gson gson = new Gson();

        return gson.toJson(data);
    }

    public void IncreaseCardPopularity(int dataBaseID)
    {
        if (dataRetriever == null)
        {
            Log.e("[SDK]", "DataRetriever is not initialized");
            return;
        }

        dataRetriever.IncreaseCardPopularity(dataBaseID);
    }
}
