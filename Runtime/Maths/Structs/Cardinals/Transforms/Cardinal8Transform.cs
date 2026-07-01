using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.InputSystem;

using MaroonSeal.Maths.Geometry.Shapes;
using MaroonSeal.Maths.DataStructures;
using MaroonSeal.Maths.DataStructures.Grid;

namespace MaroonSeal.Maths {
    public class Cardinal8Transform : MonoBehaviour
    {
        //[SerializeField] protected PolarArray<Vector3> testPolarArray;
        [SerializeField] protected SquareCell<string, float> testCell;

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