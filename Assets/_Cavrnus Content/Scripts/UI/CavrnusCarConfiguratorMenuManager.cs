using System;
using System.Collections;
using System.Collections.Generic;
using CavrnusCore;
using UnityEngine;
using UnityEngine.UI;

namespace CavrnusDemo
{
    public class CavrnusCarConfiguratorMenuManager : MonoBehaviour
    {
        [Serializable]
        private class ButtonMenu
        {
            public event Action<ButtonMenu> OnToggleClicked;
            public int Id{ get; private set; }
            public Toggle Toggle;
            public GameObject Menu;

            public void Setup(int id)
            {
                Id = id;
                Toggle.onValueChanged.AddListener(ToggleValueChanged);
                Menu.SetActive(false);
            }

            public void SetState(bool state)
            {
                Menu.SetActive(state);
                Toggle.SetIsOnWithoutNotify(state);
            }

            private void ToggleValueChanged(bool val)
            {
                OnToggleClicked?.Invoke(this);
            }

            public void Teardown()
            {
                Toggle.onValueChanged.RemoveAllListeners();
            }
        }

        [SerializeField] private List<ButtonMenu> buttonMenus;
        [SerializeField] private List<GameObject> menuVisuals;

        private ButtonMenu currentOpenMenu;

        private void Start()
        {
            CavrnusStatics.Scheduler.ExecCoRoutine(DelayStart());
        }

        private IEnumerator DelayStart()
        {
            yield return null;
            menuVisuals.ForEach(mv => mv.SetActive(false));
            
            for (var i = 0; i < buttonMenus.Count; i++) {
                var bm = buttonMenus[i];
                bm.Setup(i);
                bm.OnToggleClicked += OnToggleClicked;
            }
        }

        private void OnToggleClicked(ButtonMenu bm)
        {
            if (currentOpenMenu == null) { // opening new menu
                menuVisuals.ForEach(mv => mv.SetActive(true));
                bm.SetState(true);
                currentOpenMenu = bm;
            }
            else if (currentOpenMenu.Id == bm.Id) { // toggling menu off
                bm.SetState(false);
                menuVisuals.ForEach(mv => mv.SetActive(false));
                currentOpenMenu = null;
            }
            else if (currentOpenMenu.Id != bm.Id) { // opening new menu, container already visible
                currentOpenMenu.SetState(false);
                bm.SetState(true);
                currentOpenMenu = bm;
            }
        }

        private void OnDestroy()
        {
            for (var i = 0; i < buttonMenus.Count; i++) {
                var bm = buttonMenus[i];
                bm.Teardown();
                bm.OnToggleClicked -= OnToggleClicked;
            }
        }
    }
}