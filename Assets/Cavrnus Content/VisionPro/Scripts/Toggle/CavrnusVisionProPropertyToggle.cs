using System;
using CavrnusSdk.API;
using UnityEngine;

namespace Cavrnus_Content.Scripts.VisionProComponents
{
    [RequireComponent(typeof(CavrnusVisionProToggleComponent))]
    public class CavrnusVisionProPropertyToggle : MonoBehaviour
    {
        [SerializeField] private string containerName;
        [SerializeField] private string propertyName;

        private CavrnusVisionProToggleComponent toggle;

        private CavrnusSpaceConnection spaceConn;
        private IDisposable binding;

        private void Start()
        {
            CavrnusFunctionLibrary.AwaitAnySpaceConnection(sc => {
                spaceConn = sc;
                toggle = GetComponent<CavrnusVisionProToggleComponent>();
                
                print($"INITIALIZING THIS YOOOOOO");
                sc.DefineBoolPropertyDefaultValue(containerName,propertyName,false);
                binding = sc.BindBoolPropertyValue(containerName, propertyName, b => {
                    toggle.SetToggledStatus(b, false);
                });
                
                toggle.OnToggleChanged.AddListener(ToggleClicked);
            });
        }
        
        private void ToggleClicked(bool val)
        {
            print($"SpaceConn: {spaceConn}: Toggling click");
            spaceConn?.PostBoolPropertyUpdate(containerName, propertyName, val);
        }

        private void OnDestroy()
        {
            toggle.OnToggleChanged.RemoveListener(ToggleClicked);
            binding?.Dispose();
        }
    }
}