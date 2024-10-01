using System;
using System.Collections.Generic;
using System.Linq;
using CavrnusSdk.API;
using Collab.Proxy.Prop.StringProp;
using UnityEngine;

namespace CavrnusDemo
{
    public class SyncEnumTextureStrings : MonoBehaviour
    {
        [Header("Cav Properties")]
        [SerializeField] private string containerName;
        [SerializeField] private string propertyName;
        [SerializeField] private string displayName;
        [SerializeField] private string description;

        [Header("Texture & Material")]
        [SerializeField] private CavrnusColorCollection colorData;
        [SerializeField] private Material targetMaterial;

        private IDisposable disp;

        private void Start()
        {
            CavrnusFunctionLibrary.AwaitAnySpaceConnection(spaceConn => {
                var enumOptions = new List<StringEditingEnumerationOption>();
                colorData.ColorData.ForEach(tm => enumOptions.Add(new StringEditingEnumerationOption {
                    DisplayText = tm.DisplayName,
                    EnumValue = tm.DisplayName
                }));

                spaceConn.DefineStringPropertyDefinition(containerName, propertyName, displayName, description, false, enumOptions);

                disp = spaceConn.BindStringPropertyValue(containerName, propertyName, serverTextureName => {
                    var newTexture = colorData.ColorData.FirstOrDefault(tm => tm.DisplayName == serverTextureName);
                    if (newTexture != null) 
                        targetMaterial.mainTexture = newTexture.Texture;
                    else
                        Debug.LogWarning($"TextureValue is invalid! Trying to update with server value of {serverTextureName}");
                });
            });
        }

        private void OnDestroy()
        {
            disp?.Dispose();
        }
    }
}