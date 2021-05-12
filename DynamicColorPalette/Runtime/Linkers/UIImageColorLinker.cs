using DynamicColorPalette.Runtime.Properties;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace DynamicColorPalette.Runtime.Linkers
{
    [RequireComponent(typeof(Image))]
    public class UIImageColorLinker : AColorLinker
    {
        private Image m_Image;

        protected override void Initialization()
        {
            if (m_Image == null)
            {
                m_Image = GetComponent<Image>();
            }
        }

        protected override UnityAction<Color> GetAction()
        {
            return OnColorUpdated;
        }

        public void OnColorUpdated(Color _color)
        {
            m_Image.color = _color;
            EditorUtility.SetDirty(m_Image);
            //EditorApplication.QueuePlayerLoopUpdate();
        }
    }
}