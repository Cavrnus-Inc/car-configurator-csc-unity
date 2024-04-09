using System;
using System.Collections.Generic;
using CavrnusSdk.API;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

namespace CavrnusSdk.CollaborationExamples
{
    public class PostProcessingSyncMenu : MonoBehaviour
    {
        [SerializeField] private Volume volume;

        [Header("Container")]
        [SerializeField] private string containerName = "PostProcessing";

        [Header("Saturation")]
        [SerializeField] private string saturationEnabledPropertyName = "SaturationVisibility";
        [SerializeField] private string saturationValuePropertyName = "SaturationValue";
        [SerializeField] private Toggle saturationToggle;
        [SerializeField] private UISliderWrapper saturationSlider;
        private CavrnusLivePropertyUpdate<float> liveContrastUpdate = null;
        
        [Header("Saturation")]
        [SerializeField] private string hueShiftEnabledPropertyName = "HueShiftVisibility";
        [SerializeField] private string hueShiftValuePropertyName = "HueShiftValue";
        [SerializeField] private Toggle hueShiftToggle;
        [SerializeField] private UISliderWrapper hueShiftSlider;
        private CavrnusLivePropertyUpdate<float> liveHueShiftUpdate = null;

        private CavrnusSpaceConnection spaceConn;
        private List<IDisposable> disposables = new List<IDisposable>();

        private void Start()
        {
            CavrnusFunctionLibrary.AwaitAnySpaceConnection(spaceConn => {
                this.spaceConn = spaceConn;

                if (volume.profile.TryGet(out ColorAdjustments ca)) {
                    SetupSaturationProperties(spaceConn, ca);
                    SetupHueShiftProperties(spaceConn, ca);
                }
            });
        }

        #region Saturation Property
        
        private void SetupSaturationProperties(CavrnusSpaceConnection spaceConn, ColorAdjustments ca)
        {
            saturationSlider.Slider.minValue = -100f;
            saturationSlider.Slider.maxValue = 100f;

            saturationToggle.onValueChanged.AddListener(SaturationToggleUIUpdated);

            spaceConn.DefineBoolPropertyDefaultValue(containerName, saturationEnabledPropertyName, ca.saturation.overrideState);
            disposables.Add(spaceConn.BindBoolPropertyValue(containerName, saturationEnabledPropertyName, val => {
                ca.saturation.overrideState = val;
                saturationToggle.SetIsOnWithoutNotify(val);
            }));

            spaceConn.DefineFloatPropertyDefaultValue(containerName, saturationValuePropertyName, ca.saturation.value);
            disposables.Add(spaceConn.BindFloatPropertyValue(containerName, saturationValuePropertyName, val => {
                ca.saturation.value = val;
                saturationSlider.Slider.SetValueWithoutNotify(val);
            }));

            saturationSlider.OnValueUpdated += SaturationValueChanged;
            saturationSlider.OnBeginDragging += SaturationDragBegin;
            saturationSlider.OnEndDragging += SaturationDragEnd;
        }

        private void SaturationToggleUIUpdated(bool val)
        {
            spaceConn?.PostBoolPropertyUpdate(containerName, saturationEnabledPropertyName, val);
        }

        private void SaturationDragBegin(float val) 
        {
            liveContrastUpdate ??= spaceConn.BeginTransientFloatPropertyUpdate(containerName, saturationValuePropertyName, val);
        }

        private void SaturationValueChanged(float val) 
        {
            liveContrastUpdate?.UpdateWithNewData(val);
        }

        private void SaturationDragEnd(float val) 
        {
            liveContrastUpdate?.Finish();
            liveContrastUpdate = null;
        }
        
        #endregion

        #region Hue Shift Property

        private void SetupHueShiftProperties(CavrnusSpaceConnection spaceConn, ColorAdjustments ca)
        {
            hueShiftSlider.Slider.minValue = -100f;
            hueShiftSlider.Slider.maxValue = 100f;

            hueShiftToggle.onValueChanged.AddListener(HueShiftToggleUpdated);

            spaceConn.DefineBoolPropertyDefaultValue(containerName, hueShiftEnabledPropertyName, ca.hueShift.overrideState);
            disposables.Add(spaceConn.BindBoolPropertyValue(containerName, hueShiftEnabledPropertyName, val => {
                ca.hueShift.overrideState = val;
                hueShiftToggle.SetIsOnWithoutNotify(val);
            }));

            spaceConn.DefineFloatPropertyDefaultValue(containerName, hueShiftValuePropertyName, ca.hueShift.value);
            disposables.Add(spaceConn.BindFloatPropertyValue(containerName, hueShiftValuePropertyName, val => {
                ca.hueShift.value = val;
                hueShiftSlider.Slider.SetValueWithoutNotify(val);
            }));

            hueShiftSlider.OnValueUpdated += HueShiftSliderValueUpdated;
            hueShiftSlider.OnBeginDragging += HueShiftDragBegin;
            hueShiftSlider.OnEndDragging += HueShiftDragEnd;
        }
        
        private void HueShiftDragBegin(float val)
        {
            liveHueShiftUpdate ??= spaceConn.BeginTransientFloatPropertyUpdate(containerName, hueShiftValuePropertyName, val);
        }

        private void HueShiftSliderValueUpdated(float val)
        {
            liveHueShiftUpdate?.UpdateWithNewData(val);
        }
        
        private void HueShiftDragEnd(float val)
        {
            liveHueShiftUpdate?.Finish();
            liveHueShiftUpdate = null;
        }

        private void HueShiftToggleUpdated(bool val)
        {
            spaceConn?.PostBoolPropertyUpdate(containerName, hueShiftEnabledPropertyName, val);
        }
        
        #endregion
        
        private void OnDestroy()
        {
            foreach (var disposable in disposables)
                disposable.Dispose();
            
            saturationSlider.OnValueUpdated -= SaturationValueChanged;
            saturationSlider.OnBeginDragging -= SaturationDragBegin;
            saturationSlider.OnEndDragging -= SaturationDragEnd;
            
            hueShiftSlider.OnValueUpdated -= HueShiftSliderValueUpdated;
            hueShiftSlider.OnBeginDragging -= HueShiftDragBegin;
            hueShiftSlider.OnEndDragging -= HueShiftDragEnd;
        }
    }
}