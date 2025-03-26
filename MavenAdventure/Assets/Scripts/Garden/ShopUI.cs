using UnityEngine;
using UnityEngine.UI;

public class ShopUI : MonoBehaviour
{
    public ShopManager shopManager;
    public BackpackManager backpackManager;
    public Transform shopItemsParent;
    public GameObject shopItemPrefab;
    public Text goldText;
    private void Start()
    {
        UpdateShopUI();
    }

    public void UpdateShopUI()
    {
        Debug.Log("Updating Shop UI");

        goldText.text = $"Gold: {shopManager.playerGold.value}"; 

        // Clear existing shop items before updating
        foreach (Transform child in shopItemsParent)
        {
            Destroy(child.gameObject);
        }

        // Populate shop with seeds
        foreach (InventoryData seed in shopManager.seedsForSale)
        {
            CreateShopItem(seed, isProduce: false);
        }

        // Populate shop with produce
        foreach (ProduceData produce in shopManager.GetSellableProduce())
        {
            CreateShopItem(produce, isProduce: true);
        }
    }
    
    private void CreateShopItem(object item, bool isProduce)
    {
        GameObject shopItem = Instantiate(shopItemPrefab, shopItemsParent);

        string itemName = isProduce ? ((ProduceData)item).displayName : ((InventoryData)item).displayName;
        int itemPrice = isProduce ? ((ProduceData)item).goldValue : ((InventoryData)item).goldValue;
        Sprite itemIcon = isProduce ? ((ProduceData)item).produceIcon : ((InventoryData)item).Seedicon;

        int itemAmount = isProduce 
            ? (backpackManager.collectedProduce.ContainsKey((ProduceData)item) ? backpackManager.collectedProduce[(ProduceData)item] : 0) 
            : (backpackManager.collectedSeeds.ContainsKey((InventoryData)item) ? backpackManager.collectedSeeds[(InventoryData)item] : 0);

        shopItem.transform.Find("ItemName").GetComponent<Text>().text = $"{itemName} x{itemAmount}";
        shopItem.transform.Find("ItemPrice").GetComponent<Text>().text = itemPrice.ToString();
        shopItem.transform.Find("ItemIcon").GetComponent<Image>().sprite = itemIcon;

        Button buyButton = shopItem.transform.Find("BuyButton").GetComponent<Button>();
        buyButton.gameObject.SetActive(!isProduce); // Hide buy button for produce

        Button sellButton = shopItem.transform.Find("SellButton").GetComponent<Button>();
        sellButton.onClick.RemoveAllListeners();

        if (isProduce)
        {
            sellButton.onClick.AddListener(() => shopManager.SellProduce((ProduceData)item));
        }
        else
        {
            sellButton.onClick.AddListener(() => shopManager.SellSeed((InventoryData)item));

            // 🛠 FIX: Add listener for buying seeds
            buyButton.onClick.RemoveAllListeners(); // Clear old listeners to prevent duplicates
            buyButton.onClick.AddListener(() =>
            {
                Debug.Log("Buy button clicked for: " + ((InventoryData)item).displayName); // Debug
                shopManager.BuyItem((InventoryData)item);
            });
        }
    }
    private void BuyItemWithUIUpdate(InventoryData item)
    {
        if (shopManager.playerGold.value >= item.goldValue) // Ensure player has enough gold
        {
            shopManager.BuyItem(item); // Proceed with the purchase (handle gold deduction)
        
            // Add the item to the player's backpack
            backpackManager.AddSeedToBackpack(item, 1); // Adding the bought seed to the backpack with quantity 1
        
            UpdateShopUI(); // Ensure the shop UI is updated after the purchase
            backpackManager.UpdateBackpackUI(); // Update the backpack UI to reflect new items
        }
        else
        {
            Debug.Log("Not enough gold to buy this item.");
        }
    }
}