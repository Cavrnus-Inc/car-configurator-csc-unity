using System.Collections.Generic;
using UnityEngine;

namespace CavrnusDemo
{
    namespace CavrnusDemo
    {
        public abstract class CavrnusColorCollectionUIBase : MonoBehaviour
        {
            [SerializeField] private GameObject prefab;
            [SerializeField] private Transform container;
            [SerializeField] protected CavrnusColorCollection ColorCollection;

            private readonly List<ColorTextureChangerItem> items = new List<ColorTextureChangerItem>();

            protected virtual void Awake()
            {
                PopulateItems();
                BindProperty();
            }
            
            private void PopulateItems()
            {
                foreach (var data in ColorCollection.ColorData) {
                    var go = Instantiate(prefab, container);
                    items.Add(go.GetComponent<ColorTextureChangerItem>());
                    go.GetComponent<ColorTextureChangerItem>().Setup(data, OnSelected);
                }
            }

            protected abstract void BindProperty();
            protected abstract void OnSelected(CavrnusColorCollection.ColorTextureInfo data);

            protected void SetSelectedItem(CavrnusColorCollection.ColorTextureInfo selectedData)
            {
                if (selectedData != null) {
                    foreach (var item in items)
                        item.SetSelectionState(item.Info.DisplayName.ToLowerInvariant().Trim().Equals(selectedData.DisplayName.ToLowerInvariant().Trim()));
                }
            }
        }
    }
}