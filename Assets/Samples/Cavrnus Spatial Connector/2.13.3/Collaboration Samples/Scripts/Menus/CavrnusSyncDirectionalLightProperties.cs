using CavrnusSdk.API;
using UnityEngine;

namespace CavrnusSdk.CollaborationExamples
{
    public class CavrnusSyncDirectionalLightProperties : MonoBehaviour
    {
        [SerializeField] private string containerName;

        [Header("Color Sync Properties")]
        [SerializeField] private string rotationPropertyName;
        [SerializeField] private string shadowPropertyName;
        
        [Header("Components")]
        [SerializeField] private GameObject lightContainer;
        [SerializeField] private Light targetLight;
        
        private static readonly int Rotation = Shader.PropertyToID("_Rotation");
        
        private void Start()
        {
            CavrnusFunctionLibrary.AwaitAnySpaceConnection(spaceConn => {

                spaceConn.BindFloatPropertyValue(containerName, rotationPropertyName, val => {
                    RenderSettings.skybox.SetFloat(Rotation, val);
                    lightContainer.transform.localRotation = Quaternion.Euler(new Vector3(0, -val, 0));     
                });
         
                spaceConn.BindFloatPropertyValue(containerName, shadowPropertyName, val => {
                    targetLight.shadowStrength = val; 
                });
            });
        }
    }
}