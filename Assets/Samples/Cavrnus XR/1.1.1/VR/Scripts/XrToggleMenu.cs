using CavrnusSdk.API;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CavrnusSdk.XR
{
    public class XrToggleMenu : MonoBehaviour
    {
        [SerializeField] private GameObject targetMenu;
        [SerializeField] private InputAction inputAction;
        
        private void Awake()
        {
            CavrnusFunctionLibrary.AwaitAnySpaceConnection(sc => {
                inputAction.Enable();
                inputAction.performed += Pressed;
            });
        }

        private void Pressed(InputAction.CallbackContext obj)
        {
            targetMenu.SetActive(!targetMenu.activeSelf);
        }

        private void OnDestroy()
        {
            inputAction.performed -= Pressed;
        }
    }
}