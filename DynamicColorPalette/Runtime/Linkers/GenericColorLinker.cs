 using DynamicColorPalette.Runtime.Properties;
 using UnityEngine;
 using UnityEngine.Events;

 namespace DynamicColorPalette.Runtime.Linkers
{
    public class GenericColorLinker : AColorLinker
    {
        [SerializeField] private MBMethodStorage m_MethodStorage;

        protected override UnityAction<Color> GetAction()
        {
            return OnColorUpdated;
        }

        public void OnColorUpdated(Color _newColor)
        {
            m_MethodStorage?.Invoke(_newColor);
        }
    }
}