using UnityEngine;
using UnityEngine.InputSystem;

//Code borrowed and Modified by Dan Pos off of the inventory system series from youtube https://www.youtube.com/playlist?list=PL-hj540P5Q1hLK7NS5fTSNYoNJpPWSL24

public class UIController : MonoBehaviour
{
    [SerializeField] private ShopKeeperDisplay shopKeeperDisplay;
    [SerializeField] private CraftingDisplay craftingDisplay;


    private void Awake()
    {
        shopKeeperDisplay.gameObject.SetActive(false);
        craftingDisplay.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            shopKeeperDisplay.gameObject.SetActive(false);
            craftingDisplay.gameObject.SetActive(false);
        }
    }

    private void OnEnable()
    {
        ShopKeeper.OnShopWindowRequested += DisplayShopWindow;
        CraftingBench.OnCraftingBenchRequested += DisplayCraftingWindow;
    }

    private void OnDisable()
    {
        ShopKeeper.OnShopWindowRequested -= DisplayShopWindow;
        CraftingBench.OnCraftingBenchRequested -= DisplayCraftingWindow;
    }

    private void DisplayShopWindow(ShopSystem shopSystem, PlayerInventoryHolder playerInventory)
    {
        shopKeeperDisplay.gameObject.SetActive(true);
        shopKeeperDisplay.DisplayShopWindow(shopSystem, playerInventory);
    }

    private void DisplayCraftingWindow(CraftingBench craftingBench)
    {
        craftingDisplay.gameObject.SetActive(true);
        craftingDisplay.DisplayCraftingWindow(craftingBench);
    }
    
    public void CloseCraftingWindow()
    {
        craftingDisplay.CloseCraftingWindow();
    }
}
