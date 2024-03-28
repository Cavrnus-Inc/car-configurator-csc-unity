using UnityEngine.Events;

namespace CavrnusDemo
{
    public interface ICustomInteractable
    {
        UnityEvent OnInteract { get; set; }

        void SetActiveState(bool state);
        void SetVisibility(bool state);
        void Interact();
    }
}