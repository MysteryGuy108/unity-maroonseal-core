using UnityEngine;

using MaroonSeal.Maths;
using MaroonSeal.Maths.Geometry.Shapes;

namespace MaroonSeal.Maths.Geometry.Paths
{
    [System.Serializable]
    sealed public class ConicSectionPath : SamplePath
    {
        [SerializeField] private ConicSection conicSection;

        public override bool IsLoop => conicSection.eccentricity < 1.0f;

        #region Constructors
        public ConicSectionPath() : base() {}
        public ConicSectionPath(int _resolution = 2) : base(_resolution) {}
        public ConicSectionPath(ConicSection _conicSection, int _resolution) : this(_resolution)
        {
            conicSection = _conicSection;
        }
        #endregion

        #region GeometryPath
        public override Transform3D EvaluateTime(float _time)
        {
            Vector3 position = conicSection.EvaluatePointAtTime(_time);
            Vector3 tangent = conicSection.EvaluateTangentAtTime(_time);
            return new(position, Quaternion.LookRotation(tangent, conicSection.Transform.Forward));
        }
        #endregion

        #region Conic Section Path
        public ConicSection GetShape() => conicSection;

        public void SetConicSection(ConicSection _conicSection)
        {
            if (conicSection != _conicSection) { SetDirty(); }
            conicSection = _conicSection;
        }

        public void SetConicSection(Transform3D _transform, float _eccentricity, float _minRadius, int? _lutResolution = null)
        {
            SetConicSection(new(_transform, _eccentricity, _minRadius));
        }
        #endregion
    }
}