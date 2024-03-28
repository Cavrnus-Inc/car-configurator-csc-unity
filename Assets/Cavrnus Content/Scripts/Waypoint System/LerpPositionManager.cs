using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace CavrnusDemo
{
    public class LerpPositionManager : MonoBehaviour
    {
        public enum PositionEnum
        {
            Start = 0,
            AutoLift = 1,
            End = 2,
        }
        
        public UnityEvent OnWayPointReached;

        public Transform StartingTransform => waypoints[0].transform;
        
        [SerializeField] private Transform actor;
        [SerializeField] private float speed;
        
        [Space]
        [SerializeField] private List<Waypoint> waypoints;
        
        [SerializeField, HideInInspector]
        private int currentWaypointIndex = 0;

        private bool isActive;
        private Coroutine routine;

        public void ResetToStart()
        {
            if (routine != null) {
                StopCoroutine(routine);
            }

            isActive = false;
            actor.position = waypoints[0].transform.position;
            currentWaypointIndex = 0;
        }

        public void TargetPosition(PositionEnum targetPos)
        {
            if (isActive) {
                Debug.Log("Already moving!");
                return;
            }

            if ((int)targetPos > waypoints.Count || targetPos < 0) {
                Debug.LogWarning("Attempting to move object to invalid position!");
                return;
            }
            
            var targetWaypoint = waypoints[(int) targetPos].transform;
            routine = StartCoroutine(MoveObject(actor, targetWaypoint.position, speed));
            
            currentWaypointIndex = (int) targetPos;
        }
        
        private IEnumerator MoveObject(Transform objectTransform, Vector3 targetPosition, float speed)
        {
            isActive = true;

            while (Vector3.Distance(objectTransform.position, targetPosition) > 0.01f) { 
                objectTransform.position = Vector3.MoveTowards(objectTransform.position, targetPosition, speed * Time.deltaTime);
                yield return null;
            }
            
            OnWayPointReached?.Invoke();
            
            isActive = false;
        }
    }
}