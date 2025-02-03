using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackpackManager : MonoBehaviour
{
    public GameObject backpackPanel;  // Reference to UI Panel
    public Transform itemGrid;        // Parent for item list (GridLayoutGroup)
    public GameObject itemPrefab;     // Prefab for displaying items

    private Dictionary<InventoryData, int> collectedItems = new Dictionary<InventoryData, int>();

    // Add an item to the backpack
    public void AddToBackpack(InventoryData itemData)
    {
        if (itemData == null) return;

        if (collectedItems.ContainsKey(itemData))
        {
            collectedItems[itemData]++;
        }
        else
        {
            collectedItems[itemData] = 1;
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

        // Add new items based on collected items
        foreach (var item in collectedItems)
        {
            GameObject newItem = Instantiate(itemPrefab, itemGrid);

            // Assign the item icon
            Image itemImage = newItem.GetComponentInChildren<Image>();
            if (itemImage != null)
            {
                itemImage.sprite = item.Key.icon;
            }
            else
            {
                Debug.LogWarning("No Image component found in itemPrefab.");
            }

            // Assign the item count (if a text component is available)
            Text itemText = newItem.GetComponentInChildren<Text>();
            if (itemText != null)
            {
                itemText.text = $"x{item.Value}";
            }

            newItem.transform.localScale = Vector3.one;
        }
    }

    // Toggle backpack visibility
    public void ToggleBackpack(bool show)
    {
        backpackPanel.SetActive(show);
    }
}
