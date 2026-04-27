using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MaroonSeal.Maths {
    public class Cardinal2DTransform : MonoBehaviour
    {
        [Header("Cardinal Transform")]
        [SerializeField] protected Cardinal2D transformDirection;
        virtual public Cardinal2D Direction {
            get { return transformDirection; }
            set {
                transformDirection = value;
                this.transform.rotation = transformDirection.Rotation;
            }
        }
    }
}