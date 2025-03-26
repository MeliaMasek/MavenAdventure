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
        goldText.text = $"Gold: {shopManager.playerGold.value}"; // Display gold correctly

        // Clear existing shop items before updating
        foreach (Transform child in shopItemsParent)
        {
            Destroy(child.gameObject);
        }

        // Populate the shop with available seeds
        foreach (InventoryData seed in shopManager.seedsForSale)
        {
            CreateShopItem(seed);
        }
    }
    
    private void CreateShopItem(InventoryData item)
    {
        GameObject shopItem = Instantiate(shopItemPrefab, shopItemsParent);
        shopItem.transform.Find("ItemName").GetComponent<Text>().text = item.displayName;
        shopItem.transform.Find("ItemPrice").GetComponent<Text>().text = item.goldValue.ToString();
        shopItem.transform.Find("ItemIcon").GetComponent<Image>().sprite = item.Seedicon;

        Button buyButton = shopItem.transform.Find("BuyButton").GetComponent<Button>();
        buyButton.onClick.AddListener(() => BuyItemWithUIUpdate(item));
        
        Button sellButton = shopItem.transform.Find("SellButton").GetComponent<Button>();
        sellButton.onClick.AddListener(() => shopManager.SellSeed(item));
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