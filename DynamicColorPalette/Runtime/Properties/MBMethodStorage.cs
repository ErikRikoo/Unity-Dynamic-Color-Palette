using System;
using UnityEngine;

namespace DynamicColorPalette.Runtime.Properties
{
    [Serializable]
    public class MBMethodStorage
    {
        [SerializeField] public UnityEngine.Object Instance;
        [SerializeField] public string MethodName;

        public void Invoke(params object[] args)
        {
            Instance?.GetType().GetMethod(MethodName)?.Invoke(Instance, args);
        }
    }
}