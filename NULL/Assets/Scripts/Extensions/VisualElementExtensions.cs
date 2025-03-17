using UnityEngine.UIElements;

namespace NULL.Extensions.VisualElements
{
    public static class VisualElementExtensions
    {
        /// <summary>
        /// Create a child VisualElement with the given classes
        /// </summary>
        public static VisualElement CreateChild(this VisualElement parent, params string[] classes)
        {
            VisualElement child = new VisualElement();
            child.AddClass(classes).AddTo(parent);
            return child;
        }

        /// <summary>
        /// Create a child VisualElement of type T with the given classes
        /// </summary>
        public static T CreateChild<T>(this VisualElement parent, params string[] classes) where T : VisualElement, new()
        {
            T child = new T();
            child.AddClass(classes).AddTo(parent);
            return child;
        }

        /// <summary>
        /// Add a VisualElement to a parent VisualElement
        /// </summary>
        public static T AddTo<T>(this T child, VisualElement parent) where T : VisualElement
        {
            parent.Add(child);
            return child;
        }

        /// <summary>
        /// Add classes to a VisualElement
        /// </summary>
        public static T AddClass<T>(this T visualElement, params string[] classes) where T : VisualElement
        {
            foreach (string cls in classes)
            {
                if (!string.IsNullOrEmpty(cls))
                {
                    visualElement.AddToClassList(cls);
                }
            }
            return visualElement;
        }

        /// <summary>
        /// Add a manipulator to a VisualElement
        /// </summary>
        public static T WithManipulator<T>(this T visualElement, IManipulator manipulator) where T : VisualElement
        {
            visualElement.AddManipulator(manipulator);
            return visualElement;
        }
    }
}
