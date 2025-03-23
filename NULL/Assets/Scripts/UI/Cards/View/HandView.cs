using NULL.Extensions.List;
using System.Collections.Generic;
using UnityEngine;

namespace NULL.UI.Cards.View
{
    public class HandView : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private CardElement elementPrefab;
        [SerializeField] private Transform elementParent;
        [SerializeField] private CardVisual visualPrefab;
        [SerializeField] private Transform visualParent;
        [SerializeField] private CardElement selectedCard;
        [SerializeField] private CardElement hoveredCard;
        private CardElementPool elementPool;
        private CardVisualPool visualPool;
        private RectTransform rect;
        private Camera mainCamera;

        [Header("Fields")]
        [SerializeField] private bool isCrossing;
        [SerializeField] private int cardsToSpawn;
        [SerializeField] private List<(CardElement Element, CardVisual Visual)> cards;

        private float time;
        private float delta;

        private void Awake()
        {
            // Get components
            rect = GetComponent<RectTransform>();
            mainCamera = Camera.main;

            // Initialize the card pool
            elementPool = new CardElementPool(elementPrefab, elementParent);
            visualPool = new CardVisualPool(visualPrefab, visualParent);

            // Create a list to store the cards
            cards = new List<(CardElement element, CardVisual visual)>();

            // Loop through the number of cards to spawn
            for (int i = 0; i < cardsToSpawn; i++)
            {
                // Add a card to the list
                AddCard(i);
            }

            // Iterate through each card
            foreach((CardElement Element, CardVisual Visual) card in cards)
            {
                // Subscribe to events
                card.Element.BeginDrag += BeginCardDrag;
                card.Element.EndDrag += EndCardDrag;
            }
        }

        private void Update()
        {
            // Update time variables
            time = Time.time;
            delta = Time.deltaTime;

            // Iterate throughe each card
            foreach ((CardElement Element, CardVisual Visual) card in cards)
            {
                // Tick the card
                card.Element.Tick(delta);
                card.Visual.Tick(delta);
            }

            // Exit case - there is no selected card
            if (selectedCard == null) return;

            // Exit case - currently is crossing over with another card
            if (isCrossing) return;

            bool indicesChanged = false;
            int oldIndex = selectedCard.Index;
            int newIndex = selectedCard.Index;

            // Iterate through each card
            for (int i = 0; i < cards.Count; i++)
            {
                // Check if the selected card is to the right of the iterated card
                if (selectedCard.transform.position.x > cards[i].Element.transform.position.x)
                {
                    // Skip if the selected card index does not need to be swapped
                    if (selectedCard.Index >= cards[i].Element.Index) continue;

                    // Set movement indices
                    oldIndex = selectedCard.Index;
                    newIndex = cards[i].Element.Index;

                    // Notify that the indices have changed
                    indicesChanged = true;
                }

                // Check if the selected card is to the left of the iterated card
                if (selectedCard.transform.position.x < cards[i].Element.transform.position.x)
                {
                    if (selectedCard.Index <= cards[i].Element.Index) continue;

                    // Set movement indices
                    oldIndex = selectedCard.Index;
                    newIndex = cards[i].Element.Index;

                    // Notify that the indices have changed
                    indicesChanged = true;
                }
            }

            // Exit case - indices have not changed
            if (!indicesChanged) return;

            // Move the selected card to the new index
            cards.Move(oldIndex, newIndex);

            // Iterate through each card
            for (int i = 0; i < cards.Count; i++)
            {
                // Update their index
                cards[i].Element.Index = i;
                cards[i].Element.transform.SetSiblingIndex(i);
            }
        }

        private void AddCard(int index)
        {
            // Get a card from the pool
            CardElement element = elementPool.Get();

            // Initialize the card element
            element.Initialize(mainCamera, index);

            // Get the card's visual
            CardVisual visual = visualPool.Get();

            // Initialize the visual
            visual.Initialize(element, index);

            // Add the card to the list
            cards.Add((element, visual));
        }

        private void BeginCardDrag(CardElement card)
        {
            // Set the selected card
            selectedCard = card;
        }

        private void EndCardDrag(CardElement card)
        {
            // Exit case - there is no selected card
            if (selectedCard == null) return;

            rect.sizeDelta += Vector2.right;
            rect.sizeDelta -= Vector2.right;

            // Reset the selected card
            selectedCard = null;
        }
    }
}
