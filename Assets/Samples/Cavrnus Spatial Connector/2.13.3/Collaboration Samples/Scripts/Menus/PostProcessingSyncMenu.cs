using CavrnusSdk.API;
using CavrnusSdk.PropertyUISynchronizers;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace CavrnusSdk.CollaborationExamples
{
    public class PostProcessingSyncMenu : MonoBehaviour
    {
        [SerializeField] private Volume volume;

        [Header("Container")]
        [SerializeField] private string containerName = "PostProcessing";

        [Header("Saturation")]
        [SerializeField] private string saturationEnabledPropertyName = "SaturationEnabled";
        [SerializeField] private string saturationValuePropertyName = "Saturation";
        [SerializeField] private CavrnusPropertyUIToggle saturationToggle;
        [SerializeField] private CavrnusPropertyUISlider saturationUISlider;
        
        [Header("Saturation")]
        [SerializeField] private string bloomEnabledPropertyName = "BloomEnabled";
        [SerializeField] private string bloomValuePropertyName = "Bloom";
        [SerializeField] private CavrnusPropertyUIToggle bloomShiftToggle;
        [SerializeField] private CavrnusPropertyUISlider bloomShiftUISlider;

        private void Start()
        {
            CavrnusFunctionLibrary.AwaitAnySpaceConnection(spaceConn => {
                if (volume == null) {
                    volume = FindObjectOfType<Volume>();

                    if (volume == null) {
                        Debug.LogWarning($"Missing PostProcessing Volume in Scene!");
                        return;
                    }
                }
           
                if (volume.profile.TryGet(out ColorAdjustments ca))
                    SetupSaturationProperties(spaceConn, ca);
                
                if (volume.profile.TryGet(out Bloom b))
                    SetupBloomShiftProperties(spaceConn, b);
            });
        }
        
        #region Saturation Property
        
        private void SetupSaturationProperties(CavrnusSpaceConnection spaceConn, ColorAdjustments ca)
        {
            saturationUISlider.Setup(containerName, saturationValuePropertyName, new Vector2(-100, 100), val => {
                ca.saturation.value = val;
            });
            saturationToggle.Setup(spaceConn, containerName, saturationEnabledPropertyName, false, val => {
                ca.saturation.overrideState = val;
            });
        }

        #endregion

        #region Bloom Property

        private void SetupBloomShiftProperties(CavrnusSpaceConnection spaceConn, Bloom bloom)
        {
            bloomShiftUISlider.Setup(containerName, bloomValuePropertyName, new Vector2(-100, 100), val => {
                bloom.intensity.value = val;
            });
            bloomShiftToggle.Setup(spaceConn, containerName, bloomEnabledPropertyName, false, val => {
                bloom.active = val;
            });
        }
   
        #endregion
    }
}