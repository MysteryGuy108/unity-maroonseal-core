
using UnityEngine;
using UnityEditor;

namespace MaroonSealEditor {
    public struct EditorColours {
        public static Color Default {
            get {
                return EditorGUIUtility.isProSkin ? 
                    new Color(0.2196f, 0.2196f, 0.2196f) : 
                    new Color(0.7843f, 0.7843f, 0.7843f);
            }
        }

        public static Color Selected {
            get {
                return EditorGUIUtility.isProSkin ? 
                    new Color(0.1725f, 0.3647f, 0.5294f) :
                    new Color(0.22745f, 0.447f, 0.6902f);
            }
        }

        public static Color SelectedUnfocused {
            get {
                return EditorGUIUtility.isProSkin ? 
                    new Color(0.3f, 0.3f, 0.3f) :
                    new Color(0.68f, 0.68f, 0.68f);
            }
        }

        public static Color Hovered {
            get {
                return EditorGUIUtility.isProSkin ? 
                    new Color(0.2706f, 0.2706f, 0.2706f) :
                    new Color(0.698f, 0.698f, 0.698f);
            }
        }
    }
}

