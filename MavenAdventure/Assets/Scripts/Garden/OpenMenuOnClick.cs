using System;
using UnityEngine;

public class OpenMenuOnClick : MonoBehaviour
{
    public GameObject menuPanel; // Assign the UI menu panel in the Inspector
    private Collider doorCollider;
    public AudioSource audioSource;
    public AudioClip doorOpenSound;

    public float interactionDistance = 2f;
    private bool isPlayerInRange = false;

    public Transform player;

    private void Start()
    {
        doorCollider = GetComponent<Collider>();

        if (doorCollider == null)
        {
            Debug.LogError("No collider found on this object! Add a collider.");
        }

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    private void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);
        isPlayerInRange = distance <= interactionDistance;

        if (doorCollider != null)
        {
            doorCollider.enabled = isPlayerInRange;
        }
    }

    private bool IsPlayerEligible()
    {
        if (player == null)
        {
            Debug.LogError("Player reference not assigned!");
            return false;
        }

        float distance = Vector3.Distance(transform.position, player.position);
        Debug.Log("Distance to player: " + distance);

        return distance <= interactionDistance;
    }

    private void OnMouseDown()
    {
        if (player == null)
        {
            Debug.LogError("Player reference not assigned!");
            return;
        }

        float distance = Vector3.Distance(transform.position, player.position);
        if (distance > interactionDistance)
        {
            Debug.Log("Player too far away to interact!");
            return;
        }

        if (menuPanel != null)
        {
            if (menuPanel.activeSelf)
                return;

            if (doorOpenSound != null)
            {
                audioSource.volume = 0.25f;
                audioSource.PlayOneShot(doorOpenSound);
            }

            menuPanel.SetActive(true);
            Debug.Log("Menu opened!");
        }
    }
}