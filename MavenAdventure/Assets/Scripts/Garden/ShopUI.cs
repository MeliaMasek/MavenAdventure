using UnityEngine;
using UnityEngine.UI;

public class ShopUI : MonoBehaviour
{
    public ShopManager shopManager;
    public BackpackManager backpackManager;
    public Transform shopItemsParent;
    public GameObject shopItemPrefab;
    public IntData goldText;

    private void Start()
    {
        UpdateShopUI();
    }

    public void UpdateShopUI()
    {
        goldText.text = $"Gold: {shopManager.playerGold.value}"; // Accessing the value of the IntData object

        foreach (Transform child in shopItemsParent)
        {
            Destroy(child.gameObject); // Clear previous items
        }

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
        buyButton.onClick.AddListener(() => shopManager.BuyItem(item));
    }
}