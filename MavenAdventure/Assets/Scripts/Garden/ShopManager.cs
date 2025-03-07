using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public IntData playerGold; // Reference to the player's gold
    public BackpackManager backpackManager; // Reference to the player's inventory
    public List<InventoryData> seedsForSale; // Make sure this is defined in ShopManager

    public void BuyItem(InventoryData item)
    {
        if (playerGold.value >= item.goldValue) // Check if the player has enough gold
        {
            playerGold.UpdateValue(-item.goldValue); // Deduct gold
            backpackManager.AddItem(item); // Add the item to the backpack
            Debug.Log($"Bought {item.displayName} for {item.goldValue} gold.");
        }
        else
        {
            Debug.Log("Not enough gold!");
        }
    }

    public void SellItem(InventoryData item)
    {
        if (backpackManager.RemoveItem(item)) // Check if the player has the item
        {
            playerGold.UpdateValue(item.goldValue); // Add gold for selling the item
            Debug.Log($"Sold {item.displayName} for {item.goldValue} gold.");
        }
        else
        {
            Debug.Log("You don't have this item to sell!");
        }
    }
}