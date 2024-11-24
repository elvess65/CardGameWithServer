using UnityEditor;
using UnityEngine;

namespace CardGame.Data
{
    [CreateAssetMenu(fileName = "AssetWrapper", menuName = "Data/AssetWrappers")]
    public class AssetWrapper : ScriptableObject
    {
        public int Version;
        public TextAsset Asset;

#if UNITY_EDITOR
        public void UpdateData(int version, TextAsset asset)
        {
            Version = version;
            Asset = asset;
            
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssetIfDirty(this);
            AssetDatabase.Refresh();
        }
#endif
    }
}
