using CavrnusDemo.CavrnusDataObjects;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CavrnusSdk.CollaborationExamples
{
    [RequireComponent(typeof(Slider))]
    public class CavrnusPropertySlider : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IPointerClickHandler
    {
        [SerializeField] private FloatCavrnusPropertyObject propertyInfo;
        
        private Slider slider;

        private void Awake()
        {
            slider = GetComponent<Slider>();
            if (!slider) {
                print("Missing required slider!");
                return;
            }

            slider.minValue = propertyInfo.MinMaxSliderLimits.x;
            slider.maxValue = propertyInfo.MinMaxSliderLimits.y;
            
            propertyInfo.OnServerValueUpdated += OnServerValueUpdated;
            slider.onValueChanged.AddListener(OnValueChanged);
        }

        private void OnServerValueUpdated(float val)
        {
            slider.SetValueWithoutNotify(val);
        }

        private void OnValueChanged(float val)
        {
            propertyInfo.TransientUpdateWithNewData(val);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            propertyInfo.BeginTransientUpdate(slider.value);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            propertyInfo.FinishTransient();
        }

        public void OnPointerClick(PointerEventData eventData) { }

        private void OnDestroy()
        {
            propertyInfo.Unbind();
            propertyInfo.OnServerValueUpdated -= OnServerValueUpdated;
            
            slider.onValueChanged.RemoveListener(OnValueChanged);
        }
    }
}