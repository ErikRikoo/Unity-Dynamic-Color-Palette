using System;
using System.Collections.Generic;
using DynamicColorPalette.Runtime.Utilities;
using UnityEngine;

namespace DynamicColorPalette.Runtime.SO
{
    [Serializable]
    public class ColorAttribute
    {
        public Color color;
        public string name;
        public ListenersList listeners = new ListenersList();

        public static implicit operator (Color, string)(ColorAttribute _instance) => (_instance.color, _instance.name);

        public void Set((Color, string) newValues)
        {
            if (color != newValues.Item1)
            {
                color = newValues.Item1;
                listeners?.Invoke(color);
            }

            name = newValues.Item2;
        }

        public void AddListener(Listener _newListener)
        {
            listeners.AddListener(_newListener);
        }
        
        public void RemoveListener(Listener _listenerToRemove)
        {
            listeners.RemoveListener(_listenerToRemove);
        }
    }
    
    
    [ExecuteInEditMode]
    [CreateAssetMenu(fileName = "ColorPalette", menuName = "ColorPalette", order = 0)]
    public class ColorPalette : ScriptableObject
    {
        [SerializeField] private List<ColorAttribute> m_ColorAttributes = new List<ColorAttribute>();
        [SerializeField] public Color DefaultColor = new Color(0, 0, 0, 1);
        [SerializeField] public string DefaultName;

        public int Count => m_ColorAttributes.Count;

        public bool IsEmpty => Count == 0;

        private void Awake()
        {
            foreach (var colorAttribute in m_ColorAttributes)
            {
                colorAttribute.listeners.Clear();
            }
        }

        public void AddColor()
        {
            AddColor(DefaultColor, DefaultName);
        }
        
        public void AddColor(Color _newColor, string _newColorName)
        {
            m_ColorAttributes.Add(new ColorAttribute
            {
                color = _newColor,
                name = _newColorName,
            });
        }

        public void RemoveColorAt(int _index)
        {
            m_ColorAttributes.RemoveAt(_index);
        }

        public void RemoveLast()
        {
            RemoveColorAt(Count - 1);
        }
        
        public (Color, string) this[int _index]
        {
            get => m_ColorAttributes[_index];
            set => m_ColorAttributes[_index].Set(value);
        }

        public ColorAttribute GetAt(int _index)
        {
            return m_ColorAttributes[_index];
        }
        
        #if UNITY_EDITOR
        [HideInInspector]
        public bool IsDefaultFoldoutExpanded;
        [HideInInspector]
        public bool IsColorFoldoutExpanded;
        #endif
    }
}