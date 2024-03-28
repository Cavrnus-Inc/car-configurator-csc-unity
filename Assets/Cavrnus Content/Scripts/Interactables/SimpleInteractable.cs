using UnityEngine;
using UnityEngine.Events;

namespace CavrnusDemo
{
    public class SimpleInteractable : MonoBehaviour, ICustomInteractable
    {
        [SerializeField]
        private UnityEvent onInteract;

        public UnityEvent OnInteract
        {
            get => onInteract;
            set => onInteract = value;
        }

        public void SetActiveState(bool state)
        {
            
        }

        public void SetVisibility(bool state)
        {
            
        }

        public void Interact()
        {
            OnInteract?.Invoke();
        }
    }
}