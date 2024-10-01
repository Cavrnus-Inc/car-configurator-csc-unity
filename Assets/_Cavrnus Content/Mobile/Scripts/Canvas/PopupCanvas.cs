using UnityEngine;

namespace Cavrnus.UI
{
    public class PopupCanvas : MonoBehaviour
    {
        public static PopupCanvas Instance{ get; private set; }
        
        private GameObject currentPopup;

        [SerializeField] private GameObject scrim;

        private void Awake()
        {
            Instance = this;
        }

        public void OpenPopup(GameObject newPopup)
        {
            if (currentPopup != null) {
                Destroy(currentPopup);
            }

            scrim.SetActive(true);
            currentPopup = newPopup;
            newPopup.transform.SetParent(transform);
            
            // Reset the RectTransform values
            RectTransform rectTransform = newPopup.GetComponent<RectTransform>();
            if (rectTransform != null)
            {
                rectTransform.anchoredPosition = Vector2.zero;
                rectTransform.sizeDelta = Vector2.zero;
                rectTransform.localScale = Vector3.one;
            }
        }

        public void Close()
        {
            scrim.SetActive(false);
            if (currentPopup != null) {
                Destroy(currentPopup);
            }
        }
    }
}