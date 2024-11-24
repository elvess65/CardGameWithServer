using System;
using System.IO;
using CardGame.Data;
using Cysharp.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace CardGame.Editor
{
    public class UpdatedToolWindow : EditorWindow 
    {
        [MenuItem("Custom Tools/Updater Tool")]
        public static void ShowWindow() => GetWindow<UpdatedToolWindow>("Updater Tool");

        private string m_errorMessage = "";
        private bool m_isSynced = false;
        private bool m_isUpdateRequired = false;
        
        private AssetWrapper m_targetWrapper;
        private int m_serverVersion;

        private const string REQUEST_VERSION_URL = "/updater/version";
        private const string FORCE_INCREMENT_VERSION_URL = "/updater/incrementVersion";
        private const string DOWNLOAD_URL = "/updater/getNewRules";

        private string GetRequestVersionUrl => URLBuilder.GetUrl() + REQUEST_VERSION_URL;
        private string GetForceIncrementVersionUrl => URLBuilder.GetUrl() + FORCE_INCREMENT_VERSION_URL;
        private string GetDownloadUrl => URLBuilder.GetUrl() + DOWNLOAD_URL;
        
        #region Draw

        private bool DrawErrors()
        {
            if (string.IsNullOrEmpty(m_errorMessage))
                return false;
            
            var errorStyle = new GUIStyle(GUI.skin.label)
            {
                fontSize = 15,
                wordWrap = true,
                normal = { textColor = Color.red }
            };
            
            GUILayout.Label(m_errorMessage, errorStyle);
            DrawRefreshButton();
            
            return true;
        }

        private bool DrawIsUpdatingStatus()
        {
            if (m_isSynced)
                return false;
            
            GUILayout.Label("Updating...");
            return true;
        }

        private void DrawRefreshButton()
        {
            if (GUILayout.Button("Refresh", GUILayout.Width(100)))
            {
                Refresh();
            }
        }

        private void DrawBody()
        {
            var style = new GUIStyle(GUI.skin.label)
            {
                fontSize = 18,
                wordWrap = true
            };
            
            var versionStyle = new GUIStyle(GUI.skin.label)
            {
                fontSize = 10,
                wordWrap = true,
                normal = {textColor = Color.gray}
            };
            
            if(!m_isUpdateRequired)
            {
                using var _ = new GUILayout.HorizontalScope();

                using (new GUILayout.VerticalScope())
                {
                    GUILayout.Label("Data is up to date", style);
                    GUILayout.Label($"Version: {m_serverVersion}", versionStyle);
                }
                
                if (GUILayout.Button("Force Increment Version", GUILayout.Width(150)))
                {
                    ForceIncrementServerVersion();
                }
            }
            else
            {
                GUILayout.Label("Update available", style);
                GUILayout.Label($"Local version: {m_targetWrapper.Version}\nServer Version: {m_serverVersion}", versionStyle);

                if (GUILayout.Button("Update"))
                {
                    UpdateData();
                }
            }
        }
        
        private void OnGUI()
        {
            if (DrawErrors())
                return;
            
            if (DrawIsUpdatingStatus())
                return;
            
            DrawRefreshButton();
            DrawBody();
        }

        #endregion

        #region Update

        private async void UpdateData()
        {
            m_isSynced = false;

            try
            {
                var asset = await NetworkController.DownloadFileAsync(GetDownloadUrl, "Resources", "serverRules.txt");
                if (asset != null)
                {
                    m_targetWrapper.UpdateData(m_serverVersion, asset);
                }
            }
            catch (Exception e)
            {
                m_errorMessage = $"Failed to update data: {e.Message}";
                return;
            }
            finally
            {
                m_isSynced = true;
            }
            
            Refresh();
        }

        private async void ForceIncrementServerVersion()
        {
            m_isSynced = false;

            var intWrapper = await NetworkController.Post<IntWrapper>(GetForceIncrementVersionUrl, new IntWrapper() { Value = m_serverVersion + 1} );
            m_serverVersion = intWrapper.Value;
            
            m_isUpdateRequired = IsUpdateRequired(m_targetWrapper, m_serverVersion);
            m_isSynced = true;
            
            Repaint();
        }

        #endregion
        
        #region Refresh
        
        private async void Refresh()
        {
            m_isSynced = false;
            m_errorMessage = string.Empty;

            if (!ReadLocalData("RulesAssetWrapper", out m_targetWrapper))
            {
                m_errorMessage = "Couldn't read local data";
                return;
            }
            
            try
            {   
                var intWrapper = await RequestVersion(GetRequestVersionUrl);
                m_serverVersion = intWrapper.Value;
            }
            catch (Exception e)
            {
                m_errorMessage = $"Failed request version: {e.Message}";
                return;
            }
            
            m_isUpdateRequired = IsUpdateRequired(m_targetWrapper, m_serverVersion);
            m_isSynced = true;
            
            Repaint();
        }
        
        private async UniTask<IntWrapper> RequestVersion(string url) => await NetworkController.Get<IntWrapper>(url);

        private bool ReadLocalData(string assetName, out AssetWrapper assetWrapper) => TryGetAsset(assetName, out assetWrapper);

        private bool IsUpdateRequired(AssetWrapper wrapper, int serverVersion) => wrapper.Version < serverVersion;

        private bool TryGetAsset<T>(string targetName, out T assetWrapper) where T: Object
        {
            assetWrapper = null;
            
            var guids = AssetDatabase.FindAssets($"t:{typeof(T).Name}", new string[] { "Assets/Data" });
            foreach (var guid in guids)
            {
                var assetPath = AssetDatabase.GUIDToAssetPath(guid);
                if (string.IsNullOrEmpty(assetPath))
                    continue;

                var fileName = Path.GetFileNameWithoutExtension(assetPath);
                if (!fileName.Contains(targetName))
                    continue;

                assetWrapper = AssetDatabase.LoadAssetAtPath<T>(assetPath);
            }

            return assetWrapper != null;
        }
        
        #endregion

        #region System
        
        private void Initialize() => Refresh();

        private void Dispose()
        {
            Debug.Log("Dispose");
        }
        
        private void OnEnable() => Initialize();

        private void OnDisable() => Dispose();
        
        #endregion
    }
}


