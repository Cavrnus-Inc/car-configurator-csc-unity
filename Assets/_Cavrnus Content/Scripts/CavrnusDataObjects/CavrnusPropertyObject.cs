using CavrnusSdk.API;
using UnityEngine;

namespace CavrnusDemo.CavrnusDataObjects
{
    public enum CarProjectContainerNameEnum
    {
        Car
    }
    
    public abstract class CavrnusPropertyObject<T> : ScriptableObject
    {
        public CarProjectContainerNameEnum ContainerNameEnum;
        public string ContainerName => ContainerNameEnum.ToString();
        public string PropertyName;
        public T DefaultValue;
        
        private void OnEnable()
        {
            CavrnusFunctionLibrary.AwaitAnySpaceConnection(OnSpaceConnected);
        }

        protected abstract void OnSpaceConnected(CavrnusSpaceConnection spaceConnection);
    }
}