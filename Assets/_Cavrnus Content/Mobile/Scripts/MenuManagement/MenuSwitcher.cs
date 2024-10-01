using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Cavrnus.UI.MenuManagement
{
    public class MenuSwitcher : MonoBehaviour
    {
        [Serializable]
        private class MenuToToggleMap
        {
            public GameObject Menu;
            public Toggle Toggle;
            public bool IsDefaultOpen;
        }
        
        [SerializeField] private List<MenuToToggleMap> menus;

        private void Awake()
        {
            foreach (var item in menus) {
                item.Toggle.isOn = item.IsDefaultOpen;
                item.Menu.SetActive(item.IsDefaultOpen);
                
                item.Toggle.onValueChanged.AddListener(isActive => {
                    item.Menu.SetActive(isActive);
                });
            }
        }
    }
}