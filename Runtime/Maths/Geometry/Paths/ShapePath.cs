using System;
using System.Collections.Generic;

using UnityEngine;

namespace MaroonSeal.Maths.Geometry.Paths {
    abstract public class ShapePath : IShapePath, ISerializationCallbackReceiver
    {
        abstract public bool IsLoop { get; }
        abstract public float Length { get; }

        #region Constructors
        public ShapePath() { Refresh(); }
        #endregion

        #region GeometryPath
        virtual public void Refresh() {}
        virtual public void Clear() {}
        #endregion

        #region IGeometryPath
        abstract public PointTransform GetPointAtTime(float _t);
        public PointTransform2D GetPoint2DAtTime(float _t) {
            PointTransform point = this.GetPointAtTime(_t);
            if (point.Forward == Vector3.zero) { Debug.Log("HERE ACTUALLY"); }
            return new(point.position, point.Rotation, point.scale) { Right = point.Forward };
        }

        public PointTransform GetPointAtDistance(float _distance) {
            return GetPointAtTime(GetTimeAtDistance(_distance));
        }
        public PointTransform2D GetPoint2DAtDistance(float _distance) {
            PointTransform point = this.GetPointAtDistance(_distance);
            return new(point.position, point.Rotation, point.scale) { Right = point.Forward };
        }

        abstract public float GetTimeClosestToPosition(Vector3 _position);

        abstract public float GetDistanceAtTime(float _t);
        abstract public float GetTimeAtDistance(float _distance);
        #endregion

        #region ISerializationCallbackReciever
        public void OnBeforeSerialize() {}
        public void OnAfterDeserialize() => Refresh();
        #endregion
    }
}
