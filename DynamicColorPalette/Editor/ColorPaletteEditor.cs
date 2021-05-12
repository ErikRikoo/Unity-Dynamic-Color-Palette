using DynamicColorPalette.Runtime.SO;

using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
namespace DynamicColorPalette.Editor
{
    [CustomEditor(typeof(ColorPalette))]
    public class ColorPaletteEditor : UnityEditor.Editor
    {
      
        public ColorPalette Target => target as ColorPalette;

        public float SpaceBetweenItems => 5f;
        
        public override void OnInspectorGUI()
        {
            DrawDefaultValue();
            DrawColorsValues();
        }

        private void DrawColorsValues()
        {
            DrawColorHeader();
            if (Target.IsColorFoldoutExpanded)
            {
                DrawColorFields();
            }
        }

        private void DrawColorHeader()
        {
            EditorGUILayout.BeginHorizontal();
            {
                Target.IsColorFoldoutExpanded = EditorGUILayout.Foldout(Target.IsColorFoldoutExpanded, 
                    new GUIContent("Colors", "The list of colors in your palette")
                    );
                if (Target.IsColorFoldoutExpanded)
                {
                    GUILayout.FlexibleSpace();
                    if (!Target.IsEmpty)
                    {
                        if (GUILayout.Button("-", GUILayout.Width(20)))
                        {
                            Undo.RecordObject(Target, "Removing last color");
                            Target.RemoveLast();
                            EditorUtility.SetDirty(Target);
                        }
                    }

                    if (GUILayout.Button("+", GUILayout.Width(20)))
                    {
                        Undo.RecordObject(Target, "Adding new color");
                        Target.AddColor();
                        EditorUtility.SetDirty(Target);
                    }
                }
            }
            EditorGUILayout.EndHorizontal();
        }

        private void DrawDefaultValue()
        {
            Target.IsDefaultFoldoutExpanded = EditorGUILayout.Foldout(Target.IsDefaultFoldoutExpanded, 
                new GUIContent("Default Color", "The default color added when pressing the + button")
                );
            if (Target.IsDefaultFoldoutExpanded)
            {
                EditorGUILayout.BeginHorizontal();
                {
                    ++EditorGUI.indentLevel;
                    EditorGUI.BeginChangeCheck();
                    Color newColorDefault = EditorGUILayout.ColorField(Target.DefaultColor, GUILayout.MinWidth(40),
                        GUILayout.MaxWidth(100));
                    string newColorNameDefault = EditorGUILayout.TextField(Target.DefaultName);
                    if (EditorGUI.EndChangeCheck())
                    {
                        Undo.RecordObject(Target, "Updating color palette default color");
                        Target.DefaultColor = newColorDefault;
                        Target.DefaultName = newColorNameDefault;
                        EditorUtility.SetDirty(Target);
                    }

                    --EditorGUI.indentLevel;
                }
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.Space(SpaceBetweenItems);
            }
        }

        private void DrawColorFields()
        {
            ++EditorGUI.indentLevel;
            int colorCount = Target.Count;

            for (int i = 0; i < colorCount; ++i)
            {
                if(!DrawColorField(i))
                {
                    break;
                }
            }
            --EditorGUI.indentLevel;
        }

        private Rect m_DrawingRect;

        private bool DrawColorField(int _index)
        {
            (Color color, string colorName) = Target[_index];
            
            EditorGUILayout.BeginHorizontal();
            {
                EditorGUI.BeginChangeCheck();
                Color newColor = EditorGUILayout.ColorField(color, GUILayout.MinWidth(40), GUILayout.MaxWidth(100));
                string newColorName = EditorGUILayout.TextField(colorName);
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(Target, "Updating color palette");
                    Target[_index] = (newColor, newColorName);
                    EditorUtility.SetDirty(Target);
                }

                bool dropDownPressed = EditorGUILayout.DropdownButton(
                    new GUIContent("", "Display the objects using this color"),
                    FocusType.Keyboard, GUILayout.Width(20)
                );

                if (_index == 0 & Event.current.type == EventType.Repaint)
                {
                    m_DrawingRect = GUILayoutUtility.GetLastRect();
                }

                if (dropDownPressed)
                {
                    var window = new ColorListenersWindow(Target.GetAt(_index).listeners);
                    window.m_Listeners = Target.GetAt(_index).listeners;
                    PopupWindow.Show(ComputeRect(_index), window);
                }
            
                if (GUILayout.Button("x", GUILayout.Width(20)))
                {
                    Undo.RecordObject(Target, "Removing color");
                    Target.RemoveColorAt(_index);
                    EditorGUILayout.EndHorizontal();
                    EditorUtility.SetDirty(Target);
                    return false;
                }
            }
            EditorGUILayout.EndHorizontal();
            return true;
        }

        private Rect ComputeRect(int _index)
        {
            return new Rect(
                m_DrawingRect.position + new Vector2(0, EditorGUIUtility.singleLineHeight * _index),
                m_DrawingRect.size
                );
        }
    }
}
#endif