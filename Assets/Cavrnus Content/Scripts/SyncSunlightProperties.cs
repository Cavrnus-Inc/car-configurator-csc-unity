using System;
using System.Collections.Generic;
using CavrnusSdk.API;
using UnityEngine;

namespace CavrnusDemo
{
    public class SyncSunlightProperties : MonoBehaviour
    {
        [SerializeField] private string containerName = "Car";

        [Header("Color Sync Properties")]
        [SerializeField] private string rotationPropertyName = "SunlightRotation";
        [SerializeField] private string shadowPropertyName = "ShadowStrength";
        
        [Header("Components")]
        [SerializeField] private GameObject lightContainer;
        [SerializeField] private Light directionalLight;
        
        private List<IDisposable> disposables = new List<IDisposable>();
        private static readonly int Rotation = Shader.PropertyToID("_Rotation");

        private void Start()
        {
            CavrnusFunctionLibrary.AwaitAnySpaceConnection(spaceConn => {
                
                // Sunlight Rotation
                spaceConn.DefineFloatPropertyDefaultValue(containerName, rotationPropertyName, RenderSettings.skybox.GetFloat(Rotation));
                disposables.Add(spaceConn.BindFloatPropertyValue(containerName, rotationPropertyName, rotation => {
                    RenderSettings.skybox.SetFloat(Rotation, rotation);
                    lightContainer.transform.localRotation = Quaternion.Euler(new Vector3(0, -rotation, 0));
                }));
  
                // Shadow Strength
                spaceConn.DefineFloatPropertyDefaultValue(containerName, shadowPropertyName, directionalLight.shadowStrength);
                disposables.Add(spaceConn.BindFloatPropertyValue(containerName, shadowPropertyName, strength => {
                    directionalLight.shadowStrength = strength;
                }));
            });
        }
        
        private void OnDestroy()
        {
            foreach (var disposable in disposables)
                disposable.Dispose();
        }
    }
}