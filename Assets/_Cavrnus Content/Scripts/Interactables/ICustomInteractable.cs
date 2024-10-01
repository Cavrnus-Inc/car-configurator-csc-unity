using UnityEngine.Events;

namespace CavrnusDemo.Interactables
{
    public interface ICustomInteractable
    {
        UnityEvent OnInteract { get; set; }
        void Interact();
    }
}