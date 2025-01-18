using System;
using UnityEngine;

//[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(UniqueID))]

//Code borrowed and Modified by Dan Pos off of the inventory system series from youtube https://www.youtube.com/playlist?list=PL-hj540P5Q1hLK7NS5fTSNYoNJpPWSL24

public class ItemPickup : MonoBehaviour
{
    public float pickupRadius = 1f;
    public InventoryData ItemData;

    private BoxCollider itemCollider;
    private ItemPickupSaveData itemSaveData;
    private string id;

    private void Awake()
    {
        id = Guid.NewGuid().ToString(); // Generate unique ID
        SaveLoad.onLoadGame += LoadGame;
        itemSaveData = new ItemPickupSaveData(id, ItemData, transform.position, transform.rotation);
        
        itemCollider = GetComponent<BoxCollider>();
        itemCollider.isTrigger = true;
        //itemCollider.radius = pickupRadius;
    }

    private void Start()
    {
        SaveGameManager.data.activeItems.Add(id, itemSaveData);
    }

    private void LoadGame(SaveData data)
    {
        if (data.collectedItems.Contains(id)) Destroy(this.gameObject);
    }

    private void OnDestroy()
    {
        if (SaveGameManager.data.activeItems.ContainsKey(id)) SaveGameManager.data.activeItems.Remove(id);
        SaveLoad.onLoadGame -= LoadGame;
    }

    private void OnTriggerEnter(Collider other)
    {
        var inventory = other.transform.GetComponent<PlayerInventoryHolder>();
        if (!inventory) return;

        if (inventory.PrimaryInventorySystem.AddToInventory(ItemData, 1))
        {
            SaveGameManager.data.collectedItems.Add(id);
            Destroy(this.gameObject);
        }
    }
}

[System.Serializable]
public struct ItemPickupSaveData
{
    public string Id; // Unique ID
    public InventoryData ItemData;
    public Vector3 Position;
    public Quaternion Rotation;
    
    public ItemPickupSaveData(string id, InventoryData _itemData, Vector3 _position, Quaternion _rotation)
    {
        Id = id;
        ItemData = _itemData;
        Position = _position;
        Rotation = _rotation;
    } 
}
