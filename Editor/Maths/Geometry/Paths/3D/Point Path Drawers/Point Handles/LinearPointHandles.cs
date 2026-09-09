using UnityEngine;
using UnityEditor;

namespace MaroonSealEditor.Maths.Geometry.Paths.Filters {
    static public class LinearPointHandles
    {
        static public void PositionEditing(SerializedProperty _property, Quaternion _rotation) {
            SerializedProperty positionProperty = _property.FindPropertyRelative("position");
            positionProperty.vector3Value = Handles.PositionHandle(positionProperty.vector3Value, _rotation);
        }

        static public void RotationEditing(SerializedProperty _property, Quaternion _rotation) {

        }

        static public void SizeEditing(SerializedProperty _property, Quaternion _rotation) {

        }
    }
}