using UnityEngine;
using UnityEditor;

namespace MaroonSealEditor 
{
    public static class PositionHandles
    {
        [System.Flags]
        public enum Axes
        {
            None = 0,
            X = 1 << 0, Y = 1 << 1, Z = 1 << 2,
            XY = X | Y, XZ = X | Z, YZ = Y | Z,
            All = X | Y | Z,
        }

        const float kPlaneOffset = 0.25f;
        const float kPlaneSize   = 0.15f;
        const float kCenterSize  = 0.075f;

        // Dead zone: near-perpendicular views keep the previous sign instead of flickering.
        const float kFlipDeadZone = 0.01f;

        static Vector3 s_Octant = Vector3.one;

        public static Vector3 DrawHandle2D(Vector3 position, Quaternion rotation)
            => DrawHandle(position, rotation, Axes.XY);

        public static Vector3 DrawHandle3D(Vector3 position, Quaternion rotation)
            => DrawHandle(position, rotation, Axes.All);

        public static Vector3 DrawHandle(Vector3 position, Quaternion rotation, Axes axes)
        {
            float size = HandleUtility.GetHandleSize(position);
            Vector3 snap = EditorSnapSettings.move;

            Vector3 right = rotation * Vector3.right;
            Vector3 up    = rotation * Vector3.up;
            Vector3 fwd   = rotation * Vector3.forward;

            // Only re-evaluate when nothing is being dragged.
            if (GUIUtility.hotControl == 0)
                s_Octant = CalcOctant(position, rotation, s_Octant);
            Vector3 octant = s_Octant;

            if ((axes & Axes.X) != 0)
            {
                Handles.color = Handles.xAxisColor;
                position = Handles.Slider(position, right, size, Handles.ArrowHandleCap, snap.x);
            }
            if ((axes & Axes.Y) != 0)
            {
                Handles.color = Handles.yAxisColor;
                position = Handles.Slider(position, up, size, Handles.ArrowHandleCap, snap.y);
            }
            if ((axes & Axes.Z) != 0)
            {
                Handles.color = Handles.zAxisColor;
                position = Handles.Slider(position, fwd, size, Handles.ArrowHandleCap, snap.z);
            }

            if ((axes & Axes.XY) == Axes.XY)
                position = PlaneHandle(position, right * octant.x, up * octant.y, fwd,
                                    size, snap, Handles.zAxisColor);
            if ((axes & Axes.XZ) == Axes.XZ)
                position = PlaneHandle(position, right * octant.x, fwd * octant.z, up,
                                    size, snap, Handles.yAxisColor);
            if ((axes & Axes.YZ) == Axes.YZ)
                position = PlaneHandle(position, up * octant.y, fwd * octant.z, right,
                                    size, snap, Handles.xAxisColor);

            if (axes != Axes.None)
            {
                Handles.color = Handles.centerColor;
                position = Handles.FreeMoveHandle(position, size * kCenterSize,
                    snap, Handles.RectangleHandleCap);
            }

            return position;
        }

        static Vector3 CalcOctant(Vector3 position, Quaternion rotation, Vector3 previous)
        {
            Camera cam = Camera.current;
            if (cam == null)
                return previous;

            // Vector from the handle toward the viewer, in handle-local space.
            Vector3 toCamera = cam.orthographic
                ? -cam.transform.forward
                : (cam.transform.position - position);

            Vector3 local = Quaternion.Inverse(rotation) * toCamera;

            Vector3 result = previous;
            for (int i = 0; i < 3; i++)
            {
                if (Mathf.Abs(local[i]) > kFlipDeadZone)
                    result[i] = Mathf.Sign(local[i]);
            }
            return result;
        }

        static Vector3 PlaneHandle(Vector3 position, Vector3 a, Vector3 b, Vector3 normal,
                                float size, Vector3 snap, Color color)
        {
            Handles.color = color;
            Vector3 offset = (a + b) * size * kPlaneOffset;
            Vector3 moved = Handles.Slider2D(
                position + offset,
                normal, a, b,
                size * kPlaneSize,
                Handles.RectangleHandleCap,
                snap);
            return moved - offset;
        }
    }
}