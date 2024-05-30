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
        [Header("Optional Corresponding UI")]
        [SerializeField] private CavColorChangerUI cavColorChangerUI;
        
        [Header("Cav Properties")]
        [SerializeField] private string containerName;
        [SerializeField] private string propertyName;
        [SerializeField] private string displayName;
        [SerializeField] private string description;

        [Header("Texture & Material")]
        [SerializeField] private List<ColorTextureInfo> colorTextureInfo;
        [SerializeField] private Material targetMaterial;

        private CavrnusSpaceConnection spaceConn;
        private IDisposable disp;

        private void Start()
        {
            CavrnusFunctionLibrary.AwaitAnySpaceConnection(spaceConn => {
                this.spaceConn = spaceConn;
                
                cavColorChangerUI?.Setup(colorTextureInfo, Post);
                
                var enumOptions = new List<StringEditingEnumerationOption>();
                colorTextureInfo.ForEach(tm => enumOptions.Add(new StringEditingEnumerationOption {
                    DisplayText = tm.DisplayName,
                    EnumValue = tm.DisplayName
                }));

                spaceConn.DefineStringPropertyDefinition(containerName, propertyName, displayName, description, false, enumOptions);

                disp = spaceConn.BindStringPropertyValue(containerName, propertyName, serverTextureName => {
                    var foundItem = colorTextureInfo.FirstOrDefault(info => info.DisplayName.ToLowerInvariant().Equals(serverTextureName.ToLowerInvariant()));
                    if (foundItem != null)
                        cavColorChangerUI?.SetSelected(foundItem);
                    
                    var newTexture = colorTextureInfo.FirstOrDefault(tm => tm.DisplayName == serverTextureName);
                    if (newTexture != null) 
                        targetMaterial.mainTexture = newTexture.Texture;
                    else
                        Debug.LogWarning($"TextureValue is invalid! Trying to update with server value of {serverTextureName}");
                });
            });
        }

        private void Post(ColorTextureInfo item)
        {
            spaceConn?.PostStringPropertyUpdate(containerName, propertyName, item.DisplayName);
        }

        private void OnDestroy()
        {
            disp?.Dispose();
        }
    }
}