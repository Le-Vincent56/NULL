using UnityEngine;

namespace NULL.UI.Cards.Model
{
    [CreateAssetMenu(fileName = "Card Data", menuName = "Data/Cards")]
    public class CardData : ScriptableObject
    {
        [field: SerializeField] public Sprite Sprite { get; private set; }
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public int Cost { get; private set; }
        [field: SerializeField] public int Intensity { get; private set; }
        [field: SerializeField] public int Integrity { get; private set; }
        [field: SerializeField] public string Reflect { get; private set; }
        [field: SerializeField] public string Project { get; private set; }
    }
}
