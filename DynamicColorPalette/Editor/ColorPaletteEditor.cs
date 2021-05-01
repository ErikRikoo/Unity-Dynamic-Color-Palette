using DynamicColorPalette.Runtime;
using DynamicColorPalette.Runtime.SO;
using UnityEditor;
using UnityEngine;

namespace DynamicColorPalette.Editor
{
    [CustomEditor(typeof(ColorPalette))]
    public class ColorPaletteEditor : UnityEditor.Editor
    {
        public ColorPalette Target => target as ColorPalette;
        
        public override void OnInspectorGUI()
        {
            DrawButtons();
            DrawColorFields();
        }

        private void DrawButtons()
        {
            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayout.LabelField("Colors");
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
                    Target.AddColor(Color.black, "");
                    EditorUtility.SetDirty(Target);
                }
            }
            EditorGUILayout.EndHorizontal();
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
    }
}