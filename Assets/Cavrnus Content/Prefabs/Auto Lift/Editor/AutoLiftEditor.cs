using UnityEditor;
using UnityEngine;

namespace CavrnusDemo
{
    [CustomEditor(typeof(CarLift))]
    public class AutoLiftEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var autoLift = (CarLift) target;
            EditorGUILayout.Space();

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Raise")) {
                autoLift.Raise();
            }

            if (GUILayout.Button("Lower")) {
                autoLift.Lower();
            }

            if (GUILayout.Button("Stop")) {
                autoLift.Stop();
            }
            
            GUILayout.EndHorizontal();
        }
    }
}