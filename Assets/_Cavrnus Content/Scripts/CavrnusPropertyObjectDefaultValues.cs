using System.Collections.Generic;
using CavrnusDemo.CavrnusDataObjects;
using CavrnusSdk.API;
using UnityEngine;

namespace CavrnusDemo
{
    public class CavrnusPropertyObjectDefaultValues : MonoBehaviour
    {
        [SerializeField] private List<CavrnusPropertyObject<string>> stringProperties;
        [SerializeField] private List<CavrnusPropertyObject<float>> floatProperties;
        [SerializeField] private List<CavrnusPropertyObject<bool>> boolProperties;

        private void Start()
        {
            CavrnusFunctionLibrary.AwaitAnySpaceConnection(sc => {
                foreach (var obj in stringProperties)
                    sc.DefineStringPropertyDefaultValue(obj.ContainerName, obj.PropertyName, obj.DefaultValue);
                
                foreach (var obj in floatProperties)
                    sc.DefineFloatPropertyDefaultValue(obj.ContainerName, obj.PropertyName, obj.DefaultValue);
                
                foreach (var obj in boolProperties)
                    sc.DefineBoolPropertyDefaultValue(obj.ContainerName, obj.PropertyName, obj.DefaultValue);
            });
        }
    }
}