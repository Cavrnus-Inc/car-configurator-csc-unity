using CavrnusSdk.API;
using UnityEngine;

namespace CavrnusDemo.CavrnusDataObjects
{
    [CreateAssetMenu(fileName = "CavrnusStringProperty", menuName = "Cavrnus/PropertyObjects/StringData", order = 0)]
    public class StringCavrnusPropertyObject : CavrnusPropertyObject<string>
    {
        public string DisplayName;
        public string Description;
        
        public override void PostValue(string value)
        {
            SpaceConnection?.PostStringPropertyUpdate(ContainerName, PropertyName, value);
        }
        
        protected override void SetBinding()
        {
            SpaceConnection.DefineStringPropertyDefaultValue(ContainerName, PropertyName, DefaultValue);
            Binding = SpaceConnection.BindStringPropertyValue(ContainerName, PropertyName, SendUpdateEvent);
        }
    }
}