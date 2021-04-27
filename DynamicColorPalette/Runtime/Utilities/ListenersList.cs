using System;
using System.Collections.Generic;
using UnityEngine;

namespace DynamicColorPalette.Runtime.Utilities
{
    [Serializable]
    public class Listener
    {
        public UnityEngine.Object target;
        public string methodName;

        public static bool operator==(Listener _instance, Listener _other)
        {
            return Equals(_instance.target, _other.target) && _instance.methodName == _other.methodName;
        }

        public static bool operator !=(Listener _instance, Listener _other)
        {
            return !(_instance == _other);
        }

        public void Invoke(params object[] args)
        {
            target.GetType().GetMethod(methodName)?.Invoke(target, args);
        }
    }
    
    public class ListenersList
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
    }
}