﻿using System;
using System.Collections.Generic;
using CavrnusDemo.CavrnusDataObjects;
using CavrnusSdk.API;
using UnityEngine;

namespace CavrnusDemo
{
    public class CavColorTextureChangerUI : MonoBehaviour
    {
        [SerializeField] private bool userColorValue;

        [SerializeField] private ColorCavrnusPropertyObject colorPropertyObject;
        [SerializeField] private StringCavrnusPropertyObject textureColorPropertyObject;
        
        [SerializeField] private GameObject colorPrefab;
        [SerializeField] private Transform container;

        [SerializeField] private CavrnusColorCollection colorData;

        private readonly List<ColorTextureChangerItem> items = new List<ColorTextureChangerItem>();

        private List<IDisposable> bindings = new List<IDisposable>();
        private CavrnusSpaceConnection spaceConnection;

        private void Start()
        {
            
            
            CavrnusFunctionLibrary.AwaitAnySpaceConnection(sc => {
                spaceConnection = sc;
                foreach (var data in colorData.ColorData) {
                    var go = Instantiate(colorPrefab, container);
                    items.Add(go.GetComponent<ColorTextureChangerItem>());
                    go.GetComponent<ColorTextureChangerItem>().Setup(data, OnSelected);
                }

                if (userColorValue) {
                    bindings.Add(sc.BindColorPropertyValue(colorPropertyObject.ContainerName, colorPropertyObject.PropertyName, serverColor => {
                        var serverData = colorData.GetDataFromColor(serverColor);
                        SetSelectedColor(serverData);
                    }));
                }
                else {
                    bindings.Add(sc.BindStringPropertyValue(textureColorPropertyObject.ContainerName, textureColorPropertyObject.PropertyName, serverTexture => {
                        if (string.IsNullOrWhiteSpace(serverTexture)) 
                            return;
                    
                        var serverData = colorData.GetDataFromTextureName(serverTexture);
                        SetSelectedColor(serverData);
                    }));
                }
            });
        }

        private void OnSelected(CavrnusColorCollection.ColorTextureInfo data)
        {
            if (data.Texture == null)
                spaceConnection?.PostColorPropertyUpdate(colorPropertyObject.ContainerName, colorPropertyObject.PropertyName, data.Color);
            else
                spaceConnection?.PostStringPropertyUpdate(textureColorPropertyObject.ContainerName, textureColorPropertyObject.PropertyName, data.Texture.name);
        }

        private void SetSelectedColor(CavrnusColorCollection.ColorTextureInfo selectedData)
        {
            if (selectedData != null) {
                foreach (var item in items)
                    item.SetSelectionState(item.Info.DisplayName.ToLowerInvariant().Trim().Equals(selectedData.DisplayName.ToLowerInvariant().Trim()));
            }
        }

        private void OnDestroy()
        {
            bindings?.ForEach(b => b?.Dispose());
        }
    }
}