using System;
using System.Collections.Generic;
using System.Linq;
using CavrnusSdk.API;
using UnityEngine;

namespace CavrnusSdk.CollaborationExamples
{
    public class SyncStepsMenu : MonoBehaviour
    {
        [Serializable]
        public class StepInfo
        {
            public string StepName;

            public CavrnusSpaceConnection SpaceConn{ get; private set; }
            private string containerName;
            private string propertyName;

            public void Setup(CavrnusSpaceConnection spaceConn, string containerName, string propertyName)
            {
                SpaceConn = spaceConn;
                this.containerName = containerName;
                this.propertyName = propertyName;
            }

            public IDisposable BindStepStatusUpdate(Action<bool> onUpdate)
            {
                return SpaceConn.BindBoolPropertyValue(containerName, propertyName, onUpdate);
            }
            
            public void PostStepCompleteState(bool state)
            {
                SpaceConn.PostBoolPropertyUpdate(containerName, propertyName, state);
            }

            public bool IsComplete()
            {
                return SpaceConn.GetBoolPropertyValue(containerName, propertyName);
            }
        }

        [SerializeField] private string propertyContainer = "ExampleProcedure";
        [SerializeField] private string propertyNameProgress = "PropertyNameProgress";
        [SerializeField] private List<StepInfo> steps;

        [Space]
        [SerializeField] private GameObject toolTip;
        [SerializeField] private GameObject syncStepsItemPrefab;
        [SerializeField] private Transform syncStepsItemContainer;

        private CavrnusSpaceConnection spaceConn;

        private List<SyncStepsItem> syncUiItems = new List<SyncStepsItem>();
        
        private List<IDisposable> bindings = new List<IDisposable>();
        
        private void Start()
        {
            CavrnusFunctionLibrary.AwaitAnySpaceConnection(spaceConn => {
                this.spaceConn = spaceConn;
                
                spaceConn.DefineBoolPropertyDefaultValue(propertyContainer, propertyNameProgress, false);
                bindings.Add(spaceConn.BindBoolPropertyValue(propertyContainer, propertyNameProgress, isComplete => {
                    toolTip.SetActive(isComplete);
                }));

                for (var index = 0; index < steps.Count; index++) {
                    var stepInfo = steps[index];
                    stepInfo.Setup(spaceConn, propertyContainer, index.ToString());

                    var go = Instantiate(syncStepsItemPrefab, syncStepsItemContainer);
                    var item = go.GetComponent<SyncStepsItem>();
                    item.Setup(stepInfo, StepCompleted);
                    syncUiItems.Add(item);
                }
            });
        }

        private void StepCompleted(StepInfo obj)
        {
            // Here we can check if all steps are completed

            var allStepsComplete = syncUiItems.All(s => s.IsComplete());
            spaceConn.PostBoolPropertyUpdate(propertyContainer, propertyNameProgress, allStepsComplete);
        }

        public void ResetSteps()
        {
            // Overall progress reset
            spaceConn.PostBoolPropertyUpdate(propertyContainer, propertyNameProgress, false);
            
            // Post default value for reach step
            syncUiItems.ForEach(s => s.ResetItem());
        }
    }
}