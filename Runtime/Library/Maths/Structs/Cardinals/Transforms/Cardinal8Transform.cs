using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace MaroonSeal.Maths {
    public class Cardinal8Transform : MonoBehaviour
    {
        [Header("Cardinal Transform")]
        [SerializeField] protected Cardinal8 cardinalDirection;
        virtual public Cardinal8 Direction { 
            get => cardinalDirection; 
            set {
                cardinalDirection = value;
                this.transform.rotation = cardinalDirection.ToRotation();
            }
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