using System.Reflection;
using DynamicColorPalette.Editor.Utilities;
using DynamicColorPalette.Editor.Utilities.UI;
using DynamicColorPalette.Runtime.Properties;
using DynamicColorPalette.Runtime.SO;
using Editor.Utilities;
using Editor.Utilities.UI;
using UnityEditor;
using UnityEngine;

namespace DynamicColorPalette.Editor.Properties
{
    [CustomPropertyDrawer(typeof(ColorLink))]
    public class ColorLinkEditor : BasePropertyDrawer
    {
        private Vector2 SquareSize => new Vector2(MinHeight, MinHeight);
        
        protected override void DrawElements(Rect position, SerializedProperty property, GUIContent label)
        {
            ColorLink instance = GetInstance<ColorLink>();
            Object instanceOwner = InstanceOwner;

            EditorGUI.LabelField(GetLineDrawingRect(), label);
            ColorPalette palette = (ColorPalette) EditorGUI.ObjectField(
                GetLineDrawingRect(), instance.Palette,
                    typeof(ColorPalette), false
                );

            if (palette != instance.Palette)
            {
                Undo.RecordObject(instanceOwner, "Changing palette instance");
                instance.Palette = palette;
                OnInstanceChanged();
            }
            
            if (palette == null)
            {
                return;
            }

            HandlePaletteDrawing();
        }

        private void HandlePaletteDrawing()
        {
            ColorLink instance = GetInstance<ColorLink>();
            ColorPalette palette = instance.Palette;
            Vector2 squareSize = SquareSize;
            int colorCount = palette.Count;
            
            if (Width < MinHeight)
            {
                return;
            }
                
            var squareCounts = GetSquareCounts(colorCount, Width);

            Vector2 drawingPositionBeforeRects = CurrentDrawingStart;
            DrawRects(squareCounts, palette);
            if (instance.Index >= 0)
            {
                int index = instance.Index;
                Vector2 indexGridPosition = GetGridPositionFromIndex(index, squareCounts);

                Vector2 start = drawingPositionBeforeRects + indexGridPosition * squareSize;
                Drawing.DrawRectBorder(start, squareSize, Color.cyan, 2);
            }
                
            Event current = Event.current;
            if (current.type == EventType.MouseDown)
            {
                Vector2 currentMousePosition = current.mousePosition;
                Rect squaresRect = new Rect(drawingPositionBeforeRects, new Vector2(Width, CurrentDrawingStart.y - drawingPositionBeforeRects.y));
                if (squaresRect.Contains(currentMousePosition))
                {
                    currentMousePosition -= squaresRect.min;
                    currentMousePosition /= squareSize;
                    int newIndex = (int) currentMousePosition.x +
                                   (int) currentMousePosition.y * squareCounts.x;
                    if (newIndex >= 0 && newIndex < colorCount)
                    {
                        Undo.RecordObject(InstanceOwner, "Changing palette link index");
                        instance.Index = newIndex;
                        OnInstanceChanged();
                    }
                }
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
            Vector2 squareCounts = GetSquareCounts(colorCount, EditorGUIUtility.currentViewWidth);
            
            return lineHeightMargined + // Label
                   lineHeightMargined + // ColorPalette field
                   squareCounts.y  * lineHeight + // Squares
                   Margin; // Final margin
        }
        
        public void DrawRects(Vector2 squareCounts, ColorPalette palette)
        {
            int colorCount = palette.Count;
            int colorIndex = 0;
            for (int i = 0; i < squareCounts.y - 1; ++i)
            {
                colorIndex = DrawRectLine(squareCounts.x, colorIndex, palette);

            }
            DrawRectLine(colorCount - colorIndex, colorIndex, palette);
        }

        public int DrawRectLine(float _count, int colorIndex, ColorPalette palette)
        {
            Vector2 size = SquareSize;
            Vector2 currentDrawingStart = CurrentDrawingStart;
            for (int j = 0; j < _count; ++j)
            {
                var value = palette[colorIndex];
                DrawColorRect(currentDrawingStart, size, value);
                currentDrawingStart.x += size.x;
                ++colorIndex;
            }
            OnDraw(size.y);
            
            return colorIndex;
        }
        
        public void DrawColorRect(Vector2 startPosition, Vector2 size, (Color color, string name) value)
        {
            Rect r = new Rect(startPosition, size);
            EditorGUI.DrawRect(r, value.color);
            Controls.Tooltip(r, value.name);
        }

        private static Vector2Int GetGridPositionFromIndex(int index, Vector2Int squareCounts)
        {
            return new Vector2Int(index % squareCounts.x, index / squareCounts.x);
        }

        private Vector2Int GetSquareCounts(int colorCount, float width)
        {
            if (width < MinHeight)
            {
                return Vector2Int.zero;
            }
            int squareCountInWidth = (int) (width / MinHeight);
            int lineCount = colorCount / squareCountInWidth;
            return new Vector2Int(squareCountInWidth, lineCount + 1);
        }
    }
}