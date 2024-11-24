using UnityEngine;
using CardGame.Natives;

namespace CardGame.Data
{
    /// <summary>
    /// Handles SDK Requests
    /// </summary>
    public class RemoteDataProvider : IDataProvider
    {
        public RemoteDataProvider(bool simulateServer) => AndroidWrapper.NativeDataController.Init(simulateServer, URLBuilder.GetUrl());

        public PlayerData GetPlayer(int deckSize, bool isEnemy)
        {
            var playerDataJson = AndroidWrapper.NativeDataController.GetRawPlayerData(isEnemy);
            return string.IsNullOrEmpty(playerDataJson) ? null : JsonUtility.FromJson<PlayerData>(playerDataJson);
        }

        public void IncreaseCardPopularity(int dataBaseID) => AndroidWrapper.NativeDataController.IncreaseCardPopularity(dataBaseID);
    }
}
