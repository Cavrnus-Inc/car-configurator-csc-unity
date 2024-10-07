using CavrnusSdk.API;
using UnityEngine;

namespace CavrnusDemo.CavrnusDataObjects
{
    [CreateAssetMenu(fileName = "CavrnusFloatProperty", menuName = "Cavrnus/PropertyObjects/FloatData", order = 0)]
    public class FloatCavrnusPropertyObject : CavrnusPropertyObject<float>
    {
        [SerializeField] private Vector2 minMaxSliderLimits;
        public Vector2 MinMaxSliderLimits => minMaxSliderLimits;
        
        private CavrnusLivePropertyUpdate<float> livePropertyUpdate;

        public override void PostValue(float value)
        {
            SpaceConnection?.PostFloatPropertyUpdate(ContainerName, PropertyName, value);
        }
        
        public void TransientUpdateWithNewData(float sliderValue)
        {
            livePropertyUpdate?.UpdateWithNewData(sliderValue);
        }

        public void BeginTransientUpdate(float sliderValue)
        {
            livePropertyUpdate ??= SpaceConnection.BeginTransientFloatPropertyUpdate(ContainerName, PropertyName, sliderValue);
        }

        public void FinishTransient()
        {
            livePropertyUpdate?.Finish();
            livePropertyUpdate = null;
        }

        protected override void SetBinding()
        {
            SpaceConnection.DefineFloatPropertyDefaultValue(ContainerName, PropertyName, DefaultValue);
            Binding = SpaceConnection.BindFloatPropertyValue(ContainerName, PropertyName, SendUpdateEvent);
        }
    }
}