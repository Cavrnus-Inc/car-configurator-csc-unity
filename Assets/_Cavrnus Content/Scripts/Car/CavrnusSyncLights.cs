using System;
using System.Collections.Generic;
using CavrnusDemo.CavrnusDataObjects;
using CavrnusSdk.API;
using UnityEngine;

namespace CavrnusDemo
{
    public class CavrnusSyncLights : MonoBehaviour
    {
        [SerializeField] private BoolCavrnusPropertyObject boolProperty;
        
        [SerializeField] private Material emissiveMaterial;
        [SerializeField] private string emissionColorMaterialProperty = "_EmissionColor";
        [SerializeField] private List<GameObject> lights;
        
        private IDisposable binding;
        
        private void Start()
        {
            CavrnusFunctionLibrary.AwaitAnySpaceConnection(sc => {
                sc.BindBoolPropertyValue(boolProperty.ContainerName, boolProperty.PropertyName, b => {
                    lights?.ForEach(l => l.gameObject.SetActive(b));
                    emissiveMaterial.SetColor(emissionColorMaterialProperty, Color.white * Mathf.Pow(2.0F, b ? 8 : 0));
                });
            });
        }
        
        private void OnDestroy()
        {
            binding?.Dispose();
        }
    }
}