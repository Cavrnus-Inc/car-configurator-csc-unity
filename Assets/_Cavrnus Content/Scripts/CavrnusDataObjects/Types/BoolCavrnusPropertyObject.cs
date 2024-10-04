using CavrnusSdk.API;
using UnityEngine;

namespace CavrnusDemo.CavrnusDataObjects
{
    [CreateAssetMenu(fileName = "CavrnusBooleanProperty", menuName = "Cavrnus/CarData/BooleanData", order = 0)]
    public class BoolCavrnusPropertyObject : CavrnusPropertyObject<bool>
    {
        protected override void OnSpaceConnected(CavrnusSpaceConnection spaceConnection)
        {
            spaceConnection.DefineBoolPropertyDefaultValue(ContainerName, PropertyName, DefaultValue);
        }
    }
}