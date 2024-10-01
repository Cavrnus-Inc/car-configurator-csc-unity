using UnityEngine;

namespace CavrnusSdk.XR
{
    public class XrRigHelper : MonoBehaviour
    {
        public Camera XrRig{ get; private set; }

        public Transform EyePosition;
        
        private void Awake()
        {
            XrRig = Camera.main;

            if (EyePosition == null) {
                EyePosition = Camera.main?.transform;
                Debug.Log($"Automatically assigning Eye Position to main camera!");
            }

            if (EyePosition == null) {
                Debug.LogError($"Missing eye position! Ensure it's assigned!");
            } 
        }
    }
}