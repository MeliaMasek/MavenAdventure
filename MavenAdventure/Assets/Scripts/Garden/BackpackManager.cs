

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
    
    private Dictionary<InventoryData, int> collectedSeeds = new Dictionary<InventoryData, int>();
    private Dictionary<ProduceData, int> collectedProduce = new Dictionary<ProduceData, int>();

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
            AddSeedToBackpack(seed, startingSeedAmount);
        }
    }

    public void AddSeedToBackpack(InventoryData seed, int amount = 1)
    {
        if (seed == null) return;

        if (collectedSeeds.ContainsKey(seed))
        {
            collectedSeeds[seed] += amount;
        }
        else
        {
            collectedSeeds[seed] = amount;
        }

        UpdateBackpackUI();
    }

    public void UpdateBackpackUI()
    {
        // Clear UI first
        foreach (Transform child in itemGrid)
        {
            Destroy(child.gameObject);
        }

        // Display seeds
        foreach (var item in collectedSeeds)
        {
            CreateBackpackItem(item.Key.Seedicon, item.Key.displayName, item.Value, () => SelectSeed(item.Key));
        }

        // Display produce
        foreach (var item in collectedProduce)
        {
            CreateBackpackItem(item.Key.ProduceIcon, item.Key.displayName, item.Value, null);
        }
    }

    private void CreateBackpackItem(Sprite icon, string name, int amount, System.Action onClick)
    {
        GameObject newItem = Instantiate(itemPrefab, itemGrid);
        Image itemImage = newItem.GetComponentInChildren<Image>();

        if (itemImage != null)
        {
            itemImage.sprite = icon;
        }

        Text itemText = newItem.GetComponentInChildren<Text>();
        if (itemText != null)
        {
            itemText.text = $"{name} x{amount}";
        }

        Button button = newItem.GetComponent<Button>();
        if (button != null && onClick != null)
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() => onClick.Invoke());
        }

        newItem.transform.localScale = Vector3.one;
    }

    public void SelectSeed(InventoryData seed)
    {
        selectedSeed = seed;
        ToggleBackpack(false);

        if (seedManager != null)
        {
            seedManager.ActivatePlantingMode(); // Activate planting on selection
        }
    }
    
    public InventoryData GetSelectedSeed()
    {
        return selectedSeed;
    }
    
    public void ToggleBackpack(bool show)
    {
        backpackPanel.SetActive(show);
    }

    public void RemoveSeed(InventoryData seed)
    {
        if (collectedSeeds.ContainsKey(seed))
        {
            collectedSeeds[seed]--;

            if (collectedSeeds[seed] <= 0)
            {
                collectedSeeds.Remove(seed);
            }
            UpdateBackpackUI();
        }
    }
    
    public void RemoveProduce(ProduceData produce)
    {
        if (collectedProduce.ContainsKey(produce))
        {
            collectedProduce[produce]--;

            if (collectedProduce[produce] <= 0)
            {
                collectedProduce.Remove(produce);
            }
            UpdateBackpackUI();
        }
    }
    
    public void RemoveItem(InventoryData seed)
    {
        if (collectedSeeds.ContainsKey(seed))
        {
            collectedSeeds[seed]--;

            if (collectedSeeds[seed] <= 0)
            {
                collectedSeeds.Remove(seed);
            }
            UpdateBackpackUI();
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
    
    public void AddProduceToBackpack(ProduceData produce, int amount = 1)
    {
        if (produce == null)
        {
            Debug.LogError("Attempted to add null produce to backpack!");
            return;
        }

        if (collectedProduce.ContainsKey(produce))
        {
            collectedProduce[produce] += amount;
        }
        else
        {
            collectedProduce[produce] = amount;
        }

        Debug.Log($"Added {produce.displayName} to backpack. New count: {collectedProduce[produce]}");
        UpdateBackpackUI();
    }

}
