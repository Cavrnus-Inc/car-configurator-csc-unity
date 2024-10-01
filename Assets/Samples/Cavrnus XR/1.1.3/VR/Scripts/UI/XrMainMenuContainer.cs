using CavrnusSdk.UI;
using UnityEngine;

namespace CavrnusSdk.XR.UI
{
    public class XrMainMenuContainer : MonoBehaviour
    {
        [SerializeField] private Transform menusContainer;
        
        public void Setup()
        {
            gameObject.SetActive(true);
            CavrnusMenuManager.Instance.OpenMenu("LoginMenu", menusContainer);
        }
    }
}