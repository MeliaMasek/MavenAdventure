using UnityEngine;
using UnityEngine.Events;
//Code created, borrowed, and modified from Chat GPT

public class InteractableShop : MonoBehaviour, IInteractable
{
    public UnityAction<IInteractable> OnInteractionComplete { get; set; }

    public void Interact(Interactor interactor, out bool interactSuccessful)
    {
        // Add behavior for interacting with InteractableType2 here
        Debug.Log("Interacting with InteractableType2");
        interactSuccessful = true;
    }

    public void EndInteraction()
    {
        throw new System.NotImplementedException();
    }
}