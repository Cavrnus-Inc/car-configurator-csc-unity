using CavrnusDemo;
using UnityEngine;
using UnityEngine.UI;

namespace CavrnusSdk.Common
{
    public class PlayerObjectInteractor : MonoBehaviour
    {
        [SerializeField] private Image reticle;
        [SerializeField] private Color interactableColor = Color.blue;
        [SerializeField] private Color defaultColor = Color.white;
        
        private Camera mainCam;
        private void Awake()
        {
            mainCam = Camera.main;
            reticle.gameObject.SetActive(false);
            reticle.color = defaultColor;
        }

        private bool wasSet = false;
        private void Update()
        {
            if (Input.GetMouseButton(1)) {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                wasSet = true;
                reticle.gameObject.SetActive(true);
            }

            if (wasSet && !Input.GetMouseButton(1)) {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                wasSet = false;
                reticle.gameObject.SetActive(false);
            }
            
            var ray = mainCam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out var hit, 100)) {
                if (hit.transform.gameObject.TryGetComponent(out ICustomInteractable interactable)) {
                    if (Input.GetMouseButtonDown(0)) {
                        interactable.Interact();
                    }
                    reticle.color = interactableColor;
                }
            }
            else
            {
                reticle.color = defaultColor;
            }
        }
    }
}