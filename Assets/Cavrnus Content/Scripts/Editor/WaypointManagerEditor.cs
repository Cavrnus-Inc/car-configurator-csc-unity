using UnityEditor;
using UnityEngine;

namespace CavrnusDemo
{
    [CustomEditor(typeof(LerpPositionManager))]
    public class WaypointManagerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var autoLift = (LerpPositionManager) target;
            EditorGUILayout.Space();

            GUILayout.BeginHorizontal();
            
            if (GUILayout.Button("ResetToStart")) {
                autoLift.ResetToStart();
            }

            GUILayout.EndHorizontal();
        }
    }
}