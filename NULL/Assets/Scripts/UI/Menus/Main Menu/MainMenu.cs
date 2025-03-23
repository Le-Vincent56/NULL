using NULL.UI.Menus.Main.View;
using UnityEngine;

namespace NULL.UI.Menus.Main
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private MainMenuView view;

        private void Awake()
        {
            // Get components
            view = GetComponent<MainMenuView>();
        }
    }
}
