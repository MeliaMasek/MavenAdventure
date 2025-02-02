using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackpackManager : MonoBehaviour
{
    public GameObject backpackPanel;  // Reference to UI Panel
    public Transform itemGrid;        // Parent for item list (GridLayoutGroup)
    public GameObject itemPrefab;     // Prefab for displaying items

    private Dictionary<string, int> collectedPlants = new Dictionary<string, int>();

    // Add plant to the backpack
    public void AddToBackpack(string plantName)
    {
        if (collectedPlants.ContainsKey(plantName))
        {
            collectedPlants[plantName]++;
        }
        else
        {
            collectedPlants[plantName] = 1;
        }

        UpdateBackpackUI();
    }

    // Update the UI by clearing the old items and displaying the updated ones
    public void UpdateBackpackUI()
    {
        // Clear existing UI elements
        foreach (Transform child in itemGrid)
        {
            Destroy(child.gameObject);
        }

        // Add new items based on collected plants
        foreach (var plant in collectedPlants)
        {
            // Instantiate the new item prefab
            GameObject newItem = Instantiate(itemPrefab, itemGrid);

            // Set the text of the new item (make sure itemPrefab has a Text component)
            Text itemText = newItem.GetComponentInChildren<Text>();
            if (itemText != null)
            {
                itemText.text = $"{plant.Key} x{plant.Value}";
            }
            else
            {
                Debug.LogWarning("No Text component found in itemPrefab.");
            }

            // Ensure the item is properly scaled
            newItem.transform.localScale = Vector3.one;

            Debug.Log($"Added {plant.Key} x{plant.Value} to backpack.");
        }
    }

    // Toggle backpack visibility
    public void ToggleBackpack(bool show)
    {
        backpackPanel.SetActive(show);
    }
}