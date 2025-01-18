using UnityEngine;
using UnityEngine.UI;

public class StoreItemUI : MonoBehaviour
{
    public StoreItem storeItem;
    public Text itemNameText;
    public Text priceText;
    //public Image iconImage;

    private StoreManager storeManager;

    void Start()
    {
        storeManager = FindObjectOfType<StoreManager>();
        itemNameText.text = storeItem.itemName;
        priceText.text = storeItem.price.ToString();
        //iconImage.sprite = storeItem.icon;
    }

    public void OnPurchaseOne()
    {
        storeManager.PurchaseAndApplySet1(storeItem.price);
    }
    
    public void OnPurchaseTwo()
    {
        storeManager.PurchaseAndApplySet2(storeItem.price);
    }
    
    public void OnPurchaseThree()
    {
        storeManager.PurchaseAndApplySet3(storeItem.price);
    }
    
    public void OnPurchaseFour()
    {
        storeManager.PurchaseAndApplySet4(storeItem.price);
    }
}