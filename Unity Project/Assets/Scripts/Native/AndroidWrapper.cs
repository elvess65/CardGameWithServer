using UnityEngine;
using Debug = UnityEngine.Debug;

namespace CardGame.Natives
{
    public static class AndroidWrapper
    {
        private const string UNITY_PLAYER = "com.unity3d.player.UnityPlayer";
        private const string BRIDGE_NAME = "com.cardgame.sdk.UnityBridge";
        
        private static AndroidJavaClass javaClass;
        private static AndroidJavaClass unityPlayerClass;
        private static AndroidJavaObject unityActivity;
        private static AndroidJavaObject bridgeInstance;
        
        public static NativeToastSender NativeToastSender = new();
        public static NativeDataController NativeDataController = new();
        
        public static void Init()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            
            unityPlayerClass = new AndroidJavaClass(UNITY_PLAYER);
            if (unityPlayerClass == null)
            {
                Debug.LogError("UnityPlayer not found");
                return;
            }

            unityActivity = unityPlayerClass.GetStatic<AndroidJavaObject>("currentActivity");
            if (unityActivity == null)
            {
                Debug.LogError("UnityActivity not found");
                return;
            }

            bridgeInstance = new AndroidJavaObject(BRIDGE_NAME);
            if (bridgeInstance == null)
            {
                Debug.LogError("Plugin Instance not found");
                return;
            }

            bridgeInstance.CallStatic("GetActivity", unityActivity);
            
#endif
        }
    }
    
    public abstract class NativeClassWrapper
    {
        protected abstract string ClassName { get; }

        protected AndroidJavaObject m_nativeInstance;
        
        protected bool IsCallAllowed()
        {
#if UNITY_ANDROID && !UNITY_EDITOR

            if (m_nativeInstance != null) 
                return true;
            
            m_nativeInstance = new(ClassName);

            if (m_nativeInstance == null)
            {
                Debug.LogError($"Instance of native class {ClassName} is not initialized");
                return false;
            }

            return true;
#endif

            return false;
        }
    }
    
    public class NativeToastSender : NativeClassWrapper
    {
        private const string CLASS_NAME = "com.cardgame.sdk.ToastSender";
        protected override string ClassName => CLASS_NAME;

        public void ShowToast(string text, bool isLongMessage = false)
        {
#if UNITY_ANDROID && !UNITY_EDITOR

            if (!IsCallAllowed())
                return;

            m_nativeInstance.Call("ShowToast", text, isLongMessage);

#else

            Debug.Log($"[NATIVE CALL] {text}. IsLong: {isLongMessage}");

#endif
        }
    }

    public class NativeDataController : NativeClassWrapper
    {
        protected override string ClassName => "com.cardgame.sdk.data.DataController";

        public void Init(bool simulateServer, string url)
        {
#if UNITY_ANDROID && !UNITY_EDITOR

            if (!IsCallAllowed())
            {
                Debug.Log("[SDK] [CLIENT] Call is not allowed");
                return;
            }

          
            m_nativeInstance.Call("Init", simulateServer, url);
            
#endif
        }

        public void UpdateUrl(string url)
        {
#if UNITY_ANDROID && !UNITY_EDITOR

            if (!IsCallAllowed())
            {
                Debug.Log("[SDK] [CLIENT] Call is not allowed");
                return;
            }

          
            m_nativeInstance.Call("UpdateUrl", url);
            
#endif
        }

        public string GetRawPlayerData(bool isEnemy)
        {
#if UNITY_ANDROID && !UNITY_EDITOR

            if (!IsCallAllowed())
            {
                Debug.Log("[SDK] [CLIENT] Call is not allowed");
                return string.Empty;
            }


            return m_nativeInstance.Call<string>("GetPlayerData", isEnemy);   
#else
            return string.Empty;
#endif

        }

        public void IncreaseCardPopularity(int dataBaseID)
        {
#if UNITY_ANDROID && !UNITY_EDITOR

            if (!IsCallAllowed())
            {
                Debug.Log("[SDK] [CLIENT] Call is not allowed");
                return;
            }


            m_nativeInstance.Call("IncreaseCardPopularity", dataBaseID);   
#endif
        }
    }
}
