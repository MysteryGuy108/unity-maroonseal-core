using UnityEngine;

using MaroonSeal.Maths.Geometry.Paths.ReedsShepp;

namespace MaroonSeal.Maths.Geometry.Paths {
    [System.Serializable]
    public struct VehiclePoint {
        public Vector2 position;
        public float bearing;
        [Min(0.0f)] public float minRadius;
        public float gearChangeWeight;

        public Vector2 direction
        {
            readonly get => new(Mathf.Cos(bearing * Mathf.Deg2Rad), Mathf.Sin(bearing * Mathf.Deg2Rad));
            set => bearing = Mathf.Atan2(value.y, value.x) * Mathf.Deg2Rad;
        }

        public VehiclePoint(Vector2 _position, float _bearing = 0.0f, float _minRadius = 1.0f, float _gearChangeWeight = 0.0f)
        {
            this.position = _position;
            this.bearing = _bearing;
            this.minRadius = _minRadius;
            this.gearChangeWeight = _gearChangeWeight;
        }

        public static explicit operator PointTransform2D(VehiclePoint _vehicle) =>
            new(_vehicle.position, _vehicle.bearing);
    }

    [System.Serializable]
    public class VehiclePath : PointPath<VehiclePoint, ReedsSheppPath>
    {
        //[SerializeField][Min(0.0f)] private float minRadius = 1.0f;
        //[SerializeField] private float gearChangeWeight = 0.0f;

        protected override void ApplyPointsToSegment(ReedsSheppPath _segment, VehiclePoint _start, VehiclePoint _end)
        {
            PointTransform2D startPoint = new(_start.position, _start.bearing);

            ReedsSheppPath bestPath = ReedsSheppPath.FindShortestPath(startPoint, (PointTransform2D)_end, _start.minRadius, _start.gearChangeWeight);
            if (bestPath == null) { _segment.Clear(); return; }
            _segment.SetManoeuvres(bestPath.GetManoeuvres(), startPoint, _start.minRadius);
            bestPath.Clear();
        }
    }
}