namespace CardGame.Data
{
    public interface IDataProvider
    {
        PlayerData GetPlayer(int deckSize, bool isEnemy);
        void IncreaseCardPopularity(int dataBaseID);
    }
}
