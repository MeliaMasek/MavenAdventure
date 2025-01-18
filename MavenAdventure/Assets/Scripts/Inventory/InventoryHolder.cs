using UnityEngine;
using UnityEngine.Events;

[System.Serializable]

//Code borrowed and Modified by Dan Pos off of the inventory system series from youtube https://www.youtube.com/playlist?list=PL-hj540P5Q1hLK7NS5fTSNYoNJpPWSL24
public abstract class InventoryHolder : MonoBehaviour
{
    [SerializeField] private int inventorySize;
    [SerializeField] protected InventorySystem primaryInventorySystem;
    [SerializeField] protected int offset = 7;
    //[SerializeField] protected int gold;
    public int Offset => offset;
    
    public InventorySystem PrimaryInventorySystem => primaryInventorySystem;
    
    public static UnityAction<InventorySystem, int> OnDynamicInventoryDisplayRequested; 

    protected virtual void Awake()
    {
        SaveLoad.onLoadGame += LoadInventory;
        
        primaryInventorySystem = new InventorySystem(inventorySize);
    }

    protected abstract void LoadInventory(SaveData saveData);
}
[System.Serializable]
public struct InventorySaveData
{
    public InventorySystem invSystem;
    public Vector3 playerPos;
    public Quaternion playerRot;
    
    public InventorySaveData(InventorySystem _invSystem, Vector3 _playerPos, Quaternion _playerRot)
    { 
        invSystem = _invSystem;
        playerPos = _playerPos;
        playerRot = _playerRot;
    }
    
    public InventorySaveData(InventorySystem _invSystem)
    {
        invSystem = _invSystem;
        playerPos = Vector3.zero;
        playerRot = Quaternion.identity;
    }
}