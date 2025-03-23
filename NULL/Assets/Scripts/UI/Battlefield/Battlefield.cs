using NULL.Events;
using NULL.UI.Battleifled.View;
using UnityEngine;

namespace NULL.UI.Battlefield
{
    public class Battlefield : MonoBehaviour
    {
        private BattlefieldView view;

        private EventBinding<ShowBattlefield> onShowBattlefield;


        private void OnEnable()
        {
            onShowBattlefield = new EventBinding<ShowBattlefield>(ShowView);
            EventBus<ShowBattlefield>.Register(onShowBattlefield);
        }

        private void OnDisable()
        {
            EventBus<ShowBattlefield>.Deregister(onShowBattlefield);
        }

        private void ShowView()
        {

        }
    }
}
