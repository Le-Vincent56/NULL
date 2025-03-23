using UnityEngine;

namespace NULL.UI.Cards.Model
{
    public class CardModel
    {
        private readonly CardData data;

        public CardModel(CardData data)
        {
            this.data = data;
            Cost = data.Cost;
            Intensity = data.Intensity;
            Integrity = data.Integrity;
            Reflect = data.Reflect;
            Project = data.Project;
        }

        public Sprite Sprite { get => data.Sprite; }
        public string Name { get => data.Name; }
        public int Cost { get; set; }
        public int Intensity { get; set; }
        public int Integrity { get; set;  }
        public string Reflect { get; set; }
        public string Project { get; set; }

        /// <summary>
        /// Apply the card's Reflection effect
        /// </summary>
        public void ApplyReflect()
        {
            Debug.Log($"Applying {Name}'s Reflection effect");
        }

        /// <summary>
        /// Apply the card's Projection effect
        /// </summary>
        public void ApplyProject()
        {
            Debug.Log($"Applying {Name}'s Projection effect");
        }
    }
}
