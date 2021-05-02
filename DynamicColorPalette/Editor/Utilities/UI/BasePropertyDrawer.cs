 using DynamicColorPalette.Editor.Utilities.UI;
using UnityEngine;

 #if UNITY_EDITOR
 using UnityEditor;

namespace Editor.Utilities.UI
{
    public abstract class BasePropertyDrawer : PropertyDrawer
    {
        private SerializedProperty m_CurrentProperty;
        
        protected abstract void DrawElements(Rect position, SerializedProperty property, GUIContent label);
        
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            m_CurrentProperty = property;
            m_DrawingRect = position;
            m_IndentLevel = 0;

            DrawElements(position, property, label);
        }


        #region Layout

        private Rect m_DrawingRect;

        public Vector2 CurrentDrawingStart => m_DrawingRect.position;

        public float Width => m_DrawingRect.width;

        private int m_IndentLevel;
        
        public int IndentLevel
        {
            get => m_IndentLevel;
            set
            {
                int delta = value - m_IndentLevel;
                m_DrawingRect.position += new Vector2(delta * Tabulation, 0);
                m_DrawingRect.width += delta * Tabulation;
                m_IndentLevel = value;
            }
        }

        public Rect GetLineDrawingRect()
        {
            Rect ret = new Rect(m_DrawingRect.x, m_DrawingRect.y, m_DrawingRect.width, MinHeight);
            //TODO: extract and update height
            m_DrawingRect.position += new Vector2(0, MinHeight + Margin);
            return ret;
        }

        public void OnLineDrawn()
        {
            m_DrawingRect.position += new Vector2(0, MinHeight);
        }
        
        public void OnLineDrawnWithMargin()
        {
            m_DrawingRect.position += new Vector2(0, MinHeight + Margin);
        }

        public void OnDraw(float _verticalSpaceTaken)
        {
            m_DrawingRect.position += new Vector2(0, _verticalSpaceTaken);
        }

        #endregion

        #region Utilities

        protected Object InstanceOwner => m_CurrentProperty.GetInstanceOwner();

        protected T GetInstanceOwner<T>()
            where T : class
        {
            return m_CurrentProperty.GetInstanceOwner<T>();
        }

        protected T GetInstance<T>()
            where T : class
        {
            return fieldInfo.GetValue(m_CurrentProperty.serializedObject.targetObject) as T;
        }
        
        protected T GetInstance<T>(SerializedProperty _property)
            where T : class
        {
            return fieldInfo.GetValue(_property.serializedObject.targetObject) as T;
        }
        
        protected void CallOnValidateOnPropertyObject()
        {
            m_CurrentProperty.CallOnValidateOnPropertyObject();
        }

        protected void OnInstanceChanged()
        {
            EditorUtility.SetDirty(InstanceOwner);
        }
        
        #endregion

        #region Settings
        
        protected virtual int Margin => 5;

        protected virtual int MinHeight => (int) EditorGUIUtility.singleLineHeight;

        protected virtual int Tabulation => MinHeight;

        #endregion
    }
}

#endif