﻿using UnityEditor;
using UnityEngine;

namespace Editor.Utilities
{
    public abstract class BasePropertyDrawer : PropertyDrawer
    {
        public static Object GetInstanceOwner(SerializedProperty property)
        {
            return property.serializedObject.targetObject;
        }
        
        public static T GetInstanceOwner<T>(SerializedProperty property)
            where T : class 
        {
            return property.serializedObject.targetObject as T;
        }

        public T GetInstance<T>(SerializedProperty property)
            where T : class
        {
            return fieldInfo.GetValue(property.serializedObject.targetObject) as T;
        }
    }
}