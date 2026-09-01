using UnityEngine;

namespace MaroonSeal {
    public static class RectTransformExtensions
    {
        public static void DestroyAllChildren(this RectTransform _transform) {
            for(int i = _transform.childCount-1; i >=0 ; i--) {
                GameObject.Destroy(_transform.GetChild(i).gameObject);
            }
        }

        public static void DestroyAllChildrenImmediately(this RectTransform _transform) {
            for(int i = _transform.childCount-1; i >=0 ; i--) {
                GameObject.DestroyImmediate(_transform.GetChild(i).gameObject);
            }
        }
    }
}