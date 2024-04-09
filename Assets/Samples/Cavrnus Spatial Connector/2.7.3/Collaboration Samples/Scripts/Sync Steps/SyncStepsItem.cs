using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CavrnusSdk.CollaborationExamples
{
    public class SyncStepsItem : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI stepName;
        
        [Header("Status Components")]
        [SerializeField] private Button completeButton;
        [SerializeField] private GameObject completeCheckmark;

        private SyncStepsMenu.StepInfo stepInfo;

        private IDisposable binding;
        private Action<SyncStepsMenu.StepInfo> stepCompleted;

        public void Setup(SyncStepsMenu.StepInfo stepInfo, Action<SyncStepsMenu.StepInfo> stepCompleted)
        {
            stepName.text = stepInfo.StepName;
            this.stepInfo = stepInfo;
            this.stepCompleted = stepCompleted;

            binding = stepInfo.BindStepStatusUpdate(isComplete => {
                completeButton.gameObject.SetActive(!isComplete);
                completeCheckmark.gameObject.SetActive(isComplete);
                stepCompleted?.Invoke(stepInfo);
            });
        }

        public bool IsComplete()
        {
            return stepInfo.IsComplete();
        }

        public void ClickComplete()
        {
            stepInfo.PostStepCompleteState(true);
        }

        public void ResetItem()
        {
            stepInfo.PostStepCompleteState(false);
        }

        private void OnDestroy()
        {
            binding?.Dispose();
        }
    }
}