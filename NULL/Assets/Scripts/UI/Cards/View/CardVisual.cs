using System.Text;
using System;
using UnityEngine;

namespace NULL.UI.Cards.View
{
    public class CardVisual : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private CardElement cardElement;
        [SerializeField] private Transform elementTransform;

        [Header("Fields - Follow")]
        [SerializeField] private float followSpeed;
        [SerializeField] private float curveYOffset;

        /// <summary>
        /// Initialize the card view
        /// </summary>
        public void Initialize(CardElement cardElement, int index)
        {
            this.cardElement = cardElement;
            elementTransform = cardElement.transform;
            transform.position = elementTransform.position;

            // Build the card's name
            StringBuilder nameBuilder = new StringBuilder();
            nameBuilder.Append("Card Visual ");
            nameBuilder.Append(index);

            // Set the card's name
            gameObject.name = nameBuilder.ToString();
        }

        public void Tick(float delta)
        {
            // Follow the card element
            Follow(delta);
        }

        private void Follow(float delta)
        {
            Vector3 verticalOffset = (Vector3.up * (cardElement.Dragging ? 0 : curveYOffset));
            transform.position = Vector3.Lerp(transform.position, elementTransform.position + verticalOffset, followSpeed * delta);
        }
    }
}
