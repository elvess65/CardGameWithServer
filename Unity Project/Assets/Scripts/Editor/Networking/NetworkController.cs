using System.IO;
using System.Text;
using UnityEngine;
using UnityEditor;
using UnityEngine.Networking;
using Cysharp.Threading.Tasks;

namespace CardGame.Editor
{
    public class NetworkController 
    {
        private enum ERequestType { GET, POST, PUT }
        
        
        public static async UniTask<TextAsset> DownloadFileAsync(string url, string localDirectory, string fileName)
        {
            using var request = UnityWebRequest.Get(url);
            
            request.certificateHandler = new BypassCertificate();
            
            await request.SendWebRequest();
            
            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"Error downloading file: {request.error}");
                return null;
            }
            
            byte[] fileData = request.downloadHandler.data;
                
            var downloadPath = Path.Combine(Application.dataPath, localDirectory, fileName);
            var localPath = Path.Combine("Assets", localDirectory, fileName);
            
            if (File.Exists(localPath))
                AssetDatabase.DeleteAsset(localPath);

            await File.WriteAllBytesAsync(downloadPath, fileData);
            
            AssetDatabase.Refresh();
            return AssetDatabase.LoadAssetAtPath<TextAsset>(localPath);
        }
        
        public static async UniTask<T> Get<T>(string url) => await ExecuteRequest<T>(CreateRequest(url, ERequestType.GET));

        public static async UniTask<T> Post<T>(string url, object payload) => await ExecuteRequest<T>(CreateRequest(url, ERequestType.POST, payload));

        
        private static async UniTask<T> ExecuteRequest<T>(UnityWebRequest request)
        {
            await request.SendWebRequest();

            while (!request.isDone)
                await UniTask.Delay(10);
            
            return JsonUtility.FromJson<T>(request.downloadHandler.text);
        }
        
        private static UnityWebRequest CreateRequest(string url, ERequestType type, object data = null)
        {
            var request = new UnityWebRequest(url, type.ToString());
            request.certificateHandler = new BypassCertificate();
            
            if (data != null)
            {
                var bodyRaw = Encoding.UTF8.GetBytes(JsonUtility.ToJson(data));
                request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            }

            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            return request;
        }
        
        private class BypassCertificate : CertificateHandler
        {
            protected override bool ValidateCertificate(byte[] certificateData) => true;
        }
    }
}
