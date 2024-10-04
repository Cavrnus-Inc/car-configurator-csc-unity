using CavrnusSdk.API;
using UnityEngine;

namespace CavrnusDemo.CavrnusDataObjects
{
    [CreateAssetMenu(fileName = "CavrnusStringProperty", menuName = "Cavrnus/CarData/StringData", order = 0)]
    public class StringCavrnusPropertyObject : CavrnusPropertyObject<string>
    {
        protected override void OnSpaceConnected(CavrnusSpaceConnection spaceConnection)
        {
            spaceConnection.DefineStringPropertyDefaultValue(ContainerName, PropertyName, DefaultValue);
        }
    }
}