using UnityEngine;

namespace Cavrnus.UI
{
    public class PopupHelper : MonoBehaviour
    {
        public void ClosePopup()
        {
            PopupCanvas.Instance.Close();
        }
    }
}