using CavrnusDemo.CavrnusDataObjects;
using CavrnusDemo.CavrnusDemo;
using UnityEngine;

namespace CavrnusDemo
{
    public class CavrnusTextureUIChanger : CavrnusColorCollectionUIBase
    {
        [SerializeField] private StringCavrnusPropertyObject propertyObject;
        
        protected override void BindProperty()
        {
            propertyObject.OnServerValueUpdated += OnServerValueUpdated;
        }

        private void OnServerValueUpdated(string serverTexture)
        {
            if (string.IsNullOrWhiteSpace(serverTexture)) 
                return;
                    
            var serverData = ColorCollection.GetDataFromTextureName(serverTexture);
            SetSelectedItem(serverData);
        }

        protected override void OnSelected(CavrnusColorCollection.ColorTextureInfo data)
        {
            propertyObject.PostValue(data.Texture.name);
        }

        private void OnDestroy()
        {
            propertyObject.Unbind();
            propertyObject.OnServerValueUpdated -= OnServerValueUpdated;
        }
    }
}