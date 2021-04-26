﻿using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;
using UnityEngine.Events;

namespace UnityTemplateProjects
{
    [Serializable]
    public class ColorEvent : UnityEvent<Color>
    {
        
    }
    
    
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
            //listeners.RemoveListener(_newListener);
            listeners.AddListener(_newListener);
        }
        
        public void RemoveListener(Listener _listenerToRemove)
        {
            listeners.RemoveListener(_listenerToRemove);
        }
    }
    
    
    [CreateAssetMenu(fileName = "ColorPalette", menuName = "ColorPalette", order = 0)]
    public class ColorPalette : ScriptableObject
    {
        [SerializeField] private List<ColorAttribute> m_ColorAttributes = new List<ColorAttribute>();

        public int Count => m_ColorAttributes.Count;

        public bool IsEmpty => Count == 0;
        
        public void AddColor(Color _newColor, string _newColorName)
        {
            m_ColorAttributes.Add(new ColorAttribute
            {
                color = _newColor,
                name = _newColorName,
            });
        }
        
        public void AddColorListener(int _index, Listener _newListener)
        {
            m_ColorAttributes[_index].AddListener(_newListener);
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
    }
}