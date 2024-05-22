using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Cavrnus.UI.MenuManagement
{
    public class UIStateVisuals : MonoBehaviour
    {
        [Serializable]
        private class GraphicsGroup
        {
            public string Category;
            public List<Graphic> Targets;
            public Color UnselectedColor;
            public Color SelectedColor;
        }
        
        [SerializeField] private List<GraphicsGroup> targets;

        public void SetOn()
        {
            foreach (var t in targets) {
                foreach (var target in t.Targets) {
                    target.color = t.SelectedColor;
                }
            }
       
        }

        public void SetOff()
        {
            foreach (var t in targets) {
                foreach (var target in t.Targets) {
                    target.color = t.UnselectedColor;
                }
            }
        }

        public void SetValue(bool val)
        {
            if (val) {
                SetOn();
            }
            else {
                SetOff();
            }
        }
    }
}