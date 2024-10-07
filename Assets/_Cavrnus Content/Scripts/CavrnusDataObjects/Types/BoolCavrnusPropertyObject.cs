using CavrnusSdk.API;
using UnityEngine;

namespace CavrnusDemo.CavrnusDataObjects
{
    [CreateAssetMenu(fileName = "CavrnusBooleanProperty", menuName = "Cavrnus/PropertyObjects/BooleanData", order = 0)]
    public class BoolCavrnusPropertyObject : CavrnusPropertyObject<bool>
    {
        public override void PostValue(bool value)
        {
            SpaceConnection?.PostBoolPropertyUpdate(ContainerName, PropertyName, value);
        }
        
        protected override void SetBinding()
        {
            SpaceConnection.DefineBoolPropertyDefaultValue(ContainerName, PropertyName, DefaultValue);
            Binding = SpaceConnection.BindBoolPropertyValue(ContainerName, PropertyName, SendUpdateEvent);
        }
    }
}