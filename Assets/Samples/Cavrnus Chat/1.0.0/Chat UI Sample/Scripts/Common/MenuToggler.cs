using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Cavrnus.Chat
{
    public class MenuToggler : MonoBehaviour
    {
        [Serializable]
        private class MenuMap
        {
            public GameObject Menu;
            public Toggle MenuToggle;
            public bool IsOpenByDefault;
        }

        [SerializeField] private List<MenuMap> menuMap;
        
        private void Awake()
        {
            menuMap.ForEach(mm => {
                mm.Menu.SetActive(mm.IsOpenByDefault);
                mm.MenuToggle.SetIsOnWithoutNotify(mm.IsOpenByDefault);
                mm.MenuToggle.onValueChanged.AddListener(b => mm.Menu.SetActive(b));
            });
        }

        private void OnDestroy()
        {
            menuMap.ForEach(mm => {
                mm.MenuToggle.onValueChanged.RemoveListener(b => mm.Menu.SetActive(b));
            });
        }
    }
}