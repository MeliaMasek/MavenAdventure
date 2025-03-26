using UnityEngine;

public class OpenMenuOnClick : MonoBehaviour
{
    public GameObject menuPanel; // Assign the UI menu panel in the Inspector
    private Collider doorCollider; // Reference to the door's collider


    private void Start()
    {
        // Ensure we have the door's collider reference
        doorCollider = GetComponent<Collider>();
    }

    private void OnMouseDown()
    {
        if (menuPanel != null)
        {
            // If the menu is open, do nothing when clicking the door
            if (menuPanel.activeSelf)
            {
                return; // Prevent interaction with door when menu is open
            }

            // Otherwise, toggle the menu when clicked
            menuPanel.SetActive(!menuPanel.activeSelf);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Only allow opening the menu when the player is near the door and the menu is not open
        if (other.CompareTag("Player") && !menuPanel.activeSelf)
        {
            // Allow interaction with the door
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Handle when the player exits the door's trigger area
    }
}