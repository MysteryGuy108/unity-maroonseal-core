using System;
using System.Collections.Generic;

using UnityEngine;

namespace MaroonSeal.Maths.Geometry.Paths 
{
    [System.Serializable]
    abstract public class PathBase<TVector, TTransform> : IPath<TVector, TTransform>, ISerializationCallbackReceiver where TTransform : ITransform<TVector>
    {
        abstract public bool IsLoop { get; }
        abstract public float Length { get; }

        public bool IsDirty { get; private set; }

        #region Constructors
        public PathBase() => IsDirty = true;
        #endregion

        abstract public TTransform EvaluateTime(float _t);
        public TTransform EvaluateDistance(float _distance) => EvaluateTime(DistanceToTime(_distance));
        
        #region Conversions
        abstract public float TimeToDistance(float _t);
        abstract public float DistanceToTime(float _distance);
        #endregion

        #region Closest To Point
        abstract public float ClosestTimeToPoint(TVector _point);
        #endregion

        protected void SetDirty() => IsDirty = true;

        protected void EnsureClean()
        {
            if (!IsDirty) { return; }
            OnEnsureClean();
            IsDirty = false;
        }

        virtual protected void OnEnsureClean() {}

        virtual public void Clear() {}

        #region ISerializationCallbackReceiver
        public void OnBeforeSerialize() {}

        public void OnAfterDeserialize() {
            SetDirty();
            EnsureClean();
        }
        #endregion
    }

    [System.Serializable]
    abstract public class PrimativePath : PathBase<Vector3, Transform3D>, IPath3D
    {
    
    }

    [System.Serializable]
    abstract public class PrimativePath2D : PathBase<Vector2, Transform2D>, IPath2D
    {
    
    }
}
