

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackpackManager : MonoBehaviour
{
    public GameObject backpackPanel;  // Reference to UI Panel
    public Transform itemGrid;        // Parent for item list (GridLayoutGroup)
    public GameObject itemPrefab;     // Prefab for displaying items
    public InventoryData selectedSeed;
    public Button plantingButton;  // Assign this in the Inspector
    public SeedManager seedManager;
    private Dictionary<InventoryData, int> collectedItems = new Dictionary<InventoryData, int>();

    [Header("Starting Seeds")] 
    public List<InventoryData> startingSeeds;  // List of seeds the player starts with
    public int startingSeedAmount = 5;  // Number of each seed to start with

    // Add an item to the backpack
    
    private void Start()
    {
        InitializeStartingSeeds(); // Give default seeds at game start
        UpdateBackpackUI();
    }
    
    private void InitializeStartingSeeds()
    {
        foreach (InventoryData seed in startingSeeds)
        {
            AddToBackpack(seed, startingSeedAmount); // Add 5 of each seed type
        }
    }

    public void AddToBackpack(InventoryData itemData, int amount = 1)
    {
        if (itemData == null) return;

        if (collectedItems.ContainsKey(itemData))
        {
            collectedItems[itemData] += amount;
        }
        else
        {
            collectedItems[itemData] = amount;
        }

        UpdateBackpackUI();
    }

    // Update the UI by clearing the old items and displaying the updated ones
    public void UpdateBackpackUI()
    {
        foreach (Transform child in itemGrid)
        {
            Destroy(child.gameObject);
        }

        foreach (var item in collectedItems)
        {
            GameObject newItem = Instantiate(itemPrefab, itemGrid);
            Image itemImage = newItem.GetComponentInChildren<Image>();

            if (itemImage != null)
            {
                itemImage.sprite = item.Key.icon;
            }

            Text itemText = newItem.GetComponentInChildren<Text>();
            if (itemText != null)
            {
                itemText.text = $"{item.Key.displayName} x{item.Value}";
            }

            Button button = newItem.GetComponent<Button>(); 
            if (button != null)
            {
                Debug.Log($"Assigning button event for {item.Key.displayName}"); // Debugging

                InventoryData seedRef = item.Key;

                button.onClick.RemoveAllListeners();
                button.onClick.AddListener(delegate { SelectSeed(seedRef); });

                if (seedManager != null)
                {
                    button.onClick.AddListener(delegate { seedManager.ActivatePlantingMode(); });
                    Debug.Log("Planting mode assigned to button click.");
                }
                else
                {
                    Debug.LogError("SeedManager not found! Make sure it's assigned in the Inspector.");
                }
            }
            else
            {
                Debug.LogError("No Button component found on itemPrefab!");
            }

            newItem.transform.localScale = Vector3.one;
        }
    }

    
    public void SelectSeed(InventoryData seed)
    {
        selectedSeed = seed;
        Debug.Log("Selected Seed: " + seed.displayName);

        // Close the backpack menu after selecting a seed
        ToggleBackpack(false);  // Close backpack UI
    }

    public InventoryData GetSelectedSeed()
    {
        return selectedSeed;
    }
    // Toggle backpack visibility

    public void ToggleBackpack(bool show)
    {
        backpackPanel.SetActive(show);
    }
    
    public void RemoveItem(InventoryData itemData)
    {
        if (collectedItems.ContainsKey(itemData))
        {
            collectedItems[itemData]--;

            if (collectedItems[itemData] <= 0)
            {
                collectedItems.Remove(itemData);
            }

            UpdateBackpackUI();
        }
    }

}
