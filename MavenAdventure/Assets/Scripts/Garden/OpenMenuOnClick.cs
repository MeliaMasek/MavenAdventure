using UnityEngine;

public class OpenMenuOnClick : MonoBehaviour
{
    public GameObject menuPanel; // Assign the UI menu panel in the Inspector

    private void OnMouseDown()
    {
        if (menuPanel != null)
        {
            menuPanel.SetActive(!menuPanel.activeSelf); // Toggle menu on click
        }
    }
}