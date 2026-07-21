using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEditor.SceneManagement;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace MaroonSealEditor {
    
    public struct ItemSelectionStatus {
        public bool IsSelected;
        public bool IsHovered;
        public bool IsDropdownHovered;
    }

    [UnityEditor.InitializeOnLoad]
    public class CustomEditorHierarchyLabelDrawer
    {
        static bool hierarchyEditorWindowHasFocus;
        static EditorWindow hierarchyEditorWindow;
        static HashSet<int> selectedInstanceIDs;
        
        static CustomEditorHierarchyLabelDrawer() {
            selectedInstanceIDs = new HashSet<int>();

            EditorApplication.hierarchyWindowItemOnGUI -= OnHirearchyWindowItemOnGUI;
            EditorApplication.hierarchyWindowItemOnGUI += OnHirearchyWindowItemOnGUI;

            EditorApplication.update -= OnEditorUpdate;
            EditorApplication.update += OnEditorUpdate;
        } 

        #region Hierarchy Window Drawer    
        static private void OnEditorUpdate() {  
            if (hierarchyEditorWindow == null) {
                hierarchyEditorWindow = EditorWindow.GetWindow(
                    Type.GetType($"{nameof(UnityEditor)}.SceneHierarchyWindow,{nameof(UnityEditor)}"));
            }

            hierarchyEditorWindowHasFocus = EditorWindow.focusedWindow != null &&
                EditorWindow.focusedWindow == hierarchyEditorWindow;

            selectedInstanceIDs?.Clear();
        }

        static private void OnHirearchyWindowItemOnGUI(int _instanceID, Rect _selectionRect) {
            
            ItemSelectionStatus currentItemSelection = GetItemSelectionStatus(_instanceID, _selectionRect);
            UpdateSelectedObjectIDLUT(_instanceID, currentItemSelection);

            GameObject obj = EditorUtility.InstanceIDToObject(_instanceID) as GameObject;
            if (obj == null) { return; }

            EditorHierarchyLabel hierarchyLabel = obj.GetComponent<EditorHierarchyLabel>();
            if (hierarchyLabel == null) { return; }

            GUIContent content = GetHierarchyContent(obj, hierarchyLabel);
            if (content == null) { return;}
            ClearDefault(_selectionRect, currentItemSelection);

            Color originalColor = GUI.color;
            if (!obj.activeInHierarchy)  {
                Color transparentIconColor = new(originalColor.r, originalColor.g, originalColor.b, 0.5f);
                GUI.color = transparentIconColor;
            }

            EditorGUI.LabelField(_selectionRect, content);

            Rect textRect = _selectionRect;
            textRect.x += 18.5f;
            textRect.width -= 18.5f;
            EditorGUI.LabelField(textRect, obj.name);

            GUI.color = originalColor;
        }
        #endregion

        static private GUIContent GetHierarchyContent(GameObject _obj, EditorHierarchyLabel _label) {
            Component[] components = _obj.GetComponents<Component>();
            if (components == null || components.Length <= 0) { return null; }

            Component targetComponent = null;
            for(int i = 0; i < components.Length; i++) {
                if (components[i] == _label) { continue; }
                if (components[i] is Transform && components.Length > 2) { continue; }

                targetComponent = components[i];
                break;
            }

            if (targetComponent == null) { return null; }

            Type targetType = targetComponent.GetType();

            GUIContent content = EditorGUIUtility.ObjectContent(targetComponent, targetType);
            content.text = null;
            content.tooltip = targetType.Name;

            return content;
        }

        #region Selection Drawing
        static private ItemSelectionStatus GetItemSelectionStatus(int _instanceID, Rect _selectionRect) {
            Rect rowSelectionRect = _selectionRect;
            rowSelectionRect.x = 0; 
            rowSelectionRect.width = short.MaxValue;

            float expandIconWidth = 11.0f;

            Rect expandIconRect = _selectionRect;
            expandIconRect.x -= expandIconWidth;
            expandIconRect.width = expandIconWidth + 3.0f;

            return new ItemSelectionStatus() {
                IsSelected = Selection.instanceIDs.Contains(_instanceID),
                IsHovered = rowSelectionRect.Contains(Event.current.mousePosition),
                IsDropdownHovered = expandIconRect.Contains(Event.current.mousePosition)
            };
        }

        static private void UpdateSelectedObjectIDLUT(int _instanceID, ItemSelectionStatus _selectionStatus) {

            if (_selectionStatus.IsSelected || (_selectionStatus.IsDropdownHovered && EditorMouseInput.GetIsPressed()))  {
                if (Selection.instanceIDs.Length > 1) { selectedInstanceIDs.Clear(); }
                selectedInstanceIDs.Add(_instanceID);
            }
            else {
                selectedInstanceIDs.Remove(_instanceID);
            }
        }

        static private void ClearDefault(Rect _selectionRect, ItemSelectionStatus _selectionStatus) {
            
            int selectedAmount = Selection.instanceIDs.Length > 1 ? Selection.instanceIDs.Length : selectedInstanceIDs.Count;
            bool isFocus = hierarchyEditorWindowHasFocus;
            Color backgroundColour;
            bool mouseInput = EditorMouseInput.GetIsPressed();

            if (_selectionStatus.IsSelected) {
                if (mouseInput && !_selectionStatus.IsDropdownHovered && !_selectionStatus.IsHovered && selectedAmount == 1) {
                    backgroundColour = EditorColours.Default;
                }
                else {
                    backgroundColour = isFocus ? EditorColours.Selected : EditorColours.SelectedUnfocused;
                }
            }
            else if (_selectionStatus.IsHovered) {
                backgroundColour = mouseInput && !_selectionStatus.IsDropdownHovered ? EditorColours.Selected : EditorColours.Hovered;
            }
            else {
                backgroundColour = EditorColours.Default;
            }
            
            EditorGUI.DrawRect(_selectionRect, backgroundColour);
        }
        #endregion
    }
}