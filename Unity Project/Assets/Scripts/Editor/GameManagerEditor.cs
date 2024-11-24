using CardGame.Core;
using UnityEditor;
using UnityEngine;

namespace CardGame.Editor
{
    [CustomEditor(typeof(GameManager))]
    public class GameManagerEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            
            var castedTarget = (GameManager)target;

            EditorGUILayout.Space();
            string message = string.Empty;
            
            if (castedTarget.IsLocalMode)
            {
                message = "Data will be retrieved from LocalDataProvider bypassing SDK.";
            }
            else
            {
                message = castedTarget.SimulateNativeServer ? 
                    "Data will be retrieved from SDK bypassing server." : 
                    "Data will be retrieved by SDK directly from server.";
            }
            
            EditorGUILayout.HelpBox(message, MessageType.Info);
            
            EditorGUILayout.Space();

            if (GUILayout.Button("Updater Tool", GUILayout.Height(25)))
            {
                UpdatedToolWindow.ShowWindow();
            }
        }
    }
}
