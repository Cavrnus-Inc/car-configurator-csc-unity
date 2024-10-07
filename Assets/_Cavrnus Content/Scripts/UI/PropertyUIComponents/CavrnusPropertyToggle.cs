using CavrnusDemo.CavrnusDataObjects;
using UnityEngine;
using UnityEngine.UI;

namespace Cavrnus.UI
{
    [RequireComponent(typeof(Toggle))]
    public class CavrnusPropertyToggle : MonoBehaviour
    {
        [SerializeField] private BoolCavrnusPropertyObject propertyInfo;
        
        private Toggle toggle;
        
        private void Awake()
        {
            toggle = gameObject.GetComponent<Toggle>();
            if (toggle == null) {
                print("Missing required Toggle!");
                return;
            }
            
            propertyInfo.OnServerValueUpdated += OnServerValueUpdated;
            toggle.onValueChanged.AddListener(ToggleClicked);
        }
        
        private void OnServerValueUpdated(bool val)
        {
            toggle.isOn = val;
        }
        
        private void ToggleClicked(bool val)
        {
            propertyInfo.PostValue(val);
        }
        
        private void OnDestroy()
        {
            toggle.onValueChanged.RemoveListener(ToggleClicked);
            propertyInfo.OnServerValueUpdated -= OnServerValueUpdated;
        }
    }
}