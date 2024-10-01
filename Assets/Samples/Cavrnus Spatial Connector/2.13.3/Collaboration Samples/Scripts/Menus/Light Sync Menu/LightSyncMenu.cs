using System;
using CavrnusSdk.API;
using CavrnusSdk.PropertyUISynchronizers;
using UnityEngine;

namespace CavrnusSdk.CollaborationExamples
{
    public class LightSyncMenu : MonoBehaviour
    {
        [SerializeField] private string containerName = "LightColorMenu";

        [Header("Color Sync Properties")]
        [SerializeField] private string colorPropertyName = "Color";
        
        [Header("Color Sync Properties")]
        [SerializeField] private string intensityPropertyName = "Intensity";
        
        [Header("Light Component")]
        [SerializeField] private Light lightComponent;
        
        [Header("UI")]
        [SerializeField] private CavrnusPropertyUISlider intensityCavrnusPropertyUISlider;
        [SerializeField] private Vector2 intensityMinMax = new Vector2(10f, 150f);

        private IDisposable disposable;
        
        private void Start()
        {
            CavrnusFunctionLibrary.AwaitAnySpaceConnection(spaceConn => {
                intensityCavrnusPropertyUISlider.Setup(containerName, intensityPropertyName, intensityMinMax, val => {
                    lightComponent.intensity = val;
                });
                
                spaceConn.DefineColorPropertyDefaultValue(containerName, colorPropertyName, lightComponent.color);
                disposable = spaceConn.BindColorPropertyValue(containerName, colorPropertyName, serverColor => {
                    lightComponent.color = serverColor;
                });
            });
        }

        private void OnDestroy() => disposable?.Dispose();
    }
}