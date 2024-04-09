using System;
using System.Collections.Generic;
using CavrnusSdk.API;
using UnityBase;
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
        [SerializeField] private UISliderWrapper intensityUISlider;
        [SerializeField] private Vector2 intensityMinMax = new Vector2(10f, 150f);

        [Header("Color Options")]
        [SerializeField] private List<Color> colors;
        [SerializeField] private GameObject colorPrefab;
        [SerializeField] private Transform container;

        private List<LightSyncColorItem> lightColorItems = new List<LightSyncColorItem>();

        private CavrnusSpaceConnection spaceConn;
        private List<IDisposable> disposables = new List<IDisposable>();
        
        private CavrnusLivePropertyUpdate<float> liveIntensityUpdate = null;

        private void Start()
        {
            CavrnusFunctionLibrary.AwaitAnySpaceConnection(spaceConn => {
                this.spaceConn = spaceConn;

                intensityUISlider.Slider.minValue = intensityMinMax.x;
                intensityUISlider.Slider.maxValue = intensityMinMax.y;
                
                // Setup UI
                foreach (var color in colors) {
                    var go = Instantiate(colorPrefab, container);
                    var colorItem = go.GetComponent<LightSyncColorItem>();
                    lightColorItems.Add(colorItem);
                    
                    colorItem.Setup(color, ColorSelected);
                }
                
                // Setup Bindings
                spaceConn.DefineColorPropertyDefaultValue(containerName, colorPropertyName, lightComponent.color);
                disposables.Add(spaceConn.BindColorPropertyValue(containerName, colorPropertyName, serverColor => {
                    lightComponent.color = serverColor;

                    foreach (var item in lightColorItems) {
                        item.SetSelectionState(ColorsEqual(item.Color, serverColor));
                    }
                }));
                
                spaceConn.DefineFloatPropertyDefaultValue(containerName, intensityPropertyName, lightComponent.intensity);
                disposables.Add(spaceConn.BindFloatPropertyValue(containerName, intensityPropertyName, intensity => {
                    lightComponent.intensity = intensity;
                    intensityUISlider.Slider.SetValueWithoutNotify(intensity);
                }));
                
                intensityUISlider.OnValueUpdated += IntensityValueChanged;
                intensityUISlider.OnBeginDragging += IntensityDragBegin;
                intensityUISlider.OnEndDragging += IntensityDragEnd;
            });
        }
        
        private void IntensityDragBegin(float val)
        {
            liveIntensityUpdate ??= spaceConn.BeginTransientFloatPropertyUpdate(containerName, intensityPropertyName, val);
        }

        private void IntensityDragEnd(float val)
        {
            liveIntensityUpdate?.Finish();
            liveIntensityUpdate = null;
        }

        private void IntensityValueChanged(float val)
        {
            liveIntensityUpdate?.UpdateWithNewData(val);
        }

        private void ColorSelected(Color color)
        {
            spaceConn?.PostColorPropertyUpdate(containerName, colorPropertyName, color);
        }
        
        private bool ColorsEqual(Color c1, Color c2, float tolerance = 0.1f)
        {
            return c1.r.AlmostEquals(c2.r, tolerance) &&
                   c1.g.AlmostEquals(c2.g, tolerance) &&
                   c1.b.AlmostEquals(c2.b, tolerance);
        }

        private void OnDestroy()
        {
            foreach (var disposable in disposables) {
                disposable.Dispose();
            }
            
            intensityUISlider.OnValueUpdated -= IntensityValueChanged;
            intensityUISlider.OnBeginDragging -= IntensityDragBegin;
            intensityUISlider.OnEndDragging -= IntensityDragEnd;
        }
    }
}