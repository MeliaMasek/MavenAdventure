using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//Code borrowed and Modified by Dan Pos off of the inventory system series from youtube https://www.youtube.com/playlist?list=PL-hj540P5Q1hLK7NS5fTSNYoNJpPWSL24

[System.Serializable]
public class ShopSystem
{
    [SerializeField] private List<ShopSlots> shopInventory;
    [SerializeField] private int availableGold;
    [SerializeField] private float buyMarkUp;
    [SerializeField] private float sellMarkUp;

    public List<ShopSlots> ShopInventory => shopInventory;  
    public int AvailableGold => availableGold;
    public float BuyMarkUp => buyMarkUp;
    public float SellMarkUp => sellMarkUp;
    public ShopSystem(int size, int gold, float _buyMarkUp, float _sellMarkUp)
    {
        availableGold = gold;
        buyMarkUp = _buyMarkUp;
        sellMarkUp = _sellMarkUp;
        
        SetShopSize(size);
    }

    private void SetShopSize(int size)
    {
        shopInventory = new List<ShopSlots>(size);

        for (int i = 0; i < size; i++)
        {
            shopInventory.Add(new ShopSlots());
        }
    }

    public void AddToShop(InventoryData data, int amount)
    {
        if (ContainsItem(data, out ShopSlots shopSlot))
        {
            shopSlot.AddToStack(amount);
            return;
        }
        var freeSlot = GetFreeSlot();
        freeSlot.AssignItem(data, amount);
    }

    private ShopSlots GetFreeSlot()
    {
        var freeSlot = shopInventory.FirstOrDefault(i => i.ItemData == null);

        if (freeSlot == null)
        {
            freeSlot = new ShopSlots();
            shopInventory.Add(freeSlot);
        }
        return freeSlot;
    }
    
    public bool ContainsItem(InventoryData itemToAdd, out ShopSlots shopSlot)
    {
        shopSlot = shopInventory.Find(i => i.ItemData == itemToAdd);
        return shopSlot != null;
    }

    public void PurchaseItem(InventoryData data, int amount)
    {
        if (!ContainsItem(data, out ShopSlots slot)) return;
        slot.RemoveFromStack(amount);
    }
    
    public void GainGold(int basketTotal)
    {
        availableGold += basketTotal;
    }

    public void ReduceGold(int basketTotal)
    {
        availableGold -= basketTotal;
    }
    
    public void SellItem(InventoryData kvpKey, int kvpValue, int price)
    {
        AddToShop(kvpKey, kvpValue);
        ReduceGold(price);
    }
}
