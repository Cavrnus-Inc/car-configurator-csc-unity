using CavrnusSdk.UI;
using UnityEngine;

namespace CavrnusSdk.XR
{
    public class CloseAllMenus : MonoBehaviour
    {
        private MenuManager menuManager;
        private void Awake()
        {
            menuManager = GetComponentInParent<MenuManager>();
            if (menuManager == null) {
                Debug.Log($"Missing {nameof(MenuManager)} in parent! Close button will not work!");
            }
        }

        public void CloseMenus()
        {
            if (menuManager != null)
                menuManager.CloseAllMenus();
        }
    }
}