using UnityEngine;
using UnityEngine.UIElements;

using UnityEditor;
using UnityEditor.UIElements;

using MaroonSeal.Maths;

namespace MaroonSealEditor.Maths {
    static public class PointTransformHandles
    {
        #region Point Transform Handles
        static public void Transform(PointTransform _pointTransform, Vector3? _size = null,
                                     Color? _xColour = null, Color? _yColour = null, Color? _zColour = null,
                                     bool _negativeXAxis = true, bool _negativeYAxis = true, bool _negativeZAxis = true) {

            Vector3 size = _size ?? _pointTransform.scale;            

            Color xAxisColour = _xColour ?? Color.red;
            Color yAxisColour = _yColour ?? Color.green;
            Color zAxisColour = _zColour ?? Color.blue;

            Quaternion rotation = _pointTransform.Rotation;
            
            Color originalColour = Handles.color;

            Handles.color = xAxisColour;
            Vector3 right = rotation * Vector3.right * size.x * 0.5f;
            if (_negativeXAxis) { Handles.DrawLine(_pointTransform.position - right, _pointTransform.position + right); }
            else { Handles.DrawLine(_pointTransform.position, _pointTransform.position + right); }

            Handles.color = yAxisColour;
            Vector3 up = rotation * Vector3.up * size.y * 0.5f;
            if (_negativeYAxis) { Handles.DrawLine(_pointTransform.position - up, _pointTransform.position + up); }
            else { Handles.DrawLine(_pointTransform.position, _pointTransform.position + up); }

            Handles.color = zAxisColour;
            Vector3 forward = rotation * Vector3.forward * size.z * 0.5f;
            if (_negativeZAxis) { Handles.DrawLine(_pointTransform.position - forward, _pointTransform.position + forward); }
            else { Handles.DrawLine(_pointTransform.position, _pointTransform.position + forward); }

            Handles.color = originalColour;
        }

        static public PointTransform DrawPosition(PointTransform _transform, Quaternion? _rotationOverride = null) {
            Quaternion rotation = _rotationOverride ?? _transform.Rotation;
            _transform.position = Handles.PositionHandle(_transform.position, rotation);
            return _transform;
        }

        static public PointTransform DrawRotation(PointTransform _transform, Quaternion? _rotationOverride = null) {
            Quaternion rotation = _rotationOverride ?? _transform.Rotation;
            _transform.Rotation = Handles.RotationHandle(rotation, _transform.position);
            return _transform;
        }

        static public PointTransform DrawScale(PointTransform _transform, Quaternion? _rotationOverride = null) {
            Quaternion rotation = _rotationOverride ?? _transform.Rotation;
            _transform.scale = Handles.ScaleHandle(_transform.scale, _transform.position, rotation);
            return _transform;
        }
        #endregion
    }
}