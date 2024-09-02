using System;
using CavrnusSdk.API;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace Cavrnus_Content.Scripts.VisionProComponents
{
    public class CavrnusVisionProPropertySlider : MonoBehaviour
    {
        [SerializeField] private string containerName;
        [SerializeField] private string propertyName;
        
        [Space]
        [SerializeField] private CavrnusVisionProSliderComponent sliderComponentComponent;

        private IDisposable binding;
        private CavrnusSpaceConnection spaceConn;
        private CavrnusLivePropertyUpdate<float> livePropertyUpdate;

        private void Start()
        {
            CavrnusFunctionLibrary.AwaitAnySpaceConnection(sc => {
                spaceConn = sc;
                sc.DefineFloatPropertyDefaultValue(containerName,propertyName,0.5f);
                binding = sc.BindFloatPropertyValue(containerName, propertyName, val => {
                    sliderComponentComponent.SetFillPercentage(val, false);
                });
                
                sliderComponentComponent.m_OnSliderValueChanged.AddListener(SliderUpdated);
                sliderComponentComponent.selectEntered.AddListener(BeginInteract);
                sliderComponentComponent.selectExited.AddListener(FinishInteract);
            });
        }

        private void FinishInteract(SelectExitEventArgs arg0)
        {
            livePropertyUpdate?.Finish();
            livePropertyUpdate = null;
        }

        private void BeginInteract(SelectEnterEventArgs arg0)
        {
            livePropertyUpdate ??= spaceConn.BeginTransientFloatPropertyUpdate(containerName, propertyName, sliderComponentComponent.SliderValue);
        }

        private void SliderUpdated(float val)
        {
            livePropertyUpdate?.UpdateWithNewData(val);
        }

        private void OnDestroy()
        {
            binding?.Dispose();
                
            sliderComponentComponent.m_OnSliderValueChanged.RemoveListener(SliderUpdated);
            sliderComponentComponent.selectEntered.RemoveListener(BeginInteract);
            sliderComponentComponent.selectExited.RemoveListener(FinishInteract);
        }
    }
}