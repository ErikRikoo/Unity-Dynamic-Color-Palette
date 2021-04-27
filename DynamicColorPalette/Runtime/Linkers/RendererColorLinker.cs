using System;
using DynamicColorPalette.Runtime.Properties;
using UnityEngine;

namespace DynamicColorPalette.Runtime.Linkers
{
    [RequireComponent(typeof(Renderer))]
    public class RendererColorLinker : MonoBehaviour
    {
        [SerializeField] private ColorLink m_ColorLink;
        
        private Renderer m_Renderer;

        private void OnValidate()
        {
            if (m_Renderer == null)
            {
                m_Renderer = GetComponent<Renderer>();
            }
            
            if (m_ColorLink == null)
            {
                m_ColorLink = new ColorLink(OnColorUpdated);
            }
        }

        public void OnColorUpdated(Color _color)
        {
            m_Renderer.sharedMaterial.color = _color;
        }
    }
}