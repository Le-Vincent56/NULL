using NULL.Extensions.VisualElements;
using System;
using UnityEngine.UIElements;

namespace NULL.UI.Menus
{
    [Serializable]
    public class Choice : Button
    {
        private readonly Label label;

        public string Text { get => label.text; }
        public int Index { get => parent.IndexOf(this); }

        public Choice()
        {
            label = this.CreateChild<Label>("choice-text");
        }

        /// <summary>
        /// Add an event listener to the OnClick event
        /// </summary>
        public void AddOnClickListener(Action onClick) => clicked += onClick;

        /// <summary>
        /// Remove an event listener from the OnClick event
        /// </summary>
        public void RemoveOnClickListener(Action onClick) => clicked -= onClick;

        /// <summary>
        /// Set the text of the choice
        /// </summary>
        public void Set(string text)
        {
            label.text = text;
        }
    }
}
