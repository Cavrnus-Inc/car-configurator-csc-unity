using UnityEngine;
using UnityEngine.Events;

namespace CavrnusDemo.Interactables
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

        public void Interact()
        {
            OnInteract?.Invoke();
        }
    }
}