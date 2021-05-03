
using UnityEngine;

#if UNITY_EDITOR
using System.Reflection;
using UnityEditor;

namespace DynamicColorPalette._Editor.Utilities.UI
{
    public static class SerializedPropertyUtilities
    {
        public static Object GetInstanceOwner(this SerializedProperty property)
        {
            return property.serializedObject.targetObject;
        }
        
        public static T GetInstanceOwner<T>(this SerializedProperty property)
            where T : class 
        {
            return property.serializedObject.targetObject as T;
        }

        public static void CallOnValidateOnPropertyObject(this SerializedProperty property)
        {
            var target = property.serializedObject.targetObject;
            MethodInfo method = target.GetType().GetMethod("OnValidate", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            if (method != null)
            {
                method.Invoke(target, null);
            }
        }
    }
}
#endif