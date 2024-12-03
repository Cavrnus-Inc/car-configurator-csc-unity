using CavrnusSdk.API;
using UnityEngine;

namespace CavrnusSdk.CollaborationExamples
{
    public class SyncStepsAllComplete : MonoBehaviour
    {
        [SerializeField] private string propertyContainer = "ExampleProcedure";
        [SerializeField] private string propertyName = "PropertyNameProgress";

        [SerializeField] private Material incompleteMaterial;
        [SerializeField] private Material completeMaterial;
        
        private void Start()
        {
            CavrnusFunctionLibrary.AwaitAnySpaceConnection(spaceConn => {
                spaceConn.BindBoolPropertyValue(propertyContainer, propertyName, isComplete => {
                    GetComponent<Renderer>().material = isComplete ? completeMaterial : incompleteMaterial;
                });
            });
        }
    }
}