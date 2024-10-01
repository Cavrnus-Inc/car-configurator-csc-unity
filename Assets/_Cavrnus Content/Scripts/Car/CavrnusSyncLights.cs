using System;
using System.Collections.Generic;
using CavrnusSdk.API;
using UnityEngine;

namespace CavrnusDemo
{
    public class CavrnusSyncLights : MonoBehaviour
    {
        [SerializeField] private string containerName;
        [SerializeField] private string propertyName;

        [SerializeField] private Material emissiveMaterial;
        [SerializeField] private string emissionColorMaterialProperty = "_EmissionColor";
        [SerializeField] private List<GameObject> lights;
        
        private CavrnusSpaceConnection spaceConnection;
        private IDisposable binding;
        
        private void Start()
        {
            CavrnusFunctionLibrary.AwaitAnySpaceConnection(sc => {
                spaceConnection = sc;
                
                sc.BindBoolPropertyValue(containerName, propertyName, b => {
                    lights?.ForEach(l => l.gameObject.SetActive(b));
                    emissiveMaterial.SetColor(emissionColorMaterialProperty, Color.white * Mathf.Pow(2.0F, b ? 8 : 0));
                });
            });
        }
        
        public void Toggle()
        {
            var current = spaceConnection.GetBoolPropertyValue(containerName, propertyName);
            spaceConnection.PostBoolPropertyUpdate(containerName, propertyName, !current);
        }

        private void OnDestroy()
        {
            binding?.Dispose();
        }
    }
}