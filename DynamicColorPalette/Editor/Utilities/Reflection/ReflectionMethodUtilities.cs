using System;


#if UNITY_EDITOR
using System.Collections.Generic;
using System.Reflection;

namespace DynamicColorPalette._Editor.Utilities.Reflection
{
    public static class ReflectionMethodUtilities
    {
        public static MethodInfo[] GetPublicInstanceMethods<T>(this T _instance)
        {
            return _instance.GetType().GetMethods(BindingFlags.Public | BindingFlags.Instance);
        }

        public static string[] FilterAndGetNames(this MethodInfo[] _methods, Predicate<MethodInfo> _predicate)
        {
            List<string> ret = new List<string>();
            foreach (var methodInfo in _methods)
            {
                if (_predicate(methodInfo))
                {
                    ret.Add(methodInfo.Name);
                }
            }
            return ret.ToArray();
        }
        
        public static string[] FilterAndGetTypeNames<T>(this T[] _elements, Predicate<T> _predicate)
        {
            List<string> ret = new List<string>();
            foreach (var element in _elements)
            {
                if (_predicate(element))
                {
                    ret.Add(element.GetType().Name);
                }
            }
            return ret.ToArray();
        }
    }
}

#endif