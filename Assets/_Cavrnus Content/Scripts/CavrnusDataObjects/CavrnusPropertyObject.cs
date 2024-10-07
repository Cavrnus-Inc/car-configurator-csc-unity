using System;
using System.Collections;
using CavrnusCore;
using CavrnusSdk.API;
using UnityEngine;

namespace CavrnusDemo.CavrnusDataObjects
{
    public enum ContainerNameEnum
    {
        Car, 
        Environment
    }
    
    public abstract class CavrnusPropertyObject<T> : ScriptableObject
    {
        public ContainerNameEnum ContainerNameEnum;
        
        public event Action<T> OnServerValueUpdated; 
        
        public string ContainerName => ContainerNameEnum.ToString();
        public string PropertyName;
        public T DefaultValue;

        protected CavrnusSpaceConnection SpaceConnection;
        protected IDisposable Binding;
        
        private void OnEnable()
        {
            CavrnusFunctionLibrary.AwaitAnySpaceConnection(sc => {
                SpaceConnection = sc;
                CavrnusStatics.Scheduler.ExecCoRoutine(WaitFrameRoutine());
                
                return;

                // Deferring binding by a frame to allow objects to subscribe
                // to OnServerValueUpdated first.
                IEnumerator WaitFrameRoutine()
                {
                    yield return null;
                    SetBinding();
                }
            });
        }
        
        public void Unbind() => Binding?.Dispose();
        
        public abstract void PostValue(T value);

        protected abstract void SetBinding();
        protected void SendUpdateEvent(T value) => OnServerValueUpdated?.Invoke(value);
    }
}