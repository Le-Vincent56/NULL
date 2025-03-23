using System;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace NULL.UI.Cards.View
{
    public class CardElement : MonoBehaviour,
            IDragHandler, IBeginDragHandler, IEndDragHandler,
            IPointerEnterHandler, IPointerExitHandler, IPointerUpHandler, IPointerDownHandler
    {
        [Header("References")]
        [SerializeField] private Camera mainCamera;
        private LayoutElement layoutElement;

        [Header("Fields")]
        [SerializeField] private bool hovering;
        [SerializeField] private bool dragging;
        [SerializeField] private float moveSpeedMax;

        private Vector3 offset;

        public int Index { get; set; }
        public bool Dragging { get => dragging; }

        public event Action<CardElement> PointerEnter = delegate { };
        public event Action<CardElement> PointerExit = delegate { };
        public event Action<CardElement, bool> PointerUp = delegate { };
        public event Action<CardElement> PointerDown = delegate { };
        public event Action<CardElement> BeginDrag = delegate { };
        public event Action<CardElement> Drag = delegate { };
        public event Action<CardElement> EndDrag = delegate { };

        /// <summary>
        /// Initialize the card element
        /// </summary>
        public void Initialize(Camera mainCamera, int index)
        {
            // Get components
            layoutElement = GetComponent<LayoutElement>();

            // Cache the main camera
            this.mainCamera = mainCamera;

            // Set the index
            Index = index;

            // Build the card's name
            StringBuilder nameBuilder = new StringBuilder();
            nameBuilder.Append("Card Element ");
            nameBuilder.Append(index);

            // Set the card's name
            gameObject.name = nameBuilder.ToString();
        }

        public void Tick(float delta)
        {
            // Exit case - not dragging
            if (!dragging) return;

            // Clamp the card's position within the screen bounds
            ClampPosition();

            // Calculate the target position based on the mouse position and offset
            Vector2 targetPosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()) - offset;

            // Calculate the direction vector from the current position to the target position
            Vector2 direction = (targetPosition - (Vector2)transform.position).normalized;

            // Calculate the velocity vector based on the direction and move speed
            Vector2 velocity = direction * Mathf.Min(moveSpeedMax, Vector2.Distance(transform.position, targetPosition) / delta);

            // Move the card by the calculated velocity
            transform.Translate(velocity * delta);
        }

        /// <summary>
        /// Clamp the position of the card element
        /// </summary>
        private void ClampPosition()
        {
            // Get the screen bounds in world coordinates
            Vector2 screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));

            // Get the current position of the card
            Vector3 clampedPosition = transform.position;

            // Clamp the x position within the screen bounds
            clampedPosition.x = Mathf.Clamp(clampedPosition.x, -screenBounds.x, screenBounds.x);

            // Clamp the y position within the screen bounds
            clampedPosition.y = Mathf.Clamp(clampedPosition.y, -screenBounds.y, screenBounds.y);

            // Update the card's position with the clamped values
            transform.position = new Vector3(clampedPosition.x, clampedPosition.y, 0);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            // Notify begin dragging
            BeginDrag.Invoke(this);

            // Set to dragging
            dragging = true;

            // Ignore layouts
            layoutElement.ignoreLayout = true;

            // Calculate the offset between the mouse position and the card's position
            Vector2 mousePosition = mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            offset = mousePosition - (Vector2)transform.position;
        }

        public void OnDrag(PointerEventData eventData)
        {
            // Invoke the dragging event
            Drag.Invoke(this);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            // Notify the end of dragging
            EndDrag.Invoke(this);

            // Reset the layout ignoring
            layoutElement.ignoreLayout = false;

            // Set to not dragging
            dragging = false;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            // Notify the pointer enter event
            PointerEnter.Invoke(this);

            // Set to hovering
            hovering = true;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            // Notify the pointer exit event
            PointerExit.Invoke(this);

            // Set to not hovering
            hovering = false;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            // Exit case - not dragging
            if (!dragging) return;

            // Notify the pointer up event
            PointerUp.Invoke(this, hovering);

            // Reset the card's local position
            transform.localPosition = Vector3.zero;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
        }
    }
}
