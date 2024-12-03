using CavrnusSdk.API;
using UnityEngine;
using UnityEngine.UI;

namespace CavrnusSdk.CollaborationExamples
{
    public class ToggleVisibilityUIBinding : MonoBehaviour
    {
        [SerializeField] private Toggle toggle;

        [SerializeField] private string targetContainerName = "Container";
        [SerializeField] private string targetPropertyName = "Visible";
        [SerializeField] private string spaceTag = "";

        private CavrnusSpaceConnection spaceConn;

        private void Start()
        {
            CavrnusFunctionLibrary.AwaitSpaceConnectionByTag(spaceTag, spaceConn => {
                this.spaceConn = spaceConn;

                var defaultVal = spaceConn.GetBoolPropertyValue(targetContainerName, targetPropertyName);
                spaceConn.DefineBoolPropertyDefaultValue(targetContainerName, targetPropertyName, defaultVal);
                spaceConn.BindBoolPropertyValue(targetContainerName, targetPropertyName, b => {
                    toggle.SetIsOnWithoutNotify(b);
                });

                toggle.onValueChanged.AddListener(ToggleValueChanged);
            });
        }

        private void ToggleValueChanged(bool newValue)
        {
            spaceConn?.PostBoolPropertyUpdate(targetContainerName, targetPropertyName, newValue);
        }

        private void OnDestroy()
        {
            toggle.onValueChanged.RemoveListener(ToggleValueChanged);
        }
    }
}