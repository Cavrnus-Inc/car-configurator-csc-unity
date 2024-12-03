using CavrnusSdk.API;
using CavrnusSdk.PropertyUISynchronizers;
using UnityEngine;

namespace CavrnusSdk.CollaborationExamples
{
    public class DirectionalLightMenu : MonoBehaviour
    {
        [SerializeField] private string containerName = "DirectionalLight";

        [Header("Color Sync Properties")]
        [SerializeField] private string rotationPropertyName = "SunlightRotation";
        [SerializeField] private string shadowPropertyName = "ShadowStrength";
        
        [Header("Components")]
        [SerializeField] private GameObject lightContainer;
        [SerializeField] private Light targetLight;
        
        [Header("UI")]
        [SerializeField] private CavrnusPropertyUISlider rotationPropertyUISlider;
        [SerializeField] private CavrnusPropertyUISlider shadowPropertyUISlider;

        private static readonly int Rotation = Shader.PropertyToID("_Rotation");
        
        private void Start()
        {
            CavrnusFunctionLibrary.AwaitAnySpaceConnection(spaceConn => {
                rotationPropertyUISlider.Setup(containerName, rotationPropertyName, new Vector2(0f, 360f), val => {
                    RenderSettings.skybox.SetFloat(Rotation, val);
                    lightContainer.transform.localRotation = Quaternion.Euler(new Vector3(0, -val, 0));                             
                });
                
                shadowPropertyUISlider.Setup(containerName, shadowPropertyName, new Vector2(0f, 1f), val => {
                    targetLight.shadowStrength = val;
                });
            });
        }
    }
}