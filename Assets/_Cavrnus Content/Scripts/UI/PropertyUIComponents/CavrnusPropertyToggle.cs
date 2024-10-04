using System;
using CavrnusDemo.CavrnusDataObjects;
using CavrnusSdk.API;
using UnityEngine;
using UnityEngine.UI;

namespace Cavrnus.UI
{
    [RequireComponent(typeof(Toggle))]
    public class CavrnusPropertyToggle : MonoBehaviour
    {
        [SerializeField] private BoolCavrnusPropertyObject propertyInfo;

        private Toggle toggle;
        private CavrnusSpaceConnection spaceConn;
        private IDisposable binding;

        private void Start()
        {
            toggle = gameObject.GetComponent<Toggle>();
            if (toggle == null) {
                print("Toggle is null!");
                return;
            }

            CavrnusFunctionLibrary.AwaitAnySpaceConnection(sc => {
                spaceConn = sc;
                binding = sc.BindBoolPropertyValue(propertyInfo.ContainerName, propertyInfo.PropertyName, b => {
                    toggle.isOn = b;
                });
                
                toggle.onValueChanged.AddListener(ToggleClicked);
            });
        }

        private void ToggleClicked(bool val)
        {
            spaceConn?.PostBoolPropertyUpdate(propertyInfo.ContainerName, propertyInfo.PropertyName, val);
        }

        private void OnDestroy()
        {
            toggle?.onValueChanged.RemoveListener(ToggleClicked);
            binding?.Dispose();
        }
    }
}