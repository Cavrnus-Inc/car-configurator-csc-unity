using System;
using System.Collections.Generic;
using CavrnusSdk.API;
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
        [SerializeField] private Light light;
        
        [Header("UI")]
        [SerializeField] private UISliderWrapper rotationSlider;
        [SerializeField] private UISliderWrapper shadowSlider;

        private CavrnusSpaceConnection spaceConn;
        private List<IDisposable> disposables = new List<IDisposable>();
        private static readonly int Rotation = Shader.PropertyToID("_Rotation");
        
        private CavrnusLivePropertyUpdate<float> liveSunRotationUpdate;
        private CavrnusLivePropertyUpdate<float> liveShadowUpdate;

        private void Start()
        {
            CavrnusFunctionLibrary.AwaitAnySpaceConnection(spaceConn => {
                this.spaceConn = spaceConn;

                rotationSlider.Slider.minValue = 0f;
                rotationSlider.Slider.maxValue = 360f;
                
                shadowSlider.Slider.minValue = 0f;
                shadowSlider.Slider.maxValue = 1f;
                
                // Sunlight Rotation
                spaceConn.DefineFloatPropertyDefaultValue(containerName, rotationPropertyName, RenderSettings.skybox.GetFloat(Rotation));
                disposables.Add(spaceConn.BindFloatPropertyValue(containerName, rotationPropertyName, rotation => {
                    RenderSettings.skybox.SetFloat(Rotation, rotation);
                    lightContainer.transform.localRotation = Quaternion.Euler(new Vector3(0, -rotation, 0));
                    rotationSlider.Slider.SetValueWithoutNotify(rotation);
                }));
                rotationSlider.OnValueUpdated += RotationValueChanged;
                rotationSlider.OnBeginDragging += RotationDragBegin;
                rotationSlider.OnEndDragging += RotationDragEnd;
                
                // Shadow Strength
                spaceConn.DefineFloatPropertyDefaultValue(containerName, shadowPropertyName, light.shadowStrength);
                disposables.Add(spaceConn.BindFloatPropertyValue(containerName, shadowPropertyName, strength => {
                    light.shadowStrength = strength;
                    shadowSlider.Slider.SetValueWithoutNotify(strength);
                }));
                shadowSlider.OnValueUpdated += ShadowValueChanged;
                shadowSlider.OnBeginDragging += ShadowDragBegin;
                shadowSlider.OnEndDragging += ShadowDragEnd;
            });
        }

        private void ShadowValueChanged(float val)
        {
            liveShadowUpdate?.UpdateWithNewData(val);
        }

        private void ShadowDragBegin(float val)
        {
            liveShadowUpdate ??= spaceConn.BeginTransientFloatPropertyUpdate(containerName, shadowPropertyName, val);
        }

        private void ShadowDragEnd(float val)
        {
            liveShadowUpdate?.Finish();
            liveShadowUpdate = null;
        }

        private void RotationDragBegin(float val)
        {
            liveSunRotationUpdate ??= spaceConn.BeginTransientFloatPropertyUpdate(containerName, rotationPropertyName, val);
        }

        private void RotationDragEnd(float val)
        {
            liveSunRotationUpdate?.Finish();
            liveSunRotationUpdate = null;
        }

        private void RotationValueChanged(float val)
        {
            liveSunRotationUpdate?.UpdateWithNewData(val);
        }

        private void OnDestroy()
        {
            foreach (var disposable in disposables)
                disposable.Dispose();
            
            rotationSlider.OnValueUpdated -= RotationValueChanged;
            rotationSlider.OnBeginDragging -= RotationDragBegin;
            rotationSlider.OnEndDragging -= RotationDragEnd;
            
            shadowSlider.OnValueUpdated -= ShadowValueChanged;
            shadowSlider.OnBeginDragging -= ShadowDragBegin;
            shadowSlider.OnEndDragging -= ShadowDragEnd;
        }
    }
}