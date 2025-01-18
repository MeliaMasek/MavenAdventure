using System.Collections.Generic;
using UnityEngine;

//Code borrowed and Modified by Dan Pos off of the inventory system series from youtube https://www.youtube.com/playlist?list=PL-hj540P5Q1hLK7NS5fTSNYoNJpPWSL24
public class StaticInventoryDisplay : InventoryDisplay
{
    [SerializeField] private InventoryHolder inventoryHolder;
    [SerializeField] private SlotUI[] slotArray;
    
    private void OnEnable()
    {
        PlayerInventoryHolder.OnPlayerInventoryChanged += RefreshStaticDisplay;
    }

    private void OnDisable()
    {
        PlayerInventoryHolder.OnPlayerInventoryChanged -= RefreshStaticDisplay;
    }

    public void RefreshStaticDisplay()
    {
        if (inventoryHolder != null)
        {
            inventorySystem = inventoryHolder.PrimaryInventorySystem;
            inventorySystem.OnInventorySlotChanged += UpdateSlot;
        }
        else Debug.LogWarning($"no inventory assigned to this {this.gameObject}");
        
        AssignSlot(inventorySystem, offset: 0);
    }
    
    protected override void Start()
    {
        base.Start();
        RefreshStaticDisplay();
    }
    
    public override void AssignSlot(InventorySystem invToDisplay, int offset)
    {
        slotDictionary = new Dictionary<SlotUI, InventorySlot>();
        
            for (int i = 0; i < inventoryHolder.Offset; i++)
            {
                slotDictionary.Add(slotArray[i], inventorySystem.InventorySlots[i]);
                slotArray[i].Init(inventorySystem.InventorySlots[i]);
            }
    }
    
    public void ClearSlots()
    {
        foreach (var slot in slotArray)
        {
            slot.ClearSlot();
            if (slotDictionary.ContainsKey(slot))
            {
                slotDictionary.Remove(slot);
            }
        }
    }
}
