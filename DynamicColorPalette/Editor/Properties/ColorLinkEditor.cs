using System.Reflection;
using DynamicColorPalette.Editor.Utilities;
using DynamicColorPalette.Runtime;
using DynamicColorPalette.Runtime.Properties;
using Editor.Utilities;
using UnityEditor;
using UnityEngine;

namespace DynamicColorPalette.Editor.Properties
{
    [CustomPropertyDrawer(typeof(ColorLink))]
    public class ColorLinkEditor : BasePropertyDrawer
    {
        private int Margin => 5;

        private int MinHeight => (int) EditorGUIUtility.singleLineHeight;
        
        private Vector2 SquareSize => new Vector2(MinHeight, MinHeight);
        
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            ColorLink instance = GetInstance<ColorLink>(property);
            Object instanceOwner = GetInstanceOwner(property);
            int minHeight = MinHeight;
            Vector2 squareSize = SquareSize;
            float minHeightAndMargin = minHeight + Margin;
            
            Vector2 startPosition = new Vector2(position.x, position.y);
            
            
            // TODO: Add Undo.RecordObject
            EditorGUI.LabelField(new Rect(position.x, startPosition.y, position.width, minHeight), label);
            startPosition.y += minHeightAndMargin;
            
            ColorPalette palette = (ColorPalette) EditorGUI.ObjectField(
                new Rect(position.x, startPosition.y, position.width, minHeight), instance.Palette,
                    typeof(ColorPalette), allowSceneObjects: false
                );
            startPosition.y += minHeightAndMargin;

            if (palette != instance.Palette)
            {
                Undo.RecordObject(instanceOwner, "Changing palette instance");
                instance.Palette = palette;
                CallOnValidateOnPropertyObject(property);
            }
            
            if (palette == null)
            {
                return;
            }

            int colorCount = palette.Count;
            
            //position.y += minHeight;
            //property.isExpanded = EditorGUI.Foldout(position, property.isExpanded, label, true);
            // if (property.isExpanded)
            {
                EditorGUI.BeginProperty(position, label, property);
                {
                    float width = position.width;
                    if (width < minHeight)
                    {
                        return;
                    }
                    
                    var squareCounts = GetSquareCounts(width, colorCount);

                    float yBeforeRectDrawing = startPosition.y;
                    startPosition.y = DrawRects(startPosition, squareSize, squareCounts, palette);
                    if (instance.Index >= 0)
                    {
                        int index = instance.Index;
                        Vector2 indexGridPosition = GetIndexFromGridPosition(index, squareCounts);
                        
                        Vector2 start = new Vector2(startPosition.x, yBeforeRectDrawing) 
                                        + new Vector2(indexGridPosition.x * squareSize.x, indexGridPosition.y * squareSize.y);
                        Drawing.DrawRectBorder(start, squareSize, Color.cyan, 2);
                    }
                    
                    Event current = Event.current;
                    Vector2 currentMousePosition = current.mousePosition;
                    if (current.type == EventType.MouseDown)
                    {
                        float DrawingSpaceWidth = position.width;
                        Rect squaresRect = new Rect(startPosition.x, yBeforeRectDrawing, DrawingSpaceWidth, startPosition.y - yBeforeRectDrawing);
                        if (squaresRect.Contains(currentMousePosition))
                        {
                            currentMousePosition -= squaresRect.min;
                            currentMousePosition /= squareSize;
                            int newIndex = (int) currentMousePosition.x +
                                           (int) currentMousePosition.y * (int) squareCounts.x;
                            if (newIndex >= 0 && newIndex < colorCount)
                            {
                                Undo.RecordObject(instanceOwner, "Changing palette link index");
                                instance.Index = newIndex;
                                CallOnValidateOnPropertyObject(property);
                            }
                            
                        }
                    }
                }
                EditorGUI.EndProperty();
            }
        }
        
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            ColorLink instance = GetInstance<ColorLink>(property);
            float lineHeight = MinHeight;
            float lineHeightMargined = MinHeight + Margin;

            if (instance.Palette == null)
            {
                return lineHeightMargined * 2;
            }
            
            int colorCount = instance.Palette.Count;
            float drawingSpaceWidth = EditorGUIUtility.currentViewWidth;
            Vector2 squareCounts = GetSquareCounts(drawingSpaceWidth, colorCount);
            
            return lineHeightMargined + // Label
                   lineHeightMargined + // ColorPalette field
                   squareCounts.y  * lineHeight + // Squares
                   Margin; // Final margin
        }
        
        public float DrawRects(Vector2 startPosition, Vector2 size, Vector2 squareCounts, ColorPalette palette)
        {
            int colorCount = palette.Count;
            int colorIndex = 0;
            for (int i = 0; i < squareCounts.y - 1; ++i)
            {
                colorIndex = DrawRectLine(squareCounts.x, startPosition, size, colorIndex, palette);
                startPosition.y += size.y;
            }
            colorIndex = DrawRectLine(colorCount - colorIndex, startPosition, size, colorIndex, palette);
            return startPosition.y + size.y;
        }

        public int DrawRectLine(float _count, Vector2 startPosition, Vector2 size, int colorIndex, ColorPalette palette)
        {
            for (int j = 0; j < _count; ++j)
            {
                var value = palette[colorIndex];
                DrawColorRect(startPosition, size, value);
                startPosition.x += size.x;
                ++colorIndex;
            }

            return colorIndex;
        }
        
        public void DrawColorRect(Vector2 startPosition, Vector2 size, (Color color, string name) value)
        {
            Rect r = new Rect(startPosition, size);
            EditorGUI.DrawRect(r, value.color);
            if (value.name.Length != 0)
            {
                GUI.Label(r, new GUIContent("", value.name), GUIStyle.none);
            }
        }

        private static Vector2Int GetIndexFromGridPosition(int index, Vector2Int squareCounts)
        {
            return new Vector2Int(index % squareCounts.x, index / squareCounts.x);
        }

        private Vector2Int GetSquareCounts(float width, int colorCount)
        {
            if (width == 0)
            {
                return Vector2Int.zero;
            }
            int squareCountInWidth = (int) (width / MinHeight);
            int lineCount = colorCount / squareCountInWidth;
            return new Vector2Int(squareCountInWidth, lineCount + 1);
        }

        private void CallOnValidateOnPropertyObject(SerializedProperty property)
        {
            var target = property.serializedObject.targetObject;
            MethodInfo method = target.GetType().GetMethod("OnValidate", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            if (method != null)
            {
                method.Invoke(target, null);
            }
        }
    }
}