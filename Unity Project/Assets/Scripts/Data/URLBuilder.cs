using UnityEngine;

namespace CardGame.Data
{
    public static class URLBuilder
    {
        private const string DEFAULT_URL = "https://localhost:7191";
        private const string URL_KEY = "url";
        
        public static string GetUrl() => PlayerPrefs.GetString(URL_KEY, DEFAULT_URL);

        public static void SetNewUrl(string newUrl) => PlayerPrefs.SetString(URL_KEY, newUrl);   
    }
}
