using NULL.Extensions.VisualElements;
using System;
using UnityEngine.UIElements;

namespace NULL.UI.Menus
{
    [Serializable]
    public class Choice : VisualElement
    {
        private readonly Label label;
        public Action OnClick = delegate { };

        public string Text { get => label.text; }
        public int Index { get => parent.IndexOf(this); }

        public Choice()
        {
            label = this.CreateChild<Label>("choice-text");
        }

        ~Choice()
        {
            // Remove all event listeners
            OnClick = null;
        }


        /// <summary>
        /// Add an event listener to the OnClick event
        /// </summary>
        public void AddOnClickListener(Action onClick) => OnClick += onClick;

        /// <summary>
        /// Remove an event listener from the OnClick event
        /// </summary>
        public void RemoveOnClickListener(Action onClick) => OnClick -= onClick;

        /// <summary>
        /// Set the text of the choice
        /// </summary>
        public void Set(string text)
        {
            label.text = text;
        }
    }
}
