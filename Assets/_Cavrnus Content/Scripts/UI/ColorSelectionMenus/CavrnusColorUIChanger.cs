using CavrnusDemo.CavrnusDataObjects;
using CavrnusDemo.CavrnusDemo;
using UnityEngine;

namespace CavrnusDemo
{
    public class CavrnusColorUIChanger : CavrnusColorCollectionUIBase
    {
        [SerializeField] private ColorCavrnusPropertyObject propertyObject;
        
        protected override void BindProperty()
        {
            propertyObject.OnServerValueUpdated += OnServerValueUpdated;
        }

        private void OnServerValueUpdated(Color serverColor)
        {
            var serverData = ColorCollection.GetDataFromColor(serverColor);
            SetSelectedItem(serverData);
        }

        protected override void OnSelected(CavrnusColorCollection.ColorTextureInfo data)
        {
            propertyObject.PostValue(data.Color);
        }
        
        private void OnDestroy()
        {
            propertyObject.Unbind();
            propertyObject.OnServerValueUpdated -= OnServerValueUpdated;
        }
    }
}