using System;
using DynamicColorPalette.Runtime.Properties;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.PlayerLoop;

namespace DynamicColorPalette.Runtime.Linkers
{
    [RequireComponent(typeof(Renderer))]
    public class RendererColorLinker : AColorLinker
    {
       private Renderer m_Renderer;

        protected override void Initialization()
        {
            if (m_Renderer == null)
            {
                m_Renderer = GetComponent<Renderer>();
            }
        }

        protected override UnityAction<Color> GetAction()
        {
            return OnColorUpdated;
        }


        public void OnColorUpdated(Color _newColor)
        {
            m_Renderer.sharedMaterial.color = _newColor;
        }
    }
}