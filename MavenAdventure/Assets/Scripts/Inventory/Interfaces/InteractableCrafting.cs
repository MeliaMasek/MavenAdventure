using UnityEngine;
using UnityEngine.Events;

public class InteractableCrafting : MonoBehaviour, IInteractable
{
    public UnityAction<IInteractable> OnInteractionComplete { get; set; }

    public void Interact(Interactor interactor, out bool interactSuccessful)
    {
        // Add behavior for interacting with InteractableType1 here
        Debug.Log("Interacting with InteractableType1");
        interactSuccessful = true;
    }

    public void EndInteraction()
    {
        throw new System.NotImplementedException();
    }
}