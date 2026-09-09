using UnityEngine;
using UnityEditor;
/*
namespace MaroonSealEditor.Maths.Geometry.Paths.Filters {
    static public class SplinePointHandles
    {
        #region Selection
        static public SplinePathFilterEditor.PointSelectionType PointSelection(SerializedProperty _pointProperty) {
            Vector3 anchor = _pointProperty.FindPropertyRelative("position").vector3Value;

            // Getting active control
            bool hasPrevious = _pointProperty.FindPropertyRelative("hasPrevious").boolValue;
            bool hasNext = _pointProperty.FindPropertyRelative("hasNext").boolValue;

            // Getting Controls
            Vector3 controlIn = _pointProperty.FindPropertyRelative("tangentIn").vector3Value + anchor;
            Vector3 controlOut = _pointProperty.FindPropertyRelative("tangentOut").vector3Value + anchor;

            // Drawing Tangent Lines
            Handles.color = new Color(0.875f, 0.875f, 0.0f, 1.0f);
            if (hasPrevious) { Handles.DrawLine(anchor, controlIn); }
            if (hasNext) { Handles.DrawLine(anchor, controlOut); }

            return DrawButtons(anchor, hasPrevious ? controlIn : null, hasNext ? controlOut : null);
        }

        static private SplinePathFilterEditor.PointSelectionType DrawButtons(Vector3 _anchor, Vector3? _controlIn, Vector3? _controlOut) {
            Handles.color = new Color(0.875f, 0.0f, 0.0f, 1.0f);
            if (Handles.Button(_anchor, Quaternion.identity, 0.05f, 0.25f, Handles.DotHandleCap)) { 
                return SplinePathFilterEditor.PointSelectionType.Anchor;
            }

            Handles.color = new Color(0.875f, 0.875f, 0.0f, 1.0f);
            if (_controlIn != null) {
                if (Handles.Button(_controlIn.Value, Quaternion.identity, 0.05f, 0.25f, Handles.DotHandleCap)) { 
                    return SplinePathFilterEditor.PointSelectionType.TangentIn;
                }
            }

            if (_controlOut != null) {
                if (Handles.Button(_controlOut.Value, Quaternion.identity, 0.05f, 0.25f, Handles.DotHandleCap)) { 
                    return SplinePathFilterEditor.PointSelectionType.TangentOut;
                }
            }

            return SplinePathFilterEditor.PointSelectionType.None;
        } 
        #endregion

        #region Editing
        static public void AnchorPositionEditing(SerializedProperty _property, Quaternion _rotation) {
            SerializedProperty positionProperty = _property.FindPropertyRelative("position");
            positionProperty.vector3Value = Handles.PositionHandle(positionProperty.vector3Value, _rotation);
            return;
        }
        static public void ControlPositionEditing(SerializedProperty _property, string _tangentString, Quaternion _rotation) {
            Vector3 position = _property.FindPropertyRelative("position").vector3Value;
            SerializedProperty targetTangentProperty = _property.FindPropertyRelative(_tangentString);

            Vector3 startCtrl = targetTangentProperty.vector3Value + position;
            Vector3 editedCtrl = Handles.PositionHandle(startCtrl, _rotation);
            targetTangentProperty.vector3Value = editedCtrl - position;
            return;
        }

        static public void RotationEditing(SerializedProperty _property, Quaternion _rotation) {
            Vector3 position = _property.FindPropertyRelative("position").vector3Value;

            SerializedProperty tangentInProperty = _property.FindPropertyRelative("tangentIn");
            SerializedProperty tangentOutProperty = _property.FindPropertyRelative("tangentOut");

            Quaternion baseRotation = Quaternion.AngleAxis(0.0f, tangentInProperty.vector3Value);
            Quaternion newRotation = Handles.RotationHandle(baseRotation, position);

            Quaternion delta = newRotation * Quaternion.Inverse(baseRotation);

            tangentInProperty.vector3Value = delta * tangentInProperty.vector3Value;
            tangentOutProperty.vector3Value = delta * tangentOutProperty.vector3Value;
        }

        static public void SizeEditing(SerializedProperty _property, Quaternion _rotation) {

        }
        #endregion
    }
}
*/