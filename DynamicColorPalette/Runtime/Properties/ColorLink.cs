using System;
using DynamicColorPalette.Runtime.SO;
using DynamicColorPalette.Runtime.Utilities;
using UnityEngine;
using UnityEngine.Events;

namespace DynamicColorPalette.Runtime.Properties
{
    [Serializable]
    public class ColorLink
    {
        [SerializeField] public ColorPalette Palette;
        [SerializeField] private int m_Index = -1;
        [SerializeField] private Listener m_Listener;

        public ColorLink(UnityAction<Color> onColorUpdated)
        {
            m_Listener = new Listener()
            {
                target = onColorUpdated.Target as UnityEngine.Object,
                methodName = onColorUpdated.Method.Name,
            };
            
        }
        
        public int Index
        {
            get => m_Index;
            set
            {
                if (m_Index != value)
                {
                    if (m_Index >= 0)
                    {
                        Palette.GetAt(m_Index).RemoveListener(m_Listener);
                    }
                    m_Index = value;
                    Palette.GetAt(m_Index).AddListener(m_Listener);
                    m_Listener.Invoke(Palette.GetAt(m_Index).color);
                }
            }
        }

        // Should cache color
        public static implicit operator Color(ColorLink _instance) => _instance.Palette[_instance.Index].Item1;
    }
}