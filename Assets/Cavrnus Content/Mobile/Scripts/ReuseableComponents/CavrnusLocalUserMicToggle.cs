using System;
using CavrnusSdk.API;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Cavrnus_Content.Mobile.Scripts.UI
{
    public class CavrnusLocalUserMicToggle : MonoBehaviour
    {
        [SerializeField] private UnityEvent OnClicked;
        
        [SerializeField] private UnityEvent<bool> OnStateChanged;
        
        private string ContainerName;
        
        [SerializeField] private string propertyName = "muted";

        [Space]
        [SerializeField] private Toggle toggle;
        
        private CavrnusSpaceConnection spaceConn;
        private IDisposable binding;
        private CavrnusLivePropertyUpdate<bool> updater;

        private void Start()
        {
            CavrnusFunctionLibrary.AwaitAnySpaceConnection(sc => {
                spaceConn = sc;
                
                sc.AwaitLocalUser(user => {
                    ContainerName = user.ContainerId;
                    sc.DefineBoolPropertyDefaultValue(ContainerName,propertyName,false);
                    binding = sc.BindBoolPropertyValue(ContainerName, propertyName, b => {
                        b = !b;
                        toggle.isOn = b;
                        OnStateChanged?.Invoke(b);
                    });
                
                    toggle.onValueChanged.AddListener(ToggleClicked);
                });
            });
        }

        private void ToggleClicked(bool val)
        {
            OnClicked?.Invoke();
        }

        private void OnDestroy()
        {
            toggle.onValueChanged.RemoveListener(ToggleClicked);
            binding?.Dispose();
        }
    }
}