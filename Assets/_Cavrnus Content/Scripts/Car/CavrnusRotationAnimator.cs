using System;
using System.Collections;
using CavrnusDemo.CavrnusDataObjects;
using CavrnusSdk.API;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace CavrnusDemo
{
    public class CavrnusRotationAnimator : MonoBehaviour
    {
        public UnityEvent<bool> OnValueChanged;

        [SerializeField] private BoolCavrnusPropertyObject boolProperty;
        
        [Range(5f, 100f)]
        [SerializeField] private float rotationSpeed = 50f;

        [SerializeField] private AnimationCurve animationCurve;

        [SerializeField, ReadOnly] private Quaternion startingRotation = Quaternion.identity;
        [SerializeField, ReadOnly] private Quaternion endingRotation = Quaternion.identity;
        
        public bool AtEnd{ get; private set; }
        
        private CavrnusSpaceConnection spaceConnection;
        private IDisposable binding;

        private void Start()
        {
            CavrnusFunctionLibrary.AwaitAnySpaceConnection(sc => {
                spaceConnection = sc;
                binding = sc.BindBoolPropertyValue(boolProperty.ContainerName, boolProperty.PropertyName, SetState);
            });
        }

        public void Toggle()
        {
            var current = spaceConnection.GetBoolPropertyValue(boolProperty.ContainerName, boolProperty.PropertyName);
            spaceConnection.PostBoolPropertyUpdate(boolProperty.ContainerName, boolProperty.PropertyName, !current);
        }
        
        public void SetState(bool setToEnd)
        {
            if (setToEnd)
                MoveToEnd();
            else 
                MoveToStart();
        }

        public void MoveToStart()
        {
            if (currentRoutine != null) 
                StopCoroutine(currentRoutine);
            currentRoutine = StartCoroutine(RotationRoutine(startingRotation));
            AtEnd = true;
        }

        public void MoveToEnd()
        {
            if (currentRoutine != null) 
                StopCoroutine(currentRoutine);
            currentRoutine = StartCoroutine(RotationRoutine(endingRotation));
            AtEnd = false;
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

        private void OnDestroy()
        {
            binding?.Dispose();
        }
    }
}