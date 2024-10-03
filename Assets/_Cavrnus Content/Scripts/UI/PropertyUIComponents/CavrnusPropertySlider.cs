using System;
using CavrnusSdk.API;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CavrnusSdk.CollaborationExamples
{
    [RequireComponent(typeof(Slider))]
    public class CavrnusPropertySlider : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IPointerClickHandler
    {
        [SerializeField] private string containerName;
        [SerializeField] private string propertyName;

        [Space]
        [SerializeField] private float defaultValue = 0f;
        
        private Slider slider;

        private CavrnusLivePropertyUpdate<float> livePropertyUpdate;
        private IDisposable binding;
        private CavrnusSpaceConnection spaceConn;

        private void Start()
        {
            slider = GetComponent<Slider>();
            if (!slider) {
                print("Missing required slider!");
                return;
            }
            
            CavrnusFunctionLibrary.AwaitAnySpaceConnection(sc => {
                spaceConn = sc;
                sc.DefineFloatPropertyDefaultValue(containerName, propertyName, defaultValue);
                binding = sc.BindFloatPropertyValue(containerName, propertyName, val => {
                    slider.SetValueWithoutNotify(val);
                });
                
                slider.onValueChanged.AddListener(OnValueChanged);
            });
        }

        private void OnValueChanged(float val)
        {
            livePropertyUpdate?.UpdateWithNewData(val);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            livePropertyUpdate ??= spaceConn.BeginTransientFloatPropertyUpdate(containerName, propertyName, slider.value);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            livePropertyUpdate?.Finish();
            livePropertyUpdate = null;
        }

        public void OnPointerClick(PointerEventData eventData) { }

        private void OnDestroy()
        {
            binding?.Dispose();
            slider?.onValueChanged.RemoveListener(OnValueChanged);
        }
    }
}