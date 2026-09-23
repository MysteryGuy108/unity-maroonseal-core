using UnityEngine;

namespace MaroonSeal.Maths.Geometry
{
    public struct DirectionPlane
    {
        public Vector3 origin;
        public Vector3 forward;
        
        public Vector3 normal;
        public Vector3 right;

        public Vector3 worldUp;
        
        public Quaternion rotation;

        public DirectionPlane(Vector3 _from, Vector3 _to, Vector3 _up)
        {
            worldUp = _up;

            origin = _from;
            forward = GetForward(_from, _to);
            normal = GetNormal(forward, _up);
            right = Vector3.Cross(normal, forward).normalized;

            rotation = Quaternion.LookRotation(forward, normal);
        }

        public static Vector3 GetForward(Vector3 from, Vector3 to)
        {
            Vector3 forward = to - from;
            return forward.magnitude == 0.0f ? Vector3.forward : forward.normalized;
        }

        public static Vector3 GetNormal(Vector3 _forward, Vector3 _up) 
            => Quaternion.LookRotation(_forward, _up) * Vector3.up;

        readonly public Vector2 ToPlaneSpace(Vector3 point)
        {
            Vector3 offset = point - origin;

            return new Vector2(
                Vector3.Dot(offset, forward),
                Vector3.Dot(offset, right)
            );
        }

        readonly public Vector3 FromPlaneSpace(Vector2 point) 
            => origin + forward * point.x + right * point.y;

        readonly public Transform2D ToPlaneSpace(Transform3D _point)
            => new()
            {
                position = this.ToPlaneSpace(_point.position),
                angle = QuaternionMaths.GetAngleAroundDirection(Quaternion.Inverse(this.rotation) * _point.rotation, worldUp),
                scale = _point.scale
            };

        readonly public Transform3D FromPlaneSpace(Transform2D _point)
            => new()
            {
                position = this.FromPlaneSpace(_point.position),
                rotation = QuaternionMaths.GetNormalised(this.rotation * Quaternion.AngleAxis(_point.angle, worldUp)),
                scale = Vector3.one
            };

        static public Transform2D ToPlaneSpace(Transform3D _point, Transform3D _from, Transform3D _to, Vector3 _worldUp)
            => new DirectionPlane(_from.position, _to.position, _worldUp).ToPlaneSpace(_point);

        public static Transform3D FromPlaneSpace(Transform2D _point, Transform3D _from, Transform3D _to, Vector3 _worldUp)
            => new DirectionPlane(_from.position, _to.position, _worldUp).FromPlaneSpace(_point);
    }
}
