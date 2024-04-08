using System.Collections;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace CavrnusDemo.SdkExtensions
{
    public class RotationAnimator : MonoBehaviour
    {
        public UnityEvent<bool> OnValueChanged;
        
        [Range(5f, 100f)]
        [SerializeField] private float rotationSpeed = 50f;

        [SerializeField] private AnimationCurve animationCurve;

        [SerializeField, ReadOnly] private Quaternion startingRotation = Quaternion.identity;
        [SerializeField, ReadOnly] private Quaternion endingRotation = Quaternion.identity;

        public bool AtStart{ get; private set; }

        public void ToggleAnimation()
        {
            if (AtStart)
                MoveToStart();
            else 
                MoveToEnd();

            AtStart = !AtStart;
            OnValueChanged?.Invoke(AtStart);
        }
        
        public void SetState(bool setToStart)
        {
            if (setToStart)
                MoveToStart();
            else 
                MoveToEnd();
        }

        public void MoveToStart()
        {
            if (currentRoutine != null) 
                StopCoroutine(currentRoutine);
            currentRoutine = StartCoroutine(RotationRoutine(startingRotation));
            AtStart = true;
        }

        public void MoveToEnd()
        {
            if (currentRoutine != null) 
                StopCoroutine(currentRoutine);
            currentRoutine = StartCoroutine(RotationRoutine(endingRotation));
            AtStart = false;
        }

        public void SetBegin() => startingRotation = transform.localRotation;

        public void SetEnd() => endingRotation = transform.localRotation;

        private Coroutine currentRoutine;
        private IEnumerator RotationRoutine(Quaternion target)
        {   
            // Calculate the angle to rotate
            var angleToRotate = Quaternion.Angle(transform.localRotation, target);

            // Calculate the duration based on speed and angle
            var duration = angleToRotate / rotationSpeed;

            var elapsedTime = 0f;
            var start = transform.localRotation;

            while (elapsedTime < duration)
            {
                var progress = elapsedTime / duration;
                var curvePercentage = animationCurve.Evaluate(progress);
                transform.localRotation = Quaternion.Slerp(start, target, curvePercentage);
                
                elapsedTime += Time.deltaTime;

                yield return null;
            }

            transform.localRotation = target;
        }
    }
}