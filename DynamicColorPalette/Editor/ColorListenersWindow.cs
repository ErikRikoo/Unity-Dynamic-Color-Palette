using System;
using DynamicColorPalette.Runtime.Utilities;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace DynamicColorPalette.Editor
{
    public class ColorListenersWindow : PopupWindowContent
    {
        public ListenersList m_Listeners;

        public ColorListenersWindow(ListenersList listeners)
        {
            m_Listeners = listeners;
        }

        public override void OnGUI(Rect rect)
        {
            if (m_Listeners.Empty)
            {
                EditorGUILayout.LabelField("No listeners");
            }

            bool previousGUIState = GUI.enabled;
            GUI.enabled = false;
            foreach (var listener in m_Listeners)
            {
                DrawListener(listener);
            }

            GUI.enabled = previousGUIState;
        }

        private void DrawListener(Listener _listener)
        {
            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayout.ObjectField(_listener.Target, typeof(Object), true);
                EditorGUILayout.TextField(_listener.MethodName);

            }
            EditorGUILayout.EndHorizontal();
        }

        public override Vector2 GetWindowSize()
        {
            if (m_Listeners.Empty)
            {
                return GUIStyle.none.CalcSize(new GUIContent("No listeners")) + BigMargin;
            }
            
            return new Vector2(LineWidth, EditorGUIUtility.singleLineHeight * m_Listeners.Count) + SmallMargin;
        }

        public Vector2 BigMargin => new Vector2(EditorGUIUtility.singleLineHeight / 2, EditorGUIUtility.singleLineHeight / 2);
        public Vector2 SmallMargin => new Vector2(EditorGUIUtility.singleLineHeight / 4, EditorGUIUtility.singleLineHeight / 4);

        public float LineWidth => 400;
    }
}