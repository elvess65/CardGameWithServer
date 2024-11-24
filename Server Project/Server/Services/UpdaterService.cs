using CardGame.Data;

namespace Server.Services;

public interface IUpdaterService
{
    IntWrapper GetVersion();
    IntWrapper IncrementVersion();
}

public class UpdaterService : IUpdaterService
{
    private static int m_currentFileVersion = -1;
    
    private const int DEFAULT_VERSION = 2;
    
    public UpdaterService()
    {
        if (m_currentFileVersion < 0)
            m_currentFileVersion = DEFAULT_VERSION;
    }

    public IntWrapper GetVersion() => new() { Value = m_currentFileVersion };

    public IntWrapper IncrementVersion() => new () { Value = ++m_currentFileVersion };
}