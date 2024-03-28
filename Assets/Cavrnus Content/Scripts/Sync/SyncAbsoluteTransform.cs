using CavrnusSdk.API;
using CavrnusSdk.PropertySynchronizers;

namespace CavrnusDemo
{
    public class SyncAbsoluteTransform : CavrnusTransformPropertySynchronizer
    {
        public override CavrnusTransformData GetValue()
        {
            return new CavrnusTransformData(transform.position, transform.eulerAngles, transform.lossyScale);
        }

        public override void SetValue(CavrnusTransformData value)
        {
            transform.position = value.Position;
            transform.eulerAngles = value.EulerAngles;
            transform.localScale = value.Scale;
        }
    }
}