using System;
using CavrnusSdk.API;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Cavrnus_Content.Mobile.Scripts.UI
{
    public class CavrnusBoundOnlyPropertyToggle : MonoBehaviour, IPointerClickHandler
    {
        public UnityEvent OnClicked;
        public UnityEvent<bool> OnValueChanged;
        
        private string ContainerName;
        
        [SerializeField] private string propertyName;

        [Space]
        [SerializeField] private Toggle toggle;

        private IDisposable binding;

        private void Start()
        {
            CavrnusFunctionLibrary.AwaitAnySpaceConnection(sc => {
                
                sc.AwaitLocalUser(user => {
                    ContainerName = user.ContainerId;
                    sc.DefineBoolPropertyDefaultValue(ContainerName,propertyName,false);
                    binding = sc.BindBoolPropertyValue(ContainerName, propertyName, b => {
                        OnValueChanged?.Invoke(b);
                        toggle.isOn = b;
                    });
                });
            });
        }

        private void OnDestroy()
        {
            binding?.Dispose();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            OnClicked.Invoke();
        }
    }
}