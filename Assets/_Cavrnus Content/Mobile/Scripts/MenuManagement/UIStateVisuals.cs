using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Cavrnus.UI.MenuManagement
{
    public class UIStateVisuals : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [Serializable]
        private class GraphicsGroup
        {
            public string Category;
            public List<Graphic> Targets;
            public Color UnselectedColor;
            public Color SelectedColor;
        }

        [Serializable]
        private class IconState
        {
            public Image Image;
            public Sprite OnIcon;
            public Sprite OffIcon;
        }

        [SerializeField] private Toggle toggle;
        
        [SerializeField] private List<GraphicsGroup> stateTargets;
        
        [Space]
        [SerializeField] private List<GraphicsGroup> hoverTargets;

        [Space]
        [SerializeField] private IconState iconState;

        private void Awake()
        {
            SetValue(false);
            OnPointerExit(null);
        }

        private void Update()
        {
            if (toggle != null) {
                SetValue(toggle.isOn);
            }
        }

        public void SetOn()
        {
            foreach (var t in stateTargets) {
                foreach (var target in t.Targets) {
                    target.color = t.SelectedColor;
                }
            }
            
            if (iconState != null && iconState.Image != null && iconState.OnIcon != null)
                iconState.Image.sprite = iconState.OnIcon;
        }

        public void SetOff()
        {
            foreach (var t in stateTargets) {
                foreach (var target in t.Targets) {
                    target.color = t.UnselectedColor;
                }
            }

            if (iconState != null && iconState.Image != null && iconState.OffIcon != null)
                iconState.Image.sprite = iconState.OffIcon;
        }

        public void SetValue(bool val)
        {
            if (val)
                SetOn();
            else
                SetOff();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            foreach (var t in hoverTargets) {
                foreach (var target in t.Targets) {
                    target.color = t.SelectedColor;
                }
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            foreach (var t in hoverTargets) {
                foreach (var target in t.Targets) {
                    target.color = t.UnselectedColor;
                }
            }
        }
        
        public void OnPointerClick(PointerEventData eventData) { }
    }
}