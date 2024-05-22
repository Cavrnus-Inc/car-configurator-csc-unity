using System;
using System.Collections.Generic;
using UnityEngine;
using CavrnusSdk.API;
using UnityBase;
using UnityEngine.Events;

namespace CavrnusDemo
{
    public class ColorTextureChanger : MonoBehaviour
    {
        [Serializable]
        public class ColorTextureMapper
        {
            public Texture Texture;
            public Color Color;
        }

        [SerializeField] private bool useTexture;
        
        [SerializeField] private UnityEvent<ColorTextureMapper> textureChanged;
        
        [SerializeField] private List<ColorTextureMapper> colorTextureMapper;
        [SerializeField] private GameObject colorPrefab;
        [SerializeField] private Transform container;

        [SerializeField] private string containerName;
        [SerializeField] private string propertyName;
        
        private CavrnusSpaceConnection spaceConnection;

        private List<ColorTextureChangerItem> colorItems = new List<ColorTextureChangerItem>();
        
        private void Start()
        {
            CavrnusFunctionLibrary.AwaitAnySpaceConnection(sc => {
                spaceConnection = sc;
                foreach (var mapper in colorTextureMapper) {
                    var go = Instantiate(colorPrefab, container);
                    colorItems.Add(go.GetComponent<ColorTextureChangerItem>());
                    go.GetComponent<ColorTextureChangerItem>().Setup(mapper, c => {
                        textureChanged?.Invoke(mapper);
                    });
                }

                if (useTexture) {
                    sc.BindStringPropertyValue(containerName, propertyName, serverTexture => {
                        foreach (var item in colorItems) {
                            item.SetSelectionState(item.Texture.name.ToLower().Equals(serverTexture.ToLower()));
                        }
                    });
                }
                else {
                    sc.BindColorPropertyValue(containerName, propertyName, serverColor => {
                        foreach (var item in colorItems) {
                            item.SetSelectionState(ColorsEqual(item.Color, serverColor));
                        }
                    });
                }
            });
        }
        
        public void SetColor(ColorTextureMapper cm)
        {
            spaceConnection?.PostColorPropertyUpdate(containerName, propertyName, cm.Color);
        }
        
        public void SetColorTexture(ColorTextureMapper cm)
        {
            spaceConnection?.PostStringPropertyUpdate(containerName, propertyName, cm.Texture.name);
        }
        
        private bool ColorsEqual(Color c1, Color c2, float tolerance = 0.1f)
        {
            return c1.r.AlmostEquals(c2.r, tolerance) &&
                   c1.g.AlmostEquals(c2.g, tolerance) &&
                   c1.b.AlmostEquals(c2.b, tolerance);
        }

    }
}