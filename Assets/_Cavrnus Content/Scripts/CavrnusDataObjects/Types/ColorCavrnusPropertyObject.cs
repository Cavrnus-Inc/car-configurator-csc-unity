using CavrnusSdk.API;
using UnityEngine;

namespace CavrnusDemo.CavrnusDataObjects
{
    [CreateAssetMenu(fileName = "CavrnusColorProperty", menuName = "Cavrnus/CarData/ColorData", order = 0)]
    public class ColorCavrnusPropertyObject : CavrnusPropertyObject<Color>
    {
        protected override void OnSpaceConnected(CavrnusSpaceConnection spaceConnection)
        {
            spaceConnection.DefineColorPropertyDefaultValue(ContainerName, PropertyName, DefaultValue);
        }
    }
}