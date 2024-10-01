using System;
using CavrnusSdk.API;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Cavrnus_Content.Mobile.Scripts.UI
{
    public class CavrnusLocalUserMicToggle : MonoBehaviour, IPointerClickHandler
    {
        public UnityEvent OnClicked;
        public UnityEvent<bool> OnValueChanged;
        
        private string ContainerName = "muted";
        
        [SerializeField] private string propertyName;

        [Space]
        [SerializeField] private Toggle toggle;

        private CavrnusSpaceConnection spaceConnection;
        private CavrnusUser localUser;

        private IDisposable binding;

        private void Start()
        {
            CavrnusFunctionLibrary.AwaitAnySpaceConnection(sc => {
                spaceConnection = sc;
                sc.AwaitLocalUser(user => {
                    localUser = user;
                    ContainerName = user.ContainerId;
                    sc.DefineBoolPropertyDefaultValue(ContainerName,propertyName,false);
                    binding = sc.BindBoolPropertyValue(ContainerName, propertyName, b => {
                        OnValueChanged?.Invoke(b);
                        toggle.isOn = b;
                    });
                });
                
                toggle.onValueChanged.AddListener(ValueChanged);
            });
        }

        private void ValueChanged(bool val)
        {
            if (localUser.IsLocalUser)
                spaceConnection?.SetLocalUserMutedState(val);
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