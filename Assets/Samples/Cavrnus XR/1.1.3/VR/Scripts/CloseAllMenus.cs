using CavrnusSdk.UI;
using UnityEngine;

namespace CavrnusSdk.XR
{
    public class CloseAllMenus : MonoBehaviour
    {
        private CavrnusMenuManager cavrnusMenuManager;
        private void Awake()
        {
            cavrnusMenuManager = GetComponentInParent<CavrnusMenuManager>();
            if (cavrnusMenuManager == null) {
                Debug.Log($"Missing {nameof(CavrnusMenuManager)} in parent! Close button will not work!");
            }
        }

        public void CloseMenus()
        {
            if (cavrnusMenuManager != null)
                cavrnusMenuManager.CloseAllMenus();
        }
    }
}