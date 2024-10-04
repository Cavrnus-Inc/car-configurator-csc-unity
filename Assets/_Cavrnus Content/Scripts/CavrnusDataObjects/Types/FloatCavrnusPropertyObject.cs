using CavrnusSdk.API;
using UnityEngine;

namespace CavrnusDemo.CavrnusDataObjects
{
    [CreateAssetMenu(fileName = "CavrnusFloatProperty", menuName = "Cavrnus/CarData/FloatData", order = 0)]
    public class FloatCavrnusPropertyObject : CavrnusPropertyObject<float>
    {
        protected override void OnSpaceConnected(CavrnusSpaceConnection spaceConnection)
        {
            spaceConnection.DefineFloatPropertyDefaultValue(ContainerName, PropertyName, DefaultValue);
        }
    }
}