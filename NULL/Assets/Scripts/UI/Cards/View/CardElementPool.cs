using UnityEngine;
using UnityEngine.Pool;

namespace NULL.UI.Cards.View
{
    public class CardElementPool
    {
        private readonly CardElement prefab;
        private readonly Transform parent;
        private readonly ObjectPool<CardElement> pool;

        public CardElementPool(CardElement prefab, Transform parent)
        {
            this.prefab = prefab;
            this.parent = parent;
            pool = new ObjectPool<CardElement>(CreateCard, GetCard, ReleaseCard, DestroyCard, true, 10, 20);
        }

        /// <summary>
        /// Get a card from the pool
        /// </summary>
        public CardElement Get() => pool.Get();

        /// <summary>
        /// Release a card back into the pool
        /// </summary>
        public void Release(CardElement card) => pool.Release(card);

        /// <summary>
        /// Callback function for creating a card instance within the pool
        /// </summary>
        private CardElement CreateCard()
        {
            // Instantiate a new card and initialize it
            CardElement card = Object.Instantiate(prefab, parent);
            return card;
        }

        /// <summary>
        /// Callback function for getting a card instance from the pool
        /// </summary>
        private void GetCard(CardElement card)
        {
            // Enable the card
            card.gameObject.SetActive(true);
        }

        /// <summary>
        /// Callback function for releasing a card instance back into the pool
        /// </summary>
        private void ReleaseCard(CardElement card)
        {
            // Disable the card
            card.gameObject.SetActive(false);
        }

        /// <summary>
        /// Callback function for destroying a card from the pool
        /// </summary>
        private void DestroyCard(CardElement card) => Object.Destroy(card.gameObject);
    }
}
