using CavrnusDemo.CavrnusDataObjects;
using UnityEngine;

namespace CavrnusDemo
{
    public class SyncSunlightProperties : MonoBehaviour
    {
        [SerializeField] private FloatCavrnusPropertyObject sunlightRotationProperty;
        [SerializeField] private FloatCavrnusPropertyObject shadowStrengthProperty;
        
        [Header("Components")]
        [SerializeField] private GameObject lightContainer;
        [SerializeField] private Light directionalLight;
        
        private static readonly int Rotation = Shader.PropertyToID("_Rotation");

        private void Start()
        {
            shadowStrengthProperty.OnServerValueUpdated += OnShadowStrengthUpdated;
            sunlightRotationProperty.OnServerValueUpdated += OnSunlightRotationUpdated;
        }

        private void OnShadowStrengthUpdated(float val)
        {
            directionalLight.shadowStrength = val;
        }

        private void OnSunlightRotationUpdated(float val)
        {
            RenderSettings.skybox.SetFloat(Rotation, val);
            lightContainer.transform.localRotation = Quaternion.Euler(new Vector3(0, -val, 0));
        }

        private void OnDestroy()
        {
            shadowStrengthProperty.Unbind();
            sunlightRotationProperty.Unbind();
            
            shadowStrengthProperty.OnServerValueUpdated -= OnShadowStrengthUpdated;
            sunlightRotationProperty.OnServerValueUpdated -= OnSunlightRotationUpdated;
        }
    }
}