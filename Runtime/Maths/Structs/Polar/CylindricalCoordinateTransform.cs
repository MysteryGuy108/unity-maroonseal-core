using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MaroonSeal.Maths {
    public class CylindricalCoordinateTransform : MonoBehaviour
    {
        [SerializeField] protected CylindricalCoordinate coordinate;

        public float Theta { get; private set; }
        public float Radius { get; private set; }
        public float Height { get; private set; }

        private void Update() {
            RefreshTransform();
        }

        protected void RefreshTransform() {
            this.transform.localPosition = coordinate.Point;
            this.transform.localRotation = Quaternion.LookRotation(-coordinate.Point.normalized, Vector3.up);
        }

        public void SetTheta(float _theta) {
            coordinate.theta = _theta;
            RefreshTransform();
        }

        public void SetRadius(float _radius) {
            coordinate.theta = _radius;
            RefreshTransform();
        }

        private void OnDrawGizmosSelected() {
            if (this.transform.parent) {
                Gizmos.DrawLine(this.transform.parent.TransformPoint(Vector3.zero), this.transform.parent.TransformPoint(coordinate.Point));
                return;
            }
            
            Gizmos.DrawLine(Vector3.zero, coordinate.Point);
        }
    }
}


