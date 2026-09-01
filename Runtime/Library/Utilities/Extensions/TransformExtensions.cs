using UnityEngine;

namespace MaroonSeal {
    public static class TransformExtensions
    {
        public static void DestroyAllChildren(this Transform _transform) {
            for(int i = _transform.childCount-1; i >=0 ; i--) {
                GameObject.Destroy(_transform.GetChild(i).gameObject);
            }
        }

        public static void DestroyAllChildrenImmediately(this Transform _transform) {
            for(int i = _transform.childCount-1; i >=0 ; i--) {
                GameObject.DestroyImmediate(_transform.GetChild(i).gameObject);
            }
        }
    }
}