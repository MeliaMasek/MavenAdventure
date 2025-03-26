using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public IntData playerGold; // Reference to the player's gold
    public BackpackManager backpackManager; // Reference to the player's inventory
    public List<InventoryData> seedsForSale; // Make sure this is defined in ShopManager
    public ShopUI shopUI;
    
    public void BuyItem(InventoryData item)
    {
        if (playerGold.value >= item.goldValue) // Check if player has enough gold
        {
            playerGold.UpdateValue(-item.goldValue); // Deduct gold
            backpackManager.AddSeedToBackpack(item, 1); // Add to inventory

            // 🔄 Make sure UI updates
            shopUI.UpdateShopUI();
            backpackManager.UpdateBackpackUI();
        }
        else
        {
            Debug.Log("Not enough gold!");
        }
    }
    
    public void SellSeed(InventoryData seeds)
    {
        if (backpackManager.RemoveItem(seeds, 1)) // Attempt to remove one item from the backpack
        {
            playerGold.UpdateValue(seeds.goldValue); // Add gold for selling the item

            // Update the shop and backpack UI
            shopUI.UpdateShopUI();
            backpackManager.UpdateBackpackUI();
        }
        else
        {
            Debug.Log("You don't have this item to sell!");
        }
    }
    
    public void SellProduce(ProduceData produce)
    {
        if (backpackManager.collectedProduce.ContainsKey(produce) && backpackManager.collectedProduce[produce] > 0)
        {
            playerGold.UpdateValue(produce.goldValue); // Add gold for selling
            backpackManager.RemoveProduce(produce); // Remove produce from backpack

            shopUI.UpdateShopUI(); // Refresh shop UI
            backpackManager.UpdateBackpackUI(); // Refresh backpack UI
        }
        else
        {
            Debug.Log("You don't have this produce to sell!");
        }
    }
    
    public List<ProduceData> GetSellableProduce()
    {
        List<ProduceData> sellableProduce = new List<ProduceData>();

        foreach (var produce in backpackManager.collectedProduce)
        {
            if (!sellableProduce.Contains(produce.Key)) 
            {
                sellableProduce.Add(produce.Key);
            }
        }

        return sellableProduce;
    }
}