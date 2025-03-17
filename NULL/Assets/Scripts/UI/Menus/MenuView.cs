using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

namespace NULL.UI.Menus
{
    public abstract class MenuView : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] protected UIDocument document;
        [SerializeField] protected StyleSheet styleSheet;
        protected VisualElement root;
        protected VisualElement container;

        [Header("Fields")]
        [SerializeField] protected Choice[] choices;

        public abstract IEnumerator InitializeView(int size = 3);
    }
}
