using System;
using UnityEngine;

//Code borrowed and Modified by Dan Pos off of the inventory system series from youtube https://www.youtube.com/playlist?list=PL-hj540P5Q1hLK7NS5fTSNYoNJpPWSL24

public abstract class ItemSlot : ISerializationCallbackReceiver
{
    [NonSerialized] protected InventoryData itemData;
    [SerializeField] protected int itemID = -1;
    [SerializeField] protected int stacksize;
    public InventoryData ItemData => itemData;
    public int StackSize => stacksize;
    
    public void ClearSlot()
    {
        itemData = null;
        itemID = -1;
        stacksize = -1;
    }
    
    public void AssignItem(InventorySlot invSlot)
    {
        if (itemData == invSlot.itemData) AddToStack(invSlot.stacksize);
        else
        {
            itemData = invSlot.itemData;
            itemID = itemData.ID;
            stacksize = 0;
            AddToStack(invSlot.stacksize);
        }
    }

    public void AssignItem(InventoryData data, int amount)
    {
        if (itemData == data) AddToStack(amount);
        else
        {
            itemData = data;
            itemID = data.ID;
            stacksize = 0;
            AddToStack(amount);
        }

    }

    public void AddToStack(int amount)
    {
        stacksize += amount;
    }

    public void RemoveFromStack(int amount)
    {
        stacksize -= amount;
        if (stacksize <= 0) ClearSlot();
    }
    
    public void OnBeforeSerialize()
    {
        
    }

    public void OnAfterDeserialize()
    {
        if (itemID == -1) return;
        {
            var db = Resources.Load<Database>("Database");
            itemData = db.GetItem(itemID);
        }    
    }
}
