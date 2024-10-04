using CavrnusDemo.CavrnusDataObjects;
using CavrnusSdk.API;
using UnityEngine;

namespace CavrnusSdk.CollaborationExamples
{
    public class CavrnusSyncDirectionalLightProperties : MonoBehaviour
    {
        [Header("Color Sync Properties")]
        [SerializeField] private FloatCavrnusPropertyObject sunRotationProperty;
        [SerializeField] private FloatCavrnusPropertyObject shadowStrengthProperty;
        
        [Header("Components")]
        [SerializeField] private GameObject lightContainer;
        [SerializeField] private Light targetLight;
        
        private static readonly int Rotation = Shader.PropertyToID("_Rotation");
        
        private void Start()
        {
            CavrnusFunctionLibrary.AwaitAnySpaceConnection(spaceConn => {

                spaceConn.BindFloatPropertyValue(sunRotationProperty.ContainerName, sunRotationProperty.PropertyName, val => {
                    RenderSettings.skybox.SetFloat(Rotation, val);
                    lightContainer.transform.localRotation = Quaternion.Euler(new Vector3(0, -val, 0));     
                });
         
                spaceConn.BindFloatPropertyValue(shadowStrengthProperty.ContainerName, shadowStrengthProperty.PropertyName, val => {
                    targetLight.shadowStrength = val; 
                });
            });
        }
    }
}