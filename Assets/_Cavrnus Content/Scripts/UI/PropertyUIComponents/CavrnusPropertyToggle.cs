using System;
using CavrnusSdk.API;
using UnityEngine;
using UnityEngine.UI;

namespace Cavrnus.UI
{
    public class CavrnusPropertyToggle : MonoBehaviour
    {
        [SerializeField] public string ContainerName;
        [SerializeField] public string PropertyName;

        [Space]
        [SerializeField] private Toggle toggle;

        private CavrnusSpaceConnection spaceConn;
        private IDisposable binding;

        private void Start()
        {
            CavrnusFunctionLibrary.AwaitAnySpaceConnection(sc => {
                spaceConn = sc;
                sc.DefineBoolPropertyDefaultValue(ContainerName,PropertyName,false);
                binding = sc.BindBoolPropertyValue(ContainerName, PropertyName, b => {
                    toggle.isOn = b;
                });
                
                toggle.onValueChanged.AddListener(ToggleClicked);
            });
        }

        private void ToggleClicked(bool val)
        {
            spaceConn?.PostBoolPropertyUpdate(ContainerName, PropertyName, val);
        }

        private void OnDestroy()
        {
            toggle.onValueChanged.RemoveListener(ToggleClicked);
            binding?.Dispose();
        }
    }
}