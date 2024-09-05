using System;
using TMPro;
using UnityEngine;

namespace Cavrnus.UI
{
    public class DeviceSelectorPopupEntry : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI deviceNameTextMeshPro;

        private int id;
        private string deviceName;
        private Action<int> onSelect;
        
        public void Setup(int id, string deviceName, Action<int> onSelect)
        {
            this.id = id;
            this.deviceName = deviceName;
            this.onSelect = onSelect;

            deviceNameTextMeshPro.text = deviceName;
        }

        public void OnPointerClick()
        {
            onSelect?.Invoke(id);
        }
    }
}