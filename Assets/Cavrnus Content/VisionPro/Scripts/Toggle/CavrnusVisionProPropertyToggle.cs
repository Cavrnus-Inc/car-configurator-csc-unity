using System;
using CavrnusSdk.API;
using UnityEngine;

namespace Cavrnus_Content.Scripts.VisionProComponents
{
    public class CavrnusVisionProPropertyToggle : MonoBehaviour
    {
        [SerializeField] private string containerName;
        [SerializeField] private string propertyName;

        [Space]
        [SerializeField] private CavrnusVisionProToggleComponent toggle;

        private CavrnusSpaceConnection spaceConn;
        private IDisposable binding;

        private void Start()
        {
            CavrnusFunctionLibrary.AwaitAnySpaceConnection(sc => {
                spaceConn = sc;
                sc.DefineBoolPropertyDefaultValue(containerName,propertyName,false);
                binding = sc.BindBoolPropertyValue(containerName, propertyName, b => {
                    toggle.IsOn = b;
                });
                
                toggle.OnToggleChanged.AddListener(ToggleClicked);
            });
        }
        
        private void ToggleClicked(bool val)
        {
            spaceConn?.PostBoolPropertyUpdate(containerName, propertyName, val);
        }

        private void OnDestroy()
        {
            toggle.OnToggleChanged.RemoveListener(ToggleClicked);
            binding?.Dispose();
        }
    }
}