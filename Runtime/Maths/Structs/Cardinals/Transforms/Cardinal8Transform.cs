using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.InputSystem;

using MaroonSeal.Maths.Geometry.Shapes;



namespace MaroonSeal.Maths {
    public class Cardinal8Transform : MonoBehaviour
    {
        [SerializeField] protected Cardinal8Array<Vector3> test8Array;
        [SerializeField] protected Cardinal4Array<Vector3> test4Array;

        [Header("Cardinal Transform")]
        [SerializeField] protected Cardinal8 cardinalDirection;
        virtual public Cardinal8 Direction {
            get => cardinalDirection; 
            set {
                cardinalDirection = value;
                this.transform.rotation = cardinalDirection.ToRotation();
            }
        }

        [System.Serializable]
        public struct TestClass {
            public float points;
            public Vector2 position;
        }

        #region MonoBehaviour
        private void Awake() => Direction = cardinalDirection;

        #if UNITY_EDITOR
        private void OnValidate()
        {
            Direction = cardinalDirection;
        }
        #endif
        #endregion
    }
}