using NULL.UI.Menus;
using UnityEngine;

namespace NULL
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private MainMenuView view;

        private void Awake()
        {
            StartCoroutine(view.InitializeView(3));
        }
    }
}
