using UnityEngine;

namespace MaroonSealEditor {
    public static class EditorMouseInput
    {
        private static bool isPressed;


        private static bool isDragged;

        public static bool GetIsPressed() {
            UpdateMouseEventState();
            return isPressed;
        }
        public static bool GetIsDragged() {
            UpdateMouseEventState();
            return isDragged;
        }

        public static void UpdateMouseEventState() {
            if (Event.current == null) { return; }

            var mouseEvent = Event.current.type;

            if (mouseEvent == EventType.MouseDrag) {
                isDragged = true;
            }
                
            if (mouseEvent == EventType.DragExited) {
                isDragged = false;
                isPressed = false;
            }

            if (mouseEvent == EventType.MouseDown) { isPressed = true; }
            if (mouseEvent == EventType.MouseUp) { isPressed = false; }  
        }
    }
}
