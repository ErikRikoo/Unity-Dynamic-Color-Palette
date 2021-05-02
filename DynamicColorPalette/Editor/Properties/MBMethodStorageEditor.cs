using System;
using DynamicColorPalette.Runtime.Properties;
using UnityEngine;


#if UNITY_EDITOR
using UnityEditor;
using Editor.Utilities.UI;
using DynamicColorPalette.Editor.Utilities.Reflection;
using Object = UnityEngine.Object;

namespace DynamicColorPalette.Editor.Properties
{
    [CustomPropertyDrawer(typeof(MBMethodStorage))]
    public class MBMethodStorageEditor : BasePropertyDrawer
    {
        protected override void DrawElements(Rect position, SerializedProperty property, GUIContent label)
        {
            property.isExpanded = EditorGUI.Foldout(GetLineDrawingRect(), property.isExpanded, label);
            if (property.isExpanded)
            {
                ++IndentLevel;
                DrawComponentField();
                DrawMethodField();
                --IndentLevel;
            }
        }

        private void DrawComponentField()
        {
            MonoBehaviour owner = GetInstanceOwner<MonoBehaviour>();
            MBMethodStorage instance = GetInstance<MBMethodStorage>();

            Component[] components = owner.gameObject.GetComponents<Component>();
            string[] componentsNames = GetValidComponentNames(components);
            int objectSelectedIndex = Array.FindIndex(components, val => val == instance.Instance);

            int selectedComponent = EditorGUI.Popup(GetLineDrawingRect(), "Component", objectSelectedIndex,
                componentsNames);

            Object componentValue = selectedComponent == -1 ? null : components[selectedComponent];
            if (componentValue != instance.Instance)
            {
                instance.Instance = componentValue;
                OnInstanceChanged();
            }
        }

        private void DrawMethodField()
        {
            MBMethodStorage instance = GetInstance<MBMethodStorage>();
            if (instance.Instance != null)
            {
                string[] functionNames =
                    GetValidFunctionName(instance);
                int currentFunctionSelected = Array.FindIndex(functionNames, s => s == instance.MethodName);

                int selectedFunctionIndex = EditorGUI.Popup(GetLineDrawingRect(), "Function", currentFunctionSelected,
                    functionNames);

                string functionValue = selectedFunctionIndex == -1 ? null : functionNames[selectedFunctionIndex];
                if (functionValue != instance.MethodName)
                {
                    instance.MethodName = functionValue;
                    OnInstanceChanged();
                }
            }
        }
        
        private string[] GetValidComponentNames(Component[] components)
        {
            Object self = GetInstanceOwner<Object>();
            return components.FilterAndGetTypeNames(component => component != self);
        }
        
        private string[] GetValidFunctionName(MBMethodStorage _instance)
        {
            if (_instance.Instance == null)
            {
                return new string[0];
            }

            return _instance.Instance.GetPublicInstanceMethods().FilterAndGetNames(methodInfo =>
            {
                var parameters = methodInfo.GetParameters();
                return parameters.Length == 1 && typeof(Color).IsAssignableFrom(parameters[0].ParameterType);
            });
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
#endif