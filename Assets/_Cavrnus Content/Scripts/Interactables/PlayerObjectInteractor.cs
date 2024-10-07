using CavrnusDemo.Interactables;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CavrnusSdk.Interactables
{
    public class PlayerObjectInteractor : MonoBehaviour
    {
        [SerializeField] private Image reticle;
        [SerializeField] private Color interactableColor = Color.blue;
        [SerializeField] private Color defaultColor = Color.white;
        
        public LayerMask layerMask;
        
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
            var mousePosition = Input.mousePosition;

            // Check if mouse position is within screen bounds
            if (mousePosition.x >= 0 && mousePosition.x <= Screen.width && mousePosition.y >= 0 &&
                mousePosition.y <= Screen.height) {

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
                
                if (!EventSystem.current.IsPointerOverGameObject())
                {
                    var ray = mainCam.ScreenPointToRay(mousePosition);
        
                    if (Physics.Raycast(ray, out var hit, 100, layerMask)) {
                        if (hit.transform.gameObject.TryGetComponent(out ICustomInteractable interactable)) {
                            if (Input.GetMouseButtonDown(0)) {
                                interactable.Interact();
                            }

                            reticle.color = interactableColor;
                        }
                    }
                    else {
                        reticle.color = defaultColor;
                    }
                }
            }
        }
    }
}