using UnityEngine;

using MaroonSeal.Maths;
using MaroonSeal.Maths.Geometry.Shapes;

namespace MaroonSeal.Maths.Geometry.Paths
{
    [System.Serializable]
    sealed public class ConicSectionPath : LUTPath
    {
        [SerializeField] private ConicSection conicSection;

        public override bool IsLoop => conicSection.eccentricity < 1.0f;

        #region Constructors
        public ConicSectionPath() { SetResolution(2); }
        public ConicSectionPath(int _lutResolution = 2) { SetResolution(_lutResolution); }
        public ConicSectionPath(ConicSection _conicSection, int _resolution)
        {
            conicSection = _conicSection;
            SetResolution(_resolution);
        }
        #endregion

        #region GeometryPath
        public override PointTransform GetPointAtTime(float _t)
        {
            Vector3 position = conicSection.EvaluatePositionAtTime(_t);
            Vector3 tangent = conicSection.EvaluateTangentAtTime(_t);
            return new(position, Quaternion.LookRotation(tangent, conicSection.transform.Forward));
        }
        #endregion

        #region Conic Section Path
        public ConicSection GetShape() => conicSection;

        public void SetShape(ConicSection _conicSection, int? _lutResolution = null)
        {
            int newResolution = _lutResolution ?? lutResolution;
            newResolution = Mathf.Max(newResolution, 2);

            bool needsRefresh = conicSection != _conicSection || newResolution != lutResolution;

            conicSection = _conicSection;

            lutResolution = _lutResolution ?? lutResolution;

            if (needsRefresh) { Refresh(); }
        }

        public void SetConicSection(PointTransform _transform, float _eccentricity, float _minRadius, int? _lutResolution = null)
        {
            SetShape(new(_transform, _eccentricity, _minRadius), _lutResolution);
        }

        public void SetResolution(int _lutResolution) => SetShape(conicSection, _lutResolution);
        #endregion
    }
}