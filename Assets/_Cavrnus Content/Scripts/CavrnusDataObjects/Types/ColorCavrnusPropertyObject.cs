using CavrnusSdk.API;
using UnityEngine;

namespace CavrnusDemo.CavrnusDataObjects
{
    [CreateAssetMenu(fileName = "CavrnusColorProperty", menuName = "Cavrnus/PropertyObjects/ColorData", order = 0)]
    public class ColorCavrnusPropertyObject : CavrnusPropertyObject<Color>
    {
        public override void PostValue(Color value)
        {
            SpaceConnection?.PostColorPropertyUpdate(ContainerName, PropertyName, value);
        }

        protected override void SetBinding()
        {
            SpaceConnection.DefineColorPropertyDefaultValue(ContainerName, PropertyName, DefaultValue);
            Binding = SpaceConnection.BindColorPropertyValue(ContainerName, PropertyName, SendUpdateEvent);
        }
    }
}