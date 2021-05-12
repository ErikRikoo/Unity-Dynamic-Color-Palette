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
            m_Listener = new Listener
            {
                Target = onColorUpdated.Target as UnityEngine.Object,
                MethodName = onColorUpdated.Method.Name,
            };
        }
        
        public ColorLink(ColorLink other, UnityAction<Color> onColorUpdated)
        {
            Palette = other.Palette;
            m_Listener = new Listener
            {
                Target = onColorUpdated.Target as UnityEngine.Object,
                MethodName = onColorUpdated.Method.Name,
            };
            Index = Palette == null ? -1 : other.m_Index;
        }
        
        public int Index
        {
            get => m_Index;
            set
            {
                if (m_Index != value)
                {
                    RemoveCurrentListener();
                    m_Index = value;
                    AddListener();
                    m_Listener.Invoke((Color)this);
                }
            }
        }

        // Should cache color
        public static implicit operator Color(ColorLink _instance) => _instance.Palette[_instance.Index].Item1;

        public void OnDestroy()
        {
            RemoveCurrentListener();
        }

        private void RemoveCurrentListener()
        {
            if (m_Index >= 0)
            {
                Palette.GetAt(m_Index).RemoveListener(m_Listener);
            }
        }

        private void AddListener()
        {
            Palette?.GetAt(m_Index).AddListener(m_Listener);
        }
    }
}