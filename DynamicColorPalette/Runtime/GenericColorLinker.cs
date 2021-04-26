﻿using UnityEngine;

namespace UnityTemplateProjects
{
    public class GenericColorLinker : MonoBehaviour
    {
        [SerializeField] private ColorLink m_ColorLink;
        [SerializeField] private MBMethodStorage m_MethodStorage;
        
        
        private void OnValidate()
        {
            if (m_ColorLink == null)
            {
                m_ColorLink = new ColorLink(OnColorUpdated);
            }
        }

        public void OnColorUpdated(Color _newColor)
        {
            m_MethodStorage?.Invoke(_newColor);
        }
    }
}