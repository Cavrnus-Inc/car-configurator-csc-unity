using System;
using System.Collections.Generic;
using UnityEngine;
using CavrnusSdk.API;
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
        
        [SerializeField] private UnityEvent<ColorTextureMapper> textureChanged;
        
        [SerializeField] private List<ColorTextureMapper> colorTextureMapper;
        [SerializeField] private GameObject colorPrefab;
        [SerializeField] private Transform container;
        
        private void Start()
        {
            CavrnusFunctionLibrary.AwaitAnySpaceConnection(sc => {
                foreach (var mapper in colorTextureMapper) {
                    var go = Instantiate(colorPrefab, container);
                    go.GetComponent<ColorTextureChangerItem>().Setup(mapper, c => {
                        textureChanged?.Invoke(mapper);
                    });
                }
            });
        }
    }
}