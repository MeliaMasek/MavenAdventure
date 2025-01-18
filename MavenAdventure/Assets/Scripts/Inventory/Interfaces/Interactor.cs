using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Interactor : MonoBehaviour
{
    public Transform interactionPoint;
    public LayerMask interactionLayer;
    public float interactionPointRadius = 1f;
    public InputActionReference playerInput;
    public GameObject interactionButton; // Reference to the button object
    public Button clickInteractionButton; // Reference to the click interaction button
    private bool isInteracting;

    private void Start()
    {
        // Add a listener to the click interaction button
        clickInteractionButton.onClick.AddListener(OnClickInteraction);
    }

    private void OnDestroy()
    {
        // Remove the listener when the object is destroyed to prevent memory leaks
        clickInteractionButton.onClick.RemoveListener(OnClickInteraction);
    }

    private void OnClickInteraction()
    {
        var colliders = Physics.OverlapSphere(interactionPoint.position, interactionPointRadius, interactionLayer);

        // Check if any interactable objects are within range
        bool isInRange = colliders.Length > 0;

        if (isInRange)
        {
            for (int i = 0; i < colliders.Length; i++)
            {
                var interactable = colliders[i].GetComponent<IInteractable>();

                if (interactable != null) StartInteraction(interactable);
            }
        }
    }

    private void Update()
    {
        var colliders = Physics.OverlapSphere(interactionPoint.position, interactionPointRadius, interactionLayer);

        // Check if any interactable objects are within range
        bool isInRange = colliders.Length > 0;

        // Set the visibility of the interaction button based on whether the player is in range
        interactionButton.SetActive(isInRange);

        // Optionally, you can still keep the keyboard input as an alternative way to trigger interaction
        if (Keyboard.current.aKey.wasPressedThisFrame && isInRange)
        {
            for (int i = 0; i < colliders.Length; i++)
            {
                var interactable = colliders[i].GetComponent<IInteractable>();

                if (interactable != null) StartInteraction(interactable);
            }
        }
    }

    private void StartInteraction(IInteractable interactable)
    {
        interactable.Interact(this, out bool interactSuccessful);
        isInteracting = true;
    }

    private void EndInteraction(IInteractable interactable)
    {
        isInteracting = false;
    }
}