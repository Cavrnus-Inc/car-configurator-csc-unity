using UnityEditor;
using UnityEngine;

namespace CavrnusDemo
{
    [CustomEditor(typeof(MonoBehaviour), true)]
    public class CustomInteractableEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (target is ICustomInteractable interactable)
            {
                EditorGUILayout.Space();
                if (GUILayout.Button("Interact")) {
                    interactable.Interact();
                }
            }
        }
    }
}