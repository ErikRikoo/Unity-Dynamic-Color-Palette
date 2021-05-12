using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DynamicColorPalette.Runtime.Utilities
{
    [Serializable]
    public class Listener
    {
        public UnityEngine.Object Target;
        public string MethodName;

        public static bool operator==(Listener _instance, Listener _other)
        {
            return Equals(_instance.Target, _other.Target) && _instance.MethodName == _other.MethodName;
        }

        public static bool operator !=(Listener _instance, Listener _other)
        {
            return !(_instance == _other);
        }

        public void Invoke(params object[] args)
        {
            Target.GetType().GetMethod(MethodName)?.Invoke(Target, args);
        }
    }
    
    [Serializable]
    public class ListenersList : IEnumerable<Listener>
    {
        [SerializeField] private List<Listener> m_Listeners = new List<Listener>();

        public void AddListener(Listener _newListener)
        {
            foreach (var listener in m_Listeners)
            {
                if (listener == _newListener)
                {
                    return;
                }
            }

            m_Listeners.Add(_newListener);
        }

        public void RemoveListener(Listener _listenerToRemove)
        {
            for (var i = 0; i < m_Listeners.Count; i++)
            {
                if (_listenerToRemove == m_Listeners[i])
                {
                    m_Listeners.RemoveAt(i);
                }
            }

            m_Listeners.Remove(_listenerToRemove);
        }
        
        public void Invoke(params object[] args)
        {
            foreach (var listener in m_Listeners)
            {
                listener.Invoke(args);
            }
        }

        public void RemoveAllListeners()
        {
            m_Listeners.Clear();
        }

        public IEnumerator<Listener> GetEnumerator()
        {
            return m_Listeners.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public bool Empty => Count == 0;

        public int Count => m_Listeners.Count;

        public void Clear()
        {
            m_Listeners.Clear();
        }
    }
}