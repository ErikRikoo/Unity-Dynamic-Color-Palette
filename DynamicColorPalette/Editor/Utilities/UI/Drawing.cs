﻿using UnityEngine;

 namespace DynamicColorPalette.Editor.Utilities.UI
{
    public static class Drawing
    {
        public static void DrawHorizontalLine(Vector2 _start, Vector2 _end, Color _color, float _width)
        {
            Rect drawingRect = new Rect(_start.x, _start.y, _end.x - _start.x, _width);
            UnityEditor.EditorGUI.DrawRect(drawingRect, _color);
        }
        
        public static void DrawVerticalLine(Vector2 _start, Vector2 _end, Color _color, float _width)
        {
            Rect drawingRect = new Rect(_start.x, _start.y, _width, _end.y - _start.y);
            UnityEditor.EditorGUI.DrawRect(drawingRect, _color);
        }
        
        public static void DrawRectBorder(Vector2 _start, Vector2 _size, Color _color, float _width)
        {
            Vector2 end;
            Vector2 start;
            end = _start + new Vector2(0, _size.y);
            DrawVerticalLine(_start, end, _color, _width);
            start = _start + new Vector2(_size.x, 0);
            end = _start + _size;
            DrawVerticalLine(start, end, _color, -_width);
            end = _start + new Vector2(_size.x, 0);
            DrawHorizontalLine(_start, end, _color, _width);
            start = _start + new Vector2(0, _size.y);
            end = _start + _size;
            DrawHorizontalLine(start, end, _color, -_width);
        }
    }
}