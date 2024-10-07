using System;
using System.Collections.Generic;
using System.Linq;
using CavrnusDemo.CavrnusDataObjects;
using CavrnusSdk.API;
using Collab.Proxy.Prop.StringProp;
using UnityEngine;

namespace CavrnusDemo
{
    public class SyncEnumTextureStrings : MonoBehaviour
    {
        [SerializeField] private StringCavrnusPropertyObject propertyInfo;
        
        [Header("Texture & Material")]
        [SerializeField] private CavrnusColorCollection colorData;
        [SerializeField] private Material targetMaterial;

        private void Start()
        {
            CavrnusFunctionLibrary.AwaitAnySpaceConnection(spaceConn => {
                var enumOptions = new List<StringEditingEnumerationOption>();
                colorData.ColorData.ForEach(tm => enumOptions.Add(new StringEditingEnumerationOption {
                    DisplayText = tm.DisplayName,
                    EnumValue = tm.DisplayName
                }));

                spaceConn.DefineStringPropertyDefinition(propertyInfo.ContainerName, propertyInfo.PropertyName, propertyInfo.DisplayName, propertyInfo.Description, false, enumOptions);

                propertyInfo.OnServerValueUpdated += OnServerValueUpdated;
            });
        }

        private void OnServerValueUpdated(string serverTextureName)
        {
            var newTexture = colorData.ColorData.FirstOrDefault(tm => tm.DisplayName == serverTextureName);
            if (newTexture != null) 
                targetMaterial.mainTexture = newTexture.Texture;
            else
                Debug.LogWarning($"TextureValue is invalid! Trying to update with server value of {serverTextureName}");
        }

        private void OnDestroy()
        {
            propertyInfo.Unbind();
            propertyInfo.OnServerValueUpdated -= OnServerValueUpdated;
        }
    }
}