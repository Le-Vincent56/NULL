using NULL.UI.Cards.View;
using UnityEngine;
using UnityEngine.Pool;

namespace NULL
{
    public class CardVisualPool
    {
        private readonly CardVisual prefab;
        private readonly Transform parent;
        private readonly ObjectPool<CardVisual> pool;

        public CardVisualPool(CardVisual prefab, Transform parent)
        {
            this.prefab = prefab;
            this.parent = parent;
            pool = new ObjectPool<CardVisual>(CreateCard, GetCard, ReleaseCard, DestroyCard, true, 10, 20);
        }

        /// <summary>
        /// Get a card visual from the pool
        /// </summary>
        public CardVisual Get() => pool.Get();

        /// <summary>
        /// Release a card visual back into the pool
        /// </summary>
        public void Release(CardVisual card) => pool.Release(card);

        /// <summary>
        /// Callback function for creating a card visual instance within the pool
        /// </summary>
        private CardVisual CreateCard()
        {
            // Instantiate a new card and initialize it
            CardVisual card = Object.Instantiate(prefab, parent);
            return card;
        }

        /// <summary>
        /// Callback function for getting a card visual instance from the pool
        /// </summary>
        private void GetCard(CardVisual card)
        {
            // Enable the card
            card.gameObject.SetActive(true);
        }

        /// <summary>
        /// Callback function for releasing a card visual instance back into the pool
        /// </summary>
        private void ReleaseCard(CardVisual card)
        {
            // Disable the card
            card.gameObject.SetActive(false);
        }

        /// <summary>
        /// Callback function for destroying a card visual from the pool
        /// </summary>
        private void DestroyCard(CardVisual card) => Object.Destroy(card.gameObject);
    }
}
