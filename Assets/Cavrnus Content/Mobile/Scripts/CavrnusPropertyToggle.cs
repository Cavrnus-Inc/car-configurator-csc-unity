using System;
using CavrnusSdk.API;
using UnityEngine;
using UnityEngine.UI;

namespace Cavrnus.UI
{
    public class CavrnusPropertyToggle : MonoBehaviour
    {
        [SerializeField] private string containerName;
        [SerializeField] private string propertyName;

        [Space]
        [SerializeField] private Toggle toggle;

        private CavrnusSpaceConnection spaceConn;
        private IDisposable binding;

        private void Start()
        {
            CavrnusFunctionLibrary.AwaitAnySpaceConnection(sc => {
                spaceConn = sc;
                sc.DefineBoolPropertyDefaultValue(containerName,propertyName,false);
                binding = sc.BindBoolPropertyValue(containerName, propertyName, b => {
                    toggle.isOn = b;
                });
                
                toggle.onValueChanged.AddListener(ToggleClicked);
            });
        }

        private void ToggleClicked(bool val)
        {
            spaceConn?.PostBoolPropertyUpdate(containerName, propertyName, val);
        }

        private void OnDestroy()
        {
            toggle.onValueChanged.RemoveListener(ToggleClicked);
            binding?.Dispose();
        }
    }
}