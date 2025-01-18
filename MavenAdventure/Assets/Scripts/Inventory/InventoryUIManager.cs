using UnityEngine;
using UnityEngine.InputSystem;

//Code borrowed and Modified by Dan Pos off of the inventory system series from youtube https://www.youtube.com/playlist?list=PL-hj540P5Q1hLK7NS5fTSNYoNJpPWSL24
public class InventoryUIManager : MonoBehaviour
{
    public DynamicInventoryDisplay InventoryPanel;
    //public DynamicInventoryDisplay PlayerBackpackPanel;

    public int inventorySize = 10;
    private void Awake()
    {
        InventoryPanel.gameObject.SetActive(false);
        //PlayerBackpackPanel.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        InventoryHolder.OnDynamicInventoryDisplayRequested += DisplayInventory;
        PlayerInventoryHolder.OnPlayerBackpackDisplayRequested += DisplayPlayerBackpack;
    }
    
    private void OnDisable()
    {
        InventoryHolder.OnDynamicInventoryDisplayRequested -= DisplayInventory;
        PlayerInventoryHolder.OnPlayerBackpackDisplayRequested -= DisplayPlayerBackpack;
    }

    private void Update()
    {
        if (InventoryPanel.gameObject.activeInHierarchy && Keyboard.current.escapeKey.wasPressedThisFrame) InventoryPanel.gameObject.SetActive(false);
        
        //if (PlayerBackpackPanel.gameObject.activeInHierarchy && Keyboard.current.escapeKey.wasPressedThisFrame) PlayerBackpackPanel.gameObject.SetActive(false);
    }

    void DisplayInventory(InventorySystem invToDisplay, int offset)
    {
        InventoryPanel.gameObject.SetActive(true);
        InventoryPanel.RefreshDynamicInventory(invToDisplay, offset);
    }
    
    void DisplayPlayerBackpack(InventorySystem invToDisplay, int offset)
    {
        //PlayerBackpackPanel.gameObject.SetActive(true);
        //PlayerBackpackPanel.RefreshDynamicInventory(invToDisplay, offset);
    }
}
