using UnityEditor;
using UnityEngine;

namespace CavrnusDemo
{
    [CustomEditor(typeof(CavrnusRotationAnimator))]
    public class RotationAnimatorEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var script = (CavrnusRotationAnimator) target;

            if (GUILayout.Button("SetBegin")) {
                script.SetBegin();
            }
            if (GUILayout.Button("SetEnd")) {
                script.SetEnd();
            }
        }
    }
}