using System;
using DynamicColorPalette.Runtime.Properties;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.PlayerLoop;

namespace DynamicColorPalette.Runtime.Linkers
{
    [ExecuteInEditMode]

    public abstract class AColorLinker : MonoBehaviour
    {
        [SerializeField] private ColorLink m_ColorLink;

        private void OnValidate()
        {
            Initialization();
            if (m_ColorLink == null)
            {
                m_ColorLink = new ColorLink(GetAction());
            }
        }

        /// <summary>
        /// This function is called at the beginning of OnValidate.
        /// You can use it to cache anything you will need later.
        /// </summary>
        protected virtual void Initialization() {}

        /// <summary>
        /// Abstract method to get the function which should be called.
        /// </summary>
        /// <returns>The listener</returns>
        protected abstract UnityAction<Color> GetAction();

        private void OnDestroy()
        {
            m_ColorLink.OnDestroy();
        }
    }
}