using UnityEngine;

namespace DynamicColorPalette.Editor.Utilities.UI
{
    public static class Controls
    {
        public static void Tooltip(Rect _triggerSpace, string _value)
        {
            if (_value.Length != 0)
            {
                GUI.Label(_triggerSpace, new GUIContent("", _value), GUIStyle.none);
            }
        }
    }
}