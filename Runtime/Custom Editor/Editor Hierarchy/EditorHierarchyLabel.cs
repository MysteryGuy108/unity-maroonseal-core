using UnityEngine;

namespace MaroonSealEditor {
    [DisallowMultipleComponent]
    public class EditorHierarchyLabel : MonoBehaviour
    {
        public bool useDefaultColours = true;

        public Color backgroundColour = new(0.2196f, 0.2196f, 0.2196f, 1.0f);

        public Color HoveringColour { get { return backgroundColour * 1.25f;}}
    }
}