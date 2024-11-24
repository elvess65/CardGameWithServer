package com.cardgame.sdk.data;

import android.util.Log;
import com.google.gson.Gson;
import com.cardgame.sdk.HttpUtils;

public class RemoteDataProvider implements IDataProvider {
    @Override
    public PlayerData GetPlayer(int deckSize, boolean isEnemy)
    {
        String url = DataController.URL + "/player/" + deckSize + "/" + isEnemy;

        try {

            String response = HttpUtils.sendGetRequest(url);
            Gson gson = new Gson();

            return gson.fromJson(response, PlayerData.class);
        }
        catch (Exception e)
        {
            Log.e("[SDK]", e.getMessage());
        }

        return null;
    }

    @Override
    public void IncreaseCardPopularity(int dataBaseID) {
        try {

            String url = DataController.URL + "/player/increasePopularity/" + dataBaseID;
            HttpUtils.sendGetRequest(url);
        }
        catch (Exception e)
        {
            Log.e("[SDK]", e.getMessage());
        }
    }
}
