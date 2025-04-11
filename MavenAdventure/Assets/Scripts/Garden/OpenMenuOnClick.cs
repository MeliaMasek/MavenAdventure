using UnityEngine;

public class OpenMenuOnClick : MonoBehaviour
{
    public GameObject menuPanel; // Assign the UI menu panel in the Inspector
    private Collider doorCollider; // Reference to the door's collider
    public AudioSource audioSource; // Sound to play when the door is clicked 
    public AudioClip doorOpenSound; // Clip to play when the door is clicked

    private void Start()
    {
        doorCollider = GetComponent<Collider>();

        // Try to get AudioSource from this GameObject
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            // If there's no AudioSource, add one
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    private void OnMouseDown()
    {
        if (menuPanel != null)
        {
            if (menuPanel.activeSelf)
                return;

            // Play the sound
            if (doorOpenSound != null)
            {
                audioSource.volume = 0.25f; // Range is 0.0 (mute) to 1.0 (full volume)
                audioSource.PlayOneShot(doorOpenSound);
            }

            // Toggle the menu panel
            menuPanel.SetActive(true);
        }
    }

    // Optional: Handle trigger areas if you want to allow interaction only when nearby
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !menuPanel.activeSelf)
        {
            // You could set a flag here to allow interaction
        }
    }
}