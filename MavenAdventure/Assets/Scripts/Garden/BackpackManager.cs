using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackpackManager : MonoBehaviour
{
    public GameObject backpackPanel;
    public Transform itemGrid;
    public GameObject itemPrefab;
    public InventoryData selectedSeed;
    public Button plantingButton;
    public SeedManager seedManager;
    private Dictionary<InventoryData, int> collectedItems = new Dictionary<InventoryData, int>();

    [Header("Starting Seeds")] 
    public List<InventoryData> startingSeeds;
    public int startingSeedAmount = 5;
    
    private void Start()
    {
        InitializeStartingSeeds();
        UpdateBackpackUI();
    }
    
    private void InitializeStartingSeeds()
    {
        foreach (InventoryData seed in startingSeeds)
        {
            AddToBackpack(seed, startingSeedAmount);
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
                InventoryData seedRef = item.Key;

                button.onClick.RemoveAllListeners();
                button.onClick.AddListener(delegate { SelectSeed(seedRef); });

                if (seedManager != null)
                {
                    button.onClick.AddListener(delegate { seedManager.ActivatePlantingMode(); });
                }
            }
            newItem.transform.localScale = Vector3.one;
        }
    }
    
    public void SelectSeed(InventoryData seed)
    {
        selectedSeed = seed;
        ToggleBackpack(false);
    }

    public InventoryData GetSelectedSeed()
    {
        return selectedSeed;
    }

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