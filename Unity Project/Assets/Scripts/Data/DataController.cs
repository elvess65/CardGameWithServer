namespace CardGame.Data
{
    public class DataController
    {
        private bool m_simulateNativeServer;
        private IDataProvider m_dataProvider;

        public DataController(bool isLocalMode, bool simulateNativeServer)
        {
            m_simulateNativeServer = simulateNativeServer;
            m_dataProvider = m_dataProvider = isLocalMode ? new LocalDataProvider() : new RemoteDataProvider(m_simulateNativeServer);    
        }
        
        public PlayerData GetPlayer(int deckSize, bool isEnemy) => m_dataProvider.GetPlayer(deckSize, isEnemy);

        public void IncreaseCardPopularity(int dataBaseID) => m_dataProvider.IncreaseCardPopularity(dataBaseID);
    }
}
