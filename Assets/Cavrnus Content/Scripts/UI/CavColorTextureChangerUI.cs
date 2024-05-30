using System;
using System.Collections.Generic;
using UnityEngine;

namespace CavrnusDemo
{
    public class CavColorTextureChangerUI : CavColorChangerUI
    {
        [SerializeField] private GameObject colorPrefab;
        [SerializeField] private Transform container;

        private readonly List<ColorTextureChangerItem> items = new List<ColorTextureChangerItem>();

        public override void Setup(List<ColorTextureInfo> info, Action<ColorTextureInfo> onSelect)
        {
            foreach (var mapper in info) {
                var go = Instantiate(colorPrefab, container);
                items.Add(go.GetComponent<ColorTextureChangerItem>());
                go.GetComponent<ColorTextureChangerItem>().Setup(mapper, c => {
                    onSelect?.Invoke(c);
                });
            }
        }

        public override void SetSelected(ColorTextureInfo selectedData)
        {
            if (selectedData != null) {
                foreach (var item in items)
                    item.SetSelectionState(item.Info.DisplayName.ToLowerInvariant().Trim().Equals(selectedData.DisplayName.ToLowerInvariant().Trim()));
            }
        }
    }
}