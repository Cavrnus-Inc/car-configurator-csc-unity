using CavrnusSdk.API;
using UnityEngine;

namespace CavrnusSdk.CollaborationExamples
{
    public class SyncStepObject : MonoBehaviour
    {
        [SerializeField] private string propertyContainer = "ExampleProcedure";
        [SerializeField] private string propertyName;

        [SerializeField] private GameObject onObject;
        [SerializeField] private GameObject offObject;

        private void Start()
        {
            CavrnusFunctionLibrary.AwaitAnySpaceConnection(spaceConn => {
                spaceConn.BindBoolPropertyValue(propertyContainer, propertyName, isComplete => {
                    onObject.SetActive(isComplete);
                    offObject.SetActive(!isComplete);
                });
            });
        }
    }
}