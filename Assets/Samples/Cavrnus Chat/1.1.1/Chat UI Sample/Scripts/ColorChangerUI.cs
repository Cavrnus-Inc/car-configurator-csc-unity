using System;
using System.Collections.Generic;
using System.Linq;
using CavrnusSdk.API;
using Collab.Proxy.Prop.StringProp;
using UnityEngine;

namespace Cavrnus.Chat
{
    public class ColorChangerUI : MonoBehaviour
    {
        [Serializable]
        public class ColorMapper
        {
            public string ColorName;
            public Color ColorValue;
        }

        [Header("Cav Properties")]
        [SerializeField] private string containerName;
        [SerializeField] private string propertyName;
        [SerializeField] private string displayName;
        [SerializeField] private string description;

        [Header("Color & Material")]
        [SerializeField] private List<ColorMapper> colorData;
        [SerializeField] private Material targetMaterial;

        private IDisposable disp;

        private void Start()
        {
            CavrnusFunctionLibrary.AwaitAnySpaceConnection(spaceConn => {
                var enumOptions = new List<StringEditingEnumerationOption>();
                colorData.ForEach(cd => enumOptions.Add(new StringEditingEnumerationOption {
                    DisplayText = cd.ColorName,
                    EnumValue = cd.ColorName
                }));

                spaceConn.DefineStringPropertyDefinition(containerName, propertyName, displayName, description, false, enumOptions);

                disp = spaceConn.BindStringPropertyValue(containerName, propertyName, serverColorName => {
                    var newColor = colorData.FirstOrDefault(cm => cm.ColorName == serverColorName);
                    if (newColor != null) 
                        targetMaterial.color = newColor.ColorValue;
                    else {
                        Debug.LogWarning($"Color is invalid! Trying to update with server value of {serverColorName}");
                    }
                });
            });
        }

        private void OnDestroy()
        {
            disp?.Dispose();
        }
    }
}