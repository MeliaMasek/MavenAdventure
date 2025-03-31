using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackpackManager : MonoBehaviour
{
    public GameObject backpackPanel;
    public Transform itemGrid;
    public GameObject itemPrefab;
    public InventoryData selectedSeed;
    public SeedManager seedManager;
    public ShopUI shopUI;

    public Dictionary<InventoryData, int> collectedSeeds = new Dictionary<InventoryData, int>();
    public Dictionary<ProduceData, int> collectedProduce = new Dictionary<ProduceData, int>();
    private Dictionary<InventoryData, int> collectedItems = new Dictionary<InventoryData, int>();
    private List<ProduceData> inventoryproduceList = new List<ProduceData>();
    private List<InventoryData> inventoryseedList = new List<InventoryData>();

    [Header("Starting Seeds")] 
    public List<InventoryData> startingSeeds;
    public int startingSeedAmount = 5;
    
    [Header("Starting Produce")] 
    public List<ProduceData> startingProduce;
    public int startingProduceAmount = 5;
    
    private void Start()
    {
        InitializeStartingSeeds();
        InitializeStartingProduce();
        UpdateBackpackUI();
        shopUI.UpdateShopUI();
    }
    
    private void InitializeStartingSeeds()
    {
        foreach (InventoryData seed in startingSeeds)
        {
            AddSeedToBackpack(seed, startingSeedAmount);
        }
    }
    private void InitializeStartingProduce()
    {
        foreach (ProduceData produce in startingProduce)
        {
            if (!collectedProduce.ContainsKey(produce))
            {
                collectedProduce[produce] = startingProduceAmount;
            }
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
        foreach (Transform child in itemGrid)
        {
            Destroy(child.gameObject);
        }

        foreach (var item in collectedSeeds)
        {
            CreateBackpackItem(item.Key.Seedicon, item.Key.displayName, item.Value, () => SelectSeed(item.Key));
        }

        foreach (var item in collectedProduce)
        {
            CreateBackpackItem(item.Key.produceIcon, item.Key.displayName, item.Value, null);
        }

        // Force layout update for correct positioning in scroll view
        LayoutRebuilder.ForceRebuildLayoutImmediate(itemGrid.GetComponent<RectTransform>());
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
            seedManager.ActivatePlantingMode();
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
    
    public bool RemoveItem(InventoryData item, int amount = 1)
    {
        if (collectedSeeds.ContainsKey(item))
        {
            collectedSeeds[item] -= amount;

            // If the amount is reduced to zero or less, remove the item
            if (collectedSeeds[item] <= 0)
            {
                collectedSeeds.Remove(item);
            }

            UpdateBackpackUI();
            return true;
        }

        // If the item doesn't exist in the backpack
        return false;
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
            return;
        }

        if (collectedProduce.ContainsKey(produce))
        {
            collectedProduce[produce] += amount; // Increment the count
        }
        else
        {
            collectedProduce[produce] = amount; // Initialize the count
        }

        UpdateBackpackUI(); // Update the UI to reflect changes
    }
    
    public void AddItem(object item)
    {
        if (item == null)
        {
            return;
        }

        if (item is InventoryData inventoryItem)
        {
            InventoryData existingItem = inventoryseedList.Find(i => i.displayName == inventoryItem.displayName);
            if (existingItem != null)
            {
                existingItem.maxStackSize++; // Increase the stack size
            }
            else
            {
                inventoryItem.maxStackSize = 1; // Initialize stack size if new
                inventoryseedList.Add(inventoryItem);
            }
        }
        else if (item is ProduceData produceItem)
        {
            ProduceData existingProduce = inventoryproduceList.Find(p => p.displayName == produceItem.displayName);
            if (existingProduce != null)
            {
                existingProduce.maxStackSize++; // Increase stack size
            }
            else
            {
                produceItem.maxStackSize = 1;
                inventoryproduceList.Add(produceItem);
            }
        }
        else
        {
            Debug.LogWarning("Invalid item type.");
        }

        UpdateBackpackUI(); // Ensure the UI updates after adding an item
    }
}