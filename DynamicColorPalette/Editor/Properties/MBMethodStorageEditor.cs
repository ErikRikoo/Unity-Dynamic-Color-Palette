﻿using System.Collections.Generic;
using System.Reflection;
using Editor.Utilities;
using NUnit.Framework.Constraints;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking.PlayerConnection;
using UnityTemplateProjects;

namespace Editor
{
    [CustomPropertyDrawer(typeof(MBMethodStorage))]
    public class MBMethodStorageEditor : BasePropertyDrawer
    {
        
        private int Margin => 5;

        private int MinHeight => (int) EditorGUIUtility.singleLineHeight;
        
        public override void OnGUI(Rect position, SerializedProperty property,
            GUIContent label)
        {
            Vector2 startPosition = position.position;
            MBMethodStorage instance = GetInstance<MBMethodStorage>(property);
            float lineHeightMargined = MinHeight + Margin;
            float drawSpaceWidth = position.width;
            float indentationSpace = MinHeight * 0.5f;
            
            Rect foldoutRect = new Rect(startPosition, new Vector2(drawSpaceWidth, MinHeight));
            property.isExpanded = EditorGUI.Foldout(foldoutRect, property.isExpanded, label);
            startPosition.y += lineHeightMargined;
            
            MonoBehaviour owner = GetInstanceOwner<MonoBehaviour>(property);
            if (property.isExpanded)
            {
                ++EditorGUI.indentLevel;
                startPosition.x += indentationSpace;
                drawSpaceWidth -= indentationSpace;
                Component[] components = owner.gameObject.GetComponents<Component>();
                Rect firstPopupRect = new Rect(startPosition, new Vector2(drawSpaceWidth, MinHeight));
                int currentObjectSelected = -1;
                string[] componentsNames = GetObjectNamesWithCurrentlySelected(components, instance.Instance, ref currentObjectSelected);
                int selectedComponent = EditorGUI.Popup(firstPopupRect, "Component", currentObjectSelected, componentsNames);
                startPosition.y += lineHeightMargined;

                if (selectedComponent == -1)
                {
                    instance.Instance = null;
                    EditorUtility.SetDirty(GetInstanceOwner(property));
                }
                else if (instance.Instance != components[selectedComponent])
                {
                    instance.Instance = components[selectedComponent];
                    EditorUtility.SetDirty(GetInstanceOwner(property));
                }

                if (instance.Instance != null)
                {
                    int currentFunctionSelected = -1;
                    string[] functionNames =
                        GetValidFunctionName(instance.Instance, instance.MethodName, ref currentFunctionSelected);
                    Rect secondPopupRect = new Rect(startPosition, new Vector2(drawSpaceWidth, MinHeight));
                    int selectedFunction = EditorGUI.Popup(secondPopupRect, "Function", currentFunctionSelected, functionNames);

                    if (selectedFunction == -1)
                    {
                        instance.MethodName = null;
                        EditorUtility.SetDirty(GetInstanceOwner(property));
                    }
                    else if (instance.MethodName != functionNames[selectedFunction])
                    {
                        instance.MethodName = functionNames[selectedFunction];
                        EditorUtility.SetDirty(GetInstanceOwner(property));
                    }
                }

                drawSpaceWidth += indentationSpace;
                startPosition.x -= indentationSpace;
                --EditorGUI.indentLevel;
            }
        }

        private string[] GetValidFunctionName(Object selectedComponent, string currentFunction, ref int currentFunctionSelected)
        {
            if (selectedComponent == null)
            {
                return new string[0];
            }
            
            List<string> ret = new List<string>();
            MethodInfo[] methods = selectedComponent.GetType().GetMethods(BindingFlags.Public | BindingFlags.Instance);

            int index = 0;
            for (var i = 0; i < methods.Length; i++)
            {
                MethodInfo methodInfo = methods[i];
                var parameters = methodInfo.GetParameters();
                if (parameters.Length != 1)
                {
                    continue;
                }

                if(!typeof(Color).IsAssignableFrom(parameters[0].ParameterType))
                {
                    continue;
                }

                if (methodInfo.Name == currentFunction)
                {
                    currentFunctionSelected = ret.Count;
                }
                
                ret.Add(methodInfo.Name);
            }
            foreach (var methodInfo in methods)
            {
                var parameters = methodInfo.GetParameters();
                if (parameters.Length != 1)
                {
                    continue;
                }

                if(!typeof(Color).IsAssignableFrom(parameters[0].ParameterType))
                {
                    continue;
                }

                if (methodInfo.Name == currentFunction)
                {
                    
                }
                
                ret.Add(methodInfo.Name);
            }
            
            return ret.ToArray();
        }

        private string[] GetObjectNamesWithCurrentlySelected(Component[] components, UnityEngine.Object currentComponent, ref int current)
        {
            string[] ret = new string[components.Length];
            int index = 0;
            foreach (var component in components)
            {
                if (component == currentComponent)
                {
                    current = index;
                }

                ret[index] = component.GetType().Name;

                ++index;
            }

            return ret;
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if (!property.isExpanded)
            {
                return MinHeight + Margin;
            }

            if (GetInstance<MBMethodStorage>(property).Instance != null)
            {
                return (MinHeight + Margin) * 3;
            }

            return (MinHeight + Margin) * 2;
        }
    }
}