using System;
using System.Collections.Generic;
using System.Linq;
using CavrnusDemo;
using CavrnusSdk.API;
using Collab.Proxy.Prop.StringProp;
using UnityEngine;

namespace Cavrnus_Content.Scripts.Car
{
    public class SyncEnumTextureStrings : MonoBehaviour
    {
        [Serializable]
        public class TextureMapper
        {
            public string TextureName;
            public Texture TextureValue;
        }
        
        [Header("Cav Properties")]
        [SerializeField] private string containerName;
        [SerializeField] private string propertyName;
        [SerializeField] private string displayName;
        [SerializeField] private string description;

        [Header("Texture & Material")]
        [SerializeField] private List<TextureMapper> textureMap;
        [SerializeField] private Material targetMaterial;
        
        private IDisposable disp;

        private CavrnusSpaceConnection spaceConn;

        private void Start()
        {
            CavrnusFunctionLibrary.AwaitAnySpaceConnection(spaceConn => {
                this.spaceConn = spaceConn;
                
                var enumOptions = new List<StringEditingEnumerationOption>();
                textureMap.ForEach(tm => enumOptions.Add(new StringEditingEnumerationOption {
                    DisplayText = tm.TextureName,
                    EnumValue = tm.TextureName
                }));

                spaceConn.DefineStringPropertyDefinition(containerName, propertyName, displayName, description, false, enumOptions);

                disp = spaceConn.BindStringPropertyValue(containerName, propertyName, serverTextureName => {
                    var newTexture = textureMap.FirstOrDefault(tm => tm.TextureName == serverTextureName);
                    if (newTexture != null) 
                        targetMaterial.mainTexture = newTexture.TextureValue;
                    else {
                        Debug.LogWarning($"TextureValue is invalid! Trying to update with server value of {serverTextureName}");
                    }
                });
            });
        }

        public void SetColor(ColorTextureChanger.ColorTextureMapper cm)
        {
            spaceConn?.PostStringPropertyUpdate(containerName, propertyName, cm.Texture.name);
        }
        
        private void OnDestroy()
        {
            disp?.Dispose();
        }
    }
}