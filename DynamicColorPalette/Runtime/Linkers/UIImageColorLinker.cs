using DynamicColorPalette.Runtime.Properties;
using UnityEngine;
using UnityEngine.UI;

namespace DynamicColorPalette.Runtime.Linkers
{
    [RequireComponent(typeof(Image))]
    public class UIImageColorLinker : MonoBehaviour
    {
        [SerializeField] private ColorLink m_ColorLink;
        
        private Image m_Image;
        
        private void OnValidate()
        {
            if (m_Image == null)
            {
                m_Image = GetComponent<Image>();
            }
            
            if (m_ColorLink == null)
            {
                m_ColorLink = new ColorLink(OnColorUpdated);
            }
        }

        public void OnColorUpdated(Color _color)
        {
            Color32 _new = _color;
            m_Image.color = _new;
            //m_Image.SetAllDirty();
        }
    }
}