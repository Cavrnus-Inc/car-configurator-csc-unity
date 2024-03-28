using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace CavrnusDemo
{
    public class CarLift : MonoBehaviour
    {
        public enum PositionEnum
        {
            Top,
            Bottom
        }

        public PositionEnum position;
        
        public UnityEvent OnTopReached;
        public UnityEvent OnBottomReached;
        
        public Transform LiftActuatorTransform;
        [SerializeField] private Transform objToLift;

        [SerializeField] private float yFloor;
        [SerializeField] private float yCeil;

        [SerializeField] private float speed;

        private float currentHeight;

        public void ResetTransform()
        {
            if (currRoutine != null)
            {
                StopCoroutine(currRoutine);
                currRoutine = null;
            }
            currentHeight = 0;
            LiftActuatorTransform.transform.position = new Vector3(0, yFloor, 0);
        }

        public void Raise()
        {
            if (currRoutine != null)
                StopCoroutine(currRoutine);

            currRoutine = StartCoroutine(MoveRoutine(true));
        }

        public void Lower()
        {
            if (currRoutine != null) 
                StopCoroutine(currRoutine);

            currRoutine = StartCoroutine(MoveRoutine(false));
        }

        public void Stop()
        {
            if (currRoutine != null) 
                StopCoroutine(currRoutine);
        }

        public void ForceState(PositionEnum state)
        {
            if (state == PositionEnum.Top)
                currentHeight = yCeil;
            else
                currentHeight = yFloor;
        }

        private Coroutine currRoutine;

        private IEnumerator MoveRoutine(bool isRaising)
        {
            var targetHeight = isRaising ? yCeil : yFloor;
            var direction = isRaising ? 1f : -1f;

            while (Mathf.Abs(currentHeight - targetHeight) > 0.01f) {
                var newY = Mathf.MoveTowards(currentHeight, targetHeight, speed * Time.deltaTime);
                LiftActuatorTransform.localPosition = new Vector3(LiftActuatorTransform.localPosition.x, newY, LiftActuatorTransform.localPosition.z);
                currentHeight = newY;

                LiftActuatorTransform.position += Vector3.up * speed * Time.deltaTime * direction;
                
                if (objToLift != null)
                {
                    var distanceToMove = speed * Time.deltaTime * direction;
                    objToLift.position += Vector3.up * distanceToMove;
                }

                yield return null;
            }

            if (isRaising)
            {
                OnTopReached?.Invoke();
                position = PositionEnum.Top;
            }
            else
            {
                OnBottomReached?.Invoke();
                position = PositionEnum.Bottom;
            }
        }
    }
}