using CavrnusDemo.SdkExtensions;
using UnityEditor;
using UnityEngine;

namespace CavrnusDemo
{
    [CustomEditor(typeof(RotationAnimator))]
    public class RotationAnimatorEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var script = (RotationAnimator) target;

            if (GUILayout.Button("SetBegin")) {
                script.SetBegin();
            }
            if (GUILayout.Button("SetEnd")) {
                script.SetEnd();
            }
        }
    }
}