
using UnityEngine;
using UnityEditor;
namespace MaroonSealEditor {
    [CustomEditor(typeof(EditorHierarchyLabel))]
    public class CustomEditorHierarchyLabelEditor : Editor
    {
        public override void OnInspectorGUI() {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("useDefaultColours"));
            
            if (!(target as EditorHierarchyLabel).useDefaultColours) {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("backgroundColour"));
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}

