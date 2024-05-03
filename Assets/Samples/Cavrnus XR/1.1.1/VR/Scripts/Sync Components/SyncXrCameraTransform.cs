using CavrnusSdk.API;
using CavrnusSdk.PropertySynchronizers;
using UnityEngine;

namespace XR
{
    public class SyncXrCameraTransform : CavrnusValueSyncTransform
    {
        [SerializeField] private bool clampYPosToZero = false;
        
        public override CavrnusTransformData GetValue()
        {
            var pos = transform.position;
            var posAdjust = new Vector3(pos.x, clampYPosToZero ? 0 : pos.y, pos.z);
            
            var rot = transform.rotation.eulerAngles;
            var rotAdjust = new Vector3(0, rot.y, 0);
            
            return new CavrnusTransformData(posAdjust, rotAdjust, transform.lossyScale);
        }

        public override void SetValue(CavrnusTransformData value)
        {
            transform.position = value.Position;
            transform.eulerAngles = value.EulerAngles;
            transform.localScale = value.Scale;
        }
    }
}