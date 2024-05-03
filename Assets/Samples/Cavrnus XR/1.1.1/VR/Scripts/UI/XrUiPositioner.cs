using UnityEngine;
using CavrnusSdk.API;

namespace CavrnusSdk.XR.UiPositioners
{
    public class XrUiPositioner : MonoBehaviour
    {
        [SerializeField] private float distanceFromEyes = 0.8f;
        [SerializeField] private float heightOffsetFromEyes = 0.2f;
        [SerializeField] private float xTilt = 0f;

        [SerializeField] private XrRigHelper xrRigHelper;

        private void Awake()
        {
            if (xrRigHelper == null) 
                Debug.LogWarning($"Missing {nameof(xrRigHelper)} in parent! {gameObject}");

			CavrnusFunctionLibrary.AwaitAnySpaceConnection(csc => RealignToEyeDirectionAndHeight());
        }
        
        private void OnEnable()
        {
            RealignToEyeDirectionAndHeight();
        }

        public void RealignToEyeDirectionAndHeight()
        {
            if (xrRigHelper?.EyePosition == null) 
                return;
            
            var eyeTransform = xrRigHelper.EyePosition;
            
            var targetRotation = eyeTransform.rotation.eulerAngles;
            transform.rotation = Quaternion.Euler(xTilt, targetRotation.y, 0);
            
            var adjustedForward = eyeTransform.position + eyeTransform.forward * distanceFromEyes;
            var adjustedHeight = new Vector3(adjustedForward.x, eyeTransform.position.y - heightOffsetFromEyes, adjustedForward.z);
            
            transform.position = adjustedHeight;
        }
    }
}