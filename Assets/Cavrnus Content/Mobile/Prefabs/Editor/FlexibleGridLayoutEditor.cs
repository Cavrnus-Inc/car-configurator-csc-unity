using UnityEditor;
using UnityEditor.UI;
using UnityEngine;

namespace Cavrnus.UI
{
    [CustomEditor(typeof(FlexibleGridLayout))]
    public class FlexibleGridLayoutEditor : GridLayoutGroupEditor
    {
        [Space]
        SerializedProperty ColumnCount;
        SerializedProperty RowCount;
        SerializedProperty CellSizeConstraint;
        SerializedProperty BreakPoints;
        SerializedProperty DefaultBreakPoint;
        SerializedProperty UseAspectRatios;
        SerializedProperty TargetRectWithProperWidth;
        SerializedProperty PortraitColumnCount;
        SerializedProperty LandscapeColumnCount;

        protected override void OnEnable()
        {
            base.OnEnable();
            ColumnCount = serializedObject.FindProperty("ColumnCount");
            RowCount = serializedObject.FindProperty("RowCount");
            CellSizeConstraint = serializedObject.FindProperty("CellSizeConstraint");
            BreakPoints = serializedObject.FindProperty("BreakPoints");
            DefaultBreakPoint = serializedObject.FindProperty("DefaultBreakPoint");
            UseAspectRatios = serializedObject.FindProperty("UseAspectRatios");
            TargetRectWithProperWidth = serializedObject.FindProperty("TargetRectWithProperWidth");
            PortraitColumnCount = serializedObject.FindProperty("PortraitColumnCount");
            LandscapeColumnCount = serializedObject.FindProperty("LandscapeColumnCount");
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            EditorGUILayout.PropertyField(ColumnCount);
            EditorGUILayout.PropertyField(RowCount);
            EditorGUILayout.PropertyField(CellSizeConstraint);
            EditorGUILayout.PropertyField(BreakPoints);
            EditorGUILayout.PropertyField(DefaultBreakPoint);
            EditorGUILayout.PropertyField(UseAspectRatios);
            EditorGUILayout.PropertyField(TargetRectWithProperWidth);
            EditorGUILayout.PropertyField(PortraitColumnCount);
            EditorGUILayout.PropertyField(LandscapeColumnCount);
            serializedObject.ApplyModifiedProperties();
        }
    }
}