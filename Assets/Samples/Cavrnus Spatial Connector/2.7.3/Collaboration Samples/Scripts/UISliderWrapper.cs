using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CavrnusSdk.CollaborationExamples
{
    public class UISliderWrapper : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IPointerClickHandler
    {
        public event Action<float> OnValueUpdated;         
        public event Action<float> OnBeginDragging;         
        public event Action<float> OnEndDragging;

        public Slider Slider => slider;
        [SerializeField] private Slider slider;

        private void Awake()
        {
            slider.onValueChanged.AddListener(OnValueChanged);
        }

        private void OnValueChanged(float val)
        {
            OnValueUpdated?.Invoke(val);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            OnBeginDragging?.Invoke(Slider.value);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            OnEndDragging?.Invoke(Slider.value);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            
        }
    }
}