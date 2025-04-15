using UnityEngine;
//Code created, borrowed, and modified from Chat GPT

public class InteractionPoint : MonoBehaviour
{
    public float interactionDistance = 2f;
    public float angleThreshold = 45f;  // The angle within which the player must face the object
    public GameObject interactionMenu;

    private Transform player;
    private bool isPlayerNear = false;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;  // Make sure your player is tagged "Player"
        if (interactionMenu != null)
            interactionMenu.SetActive(false);
    }

    private void Update()
    {
        if (player == null) return;

        Vector3 directionToPlayer = player.position - transform.position;
        float distance = directionToPlayer.magnitude;

        if (distance <= interactionDistance)
        {
            Vector3 forward = transform.forward;
            directionToPlayer.y = 0;  // Flatten for horizontal check
            forward.y = 0;

            float angle = Vector3.Angle(forward, directionToPlayer);

            if (angle <= angleThreshold)
            {
                if (!isPlayerNear)
                {
                    isPlayerNear = true;
                    ShowMenu();
                }
            }
            else
            {
                if (isPlayerNear)
                {
                    isPlayerNear = false;
                    HideMenu();
                }
            }
        }
        else
        {
            if (isPlayerNear)
            {
                isPlayerNear = false;
                HideMenu();
            }
        }
    }

    private void ShowMenu()
    {
        if (interactionMenu != null)
            interactionMenu.SetActive(true);
    }

    private void HideMenu()
    {
        if (interactionMenu != null)
            interactionMenu.SetActive(false);
    }
}